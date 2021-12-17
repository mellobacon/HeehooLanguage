using HeehooLanguage.CodeAnalysis.Expressions;

namespace HeehooLanguage.CodeAnalysis;

public class Parser
{
	private readonly List<SyntaxToken> m_tokens = new();
	private readonly List<string> m_errors = new();

	// Just like the Lexer, we keep track of the current token that we have access to (aka currently parsing),
	// since recursive descent parsers are pretty intensive, it's best to avoid leaping back and forwards through
	// the token stream rapidly, if you're more curious about an "enumerating lexer and parser", we can set one up
	// later and use that if you'd like.
	private int m_position;
	
	public Parser(Lexer lexer)
	{
		while (true)
		{
			var token = lexer.Lex();
			if (token.Kind == SyntaxKind.BadToken || token.Kind == SyntaxKind.WhitespaceToken)
			{
				continue;
			}

			if (token.Kind == SyntaxKind.EofToken)
			{
				break;
			}
			
			m_tokens.Add(token);
		}
	}

	// these will be used in the future
	private SyntaxToken Previous => this[-1]; // The token right before Current
	private SyntaxToken Current => this[0]; // the current token we're parsing 
	private SyntaxToken Next => this[1]; // the token right after Current

	// I already told you this is an "Indexer" in chat, but if you want more information, here at the docs:
	// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/
	private SyntaxToken this[int offset]
	{
		get
		{
			var index = m_position + offset;
			if (index >= m_tokens.Count)
			{
				// this is just the "index from end" operator, when you do ^n, it means you want to return the 
				// nth value starting *from* the end, so think of it as indexing an array but the start index is
				// the END, if that makes sense. here's some more documentation on ranges:
				// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/ranges
				return m_tokens[^1];
			}

			return m_tokens[index];
		}
	}
	
	// The root parsing function, attempts to recursively build an abstract syntax tree
	public SyntaxTree? Parse(out IReadOnlyList<string> errors)
	{
		// The standard parsing process goes as follows:
		// ParseTerm: +, -, low precedence operators
		// ParseFactor: *, /, %, high precedence operators
		// ParsePrimary: Anything *else* that counts as an expression, but isn't immediately noticeable as one
		// such as grouped expressions, it's why '3 * (2 + 2) = 12', instead of 8.
		var rootExpression = ParseExpression();
		errors = m_errors; // after we've built the tree, we need to send out our errors
		
		if (rootExpression is null)
		{
			return null;
		}
		return new SyntaxTree(rootExpression);
	}
	
	// We'll always start with Unary first, so let's just make it named properly,
	// and keep the ParseUnaryExpression function for organizational sake. 
	private IExpression? ParseExpression(int parentPrecedence = 0)
	{
		return ParseUnaryExpression(parentPrecedence);
	}
 
	// Since "ParseExpression" with the precedence is called, we need to know the precedence of
	// the previously parsed expression, so we can act accordingly, this stuff is kind of confusing
	// but the more you look at it, the easier it starts to get.
	private IExpression? ParseUnaryExpression(int parentPrecedence = 0)
	{
		IExpression? left;
		// let's grab our precedence level for the current token we're sitting on
		var unaryPrecedence = Current.Kind.LookupUnaryPrecedence();
		// If the precedence is 0, it's not a valid operator token, because no operator
		// has 0 precedence. If OUR precedence is at least as high or greater than what was previous,
		// we can do a normal left to right evaluation
		if (unaryPrecedence != 0 && unaryPrecedence >= parentPrecedence)
		{
			var op = Consume(); // grab our operator
			
			// This is probably where it can look confusing, and this is where recursion comes into play,
			// as of right now the, we've decided that this current expression will be unary, so (-1) or-
			// something along those lines, we have the operator to perform the operation, but not the actual
			// value to use the operator on, by calling ParseExpression, and passing the current precedence,
			// we effectively restart the process, until we run out of tokens.
			var operand = ParseExpression(unaryPrecedence); 
			// just a happy lil' null check :)
			if (operand is null)
			{
				return null;
			}
			// now we know for sure 2 things:
			// 1) our operator type
			// 2) our full expression that this operator is applied to
			left = new UnaryExpression(op, operand);
		}
		else
		{
			// otherwise we just get the expression for the left side, you can basically re-read above to understand,
			// or ping me for more questions about this
			left = ParsePrimary();
		}

		// So now we have the left hand expression of a binary expression, something like (1 + 1), we have our (1), we
		// now need our (+) and (1) to complete the expression
		return ParseBinaryExpression(left);
	}

	// so we step into this method
	private IExpression? ParseBinaryExpression(IExpression? left, int parentPrecedence = 0)
	{
		// lets do some happy cartwheel loops 
		while (true)
		{
			// lets grab our current tokens precedence value 
			var binaryPrecedence = Current.Kind.LookupBinaryPrecedence();
			// if our current precedence is 0, or less than/equal to the previous one, we want to just break,
			// and return the left value, look above into unary ^^ once we return, that completes the unary expression,
			// which then completes ParseExpression, and our syntax tree is done.
			if (binaryPrecedence == 0 || binaryPrecedence <= parentPrecedence)
			{
				break;
			}
			
			// same stuff as before, taking our current precedence and just recursively going through our tokens into a tree structure
			var op = Consume();
			var right = ParseExpression(binaryPrecedence);
			if (left is null || right is null)
			{
				return null;
			}
			
			left = new BinaryExpression(left, op, right);
		}

		// this is what gets returned when we break if our precedence level is zero or less than or equal to the parent precedence
		return left;
	}

	// Parse primary is where lose ends get tied up for expressions
	private IExpression? ParsePrimary()
	{
		switch (Current.Kind)
		{
			// if we have an open parenthesis in an expression, it means we're about to parse a grouped expression, remember from before ^^
			case SyntaxKind.OpenParenToken:
			{
				// eat the token
				var left = Consume(); 
				// what's this? starting the process over again? yep, same as before
				var expression = ParseExpression();
				// lets ensure that our closing parenthesis exists, otherwise we'd have a syntax error
				var right = MatchWith(SyntaxKind.CloseParenToken);
				if (expression is null)
				{
					return null;
				}
				// and now we spit back our group expression, which returns from unary, which then returns the root expression 
				return new GroupedExpression(left, expression, right);
			}
			// You know these
			case SyntaxKind.FalseKeyword:
			case SyntaxKind.TrueKeyword:
			{
				var keyword = Consume();
				// if our token type is the true keyword, it's guaranteed to represent a truthy value,
				// otherwise, the only logical option is for it to be false
				var value = keyword.Kind == SyntaxKind.TrueKeyword;
				// value assessed above ^
				return new LiteralExpression(keyword, value);
			}
			case SyntaxKind.NumberToken:
			{
				// Just ensure that we have a number token, nothing fancy
				var number = MatchWith(SyntaxKind.NumberToken);
				return new LiteralExpression(number);
			}
			default:
				return null;
		}
	}
	
	// I'm sure you can figure out these methods
	private SyntaxToken Consume()
	{
		var current = Current;
		m_position++;
		return current;
	}

	private SyntaxToken MatchWith(SyntaxKind kind)
	{
		if (Current.Kind == kind)
		{
			return Consume();
		}

		m_errors.Add($"error: lol wrong token fuck wit, I got <{Current.Kind}>, but I wanted <{kind}>");
		return new SyntaxToken("ERROR", kind, null);
	}
}