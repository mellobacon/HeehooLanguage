using HeehooLanguage.CodeAnalysis.Expressions;

namespace HeehooLanguage.CodeAnalysis;

public class Parser
{
	private readonly List<SyntaxToken> m_tokens = new();
	private readonly List<string> m_errors = new();

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
	private SyntaxToken Previous => this[-1];
	private SyntaxToken Current => this[0];
	private SyntaxToken Next => this[1];

	private SyntaxToken this[int offset]
	{
		get
		{
			var index = m_position + offset;
			if (index >= m_tokens.Count)
			{
				return m_tokens[^1];
			}

			return m_tokens[index];
		}
	}
	
	// The root parsing function, attempts to recursively build an abstract syntax tree
	public SyntaxTree? Parse(out IReadOnlyList<string> errors)
	{
		var rootExpression = ParseExpression();
		errors = m_errors;
		
		if (rootExpression is null)
		{
			return null;
		}
		return new SyntaxTree(rootExpression);
	}
	
	private IExpression? ParseExpression(int previousPrecedence = 0)
	{
		return ParseUnaryExpression(previousPrecedence);
	}
 
	private IExpression? ParseUnaryExpression(int previousPrecedence = 0)
	{
		IExpression? left;
		var unaryPrecedence = Current.Kind.LookupUnaryPrecedence();
		if (unaryPrecedence != 0 && unaryPrecedence >= previousPrecedence)
		{
			var op = Consume();
			var operand = ParseExpression(unaryPrecedence);
			if (operand is null)
			{
				return null;
			}
			left = new UnaryExpression(op, operand);
		}
		else
		{
			left = ParsePrimary();
		}

		return ParseBinaryExpression(left);
	}

	private IExpression? ParseBinaryExpression(IExpression? left, int previousPrecedence = 0)
	{
		while (true)
		{
			var binaryPrecedence = Current.Kind.LookupBinaryPrecedence();
			if (binaryPrecedence == 0 || binaryPrecedence <= previousPrecedence)
			{
				break;
			}

			var op = Consume();
			var right = ParseExpression(binaryPrecedence);
			if (left is null || right is null)
			{
				return null;
			}
			
			left = new BinaryExpression(left, op, right);
		}

		return left;
	}

	private IExpression? ParsePrimary()
	{
		switch (Current.Kind)
		{
			case SyntaxKind.OpenParenToken:
			{
				var left = Consume();
				var expression = ParseExpression();
				var right = MatchWith(SyntaxKind.CloseParenToken);
				if (expression is null)
				{
					return null;
				}
				return new GroupedExpression(left, expression, right);
			}
			case SyntaxKind.FalseKeyword:
			case SyntaxKind.TrueKeyword:
			{
				var keyword = Consume();
				// if our token type is the true keyword, it's guaranteed to represent a truthy value,
				// otherwise, the only logical option is for it to be false
				var value = keyword.Kind == SyntaxKind.TrueKeyword;
				return new LiteralExpression(keyword, value);
			}
			case SyntaxKind.NumberToken:
			{
				var number = MatchWith(SyntaxKind.NumberToken);
				return new LiteralExpression(number);
			}
			default:
			{
				return null;
			}
		}
	}
	

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