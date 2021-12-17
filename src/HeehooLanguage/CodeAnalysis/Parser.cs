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
	
	public SyntaxTree Parse(out IReadOnlyList<string> errors)
	{
		var rootExpression = ParseTerm();
		errors = m_errors;
		return new SyntaxTree(rootExpression);
	}

	private IExpression ParseExpression()
	{
		return ParseTerm();
	}

	private IExpression ParseTerm()
	{
		var left = ParseFactor();
		while (Current.Kind == SyntaxKind.PlusToken ||
		       Current.Kind == SyntaxKind.MinusToken)
		{
			var op = Consume();
			var right = ParseFactor();
			left = new BinaryExpression(left, op, right);
		}

		return left;
	}

	private IExpression ParseFactor()
	{
		var left = ParsePrimary();

		while (Current.Kind == SyntaxKind.StarToken ||
		       Current.Kind == SyntaxKind.SlashToken ||
		       Current.Kind == SyntaxKind.ModuloToken)
		{
			var op = Consume();
			var right = ParsePrimary();
			left = new BinaryExpression(left, op, right);
		}

		return left;
	}

	private IExpression ParsePrimary()
	{
		switch (Current.Kind)
		{
			case SyntaxKind.OpenParenToken:
			{
				var left = Consume();
				var expression = ParseExpression();
				var right = MatchWith(SyntaxKind.CloseParenToken);
				return new GroupedExpression(left, expression, right);
			}
		}
		
		var number = MatchWith(SyntaxKind.NumberToken);
		return new NumberExpression(number);
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