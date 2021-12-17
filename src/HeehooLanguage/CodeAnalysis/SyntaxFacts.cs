namespace HeehooLanguage.CodeAnalysis;

public static class SyntaxFacts
{
	private static readonly Dictionary<SyntaxKind, int> s_unaryPrecedences = new()
	{
		{SyntaxKind.MinusToken, 9},
		{SyntaxKind.PlusToken, 9},
		{SyntaxKind.BangToken, 9},
	};

	private static readonly Dictionary<SyntaxKind, int> s_binaryPrecedences = new()
	{
		{SyntaxKind.StarToken, 8},
		{SyntaxKind.SlashToken, 8},
		{SyntaxKind.ModuloToken, 8},

		{SyntaxKind.PlusToken, 7},
		{SyntaxKind.MinusToken, 7},
		
		{SyntaxKind.GreaterToken, 6},
		{SyntaxKind.GreaterEqualsToken, 6},
		{SyntaxKind.LessToken, 6},
		{SyntaxKind.LessEqualsToken, 6},
		
		{SyntaxKind.EqualsEqualsToken, 5},
		{SyntaxKind.BangEqualsToken, 5},
		
		{SyntaxKind.HatToken, 4},
		
		{SyntaxKind.PipeToken, 3},
		
		{SyntaxKind.AmpersandToken, 2},
		
		{SyntaxKind.PipePipeToken, 1},
	};

	public static int LookupUnaryPrecedence(this SyntaxKind kind)
	{
		if (s_unaryPrecedences.TryGetValue(kind, out var value))
		{
			return value;
		}
		
		return 0;
	}

	public static int LookupBinaryPrecedence(this SyntaxKind kind)
	{
		if (s_binaryPrecedences.TryGetValue(kind, out var value))
		{
			return value;
		}

		return 0;
	}
	
	public static SyntaxKind GetKind(this string text)
	{
		return text switch
		{
			"true" => SyntaxKind.TrueKeyword,
			"false" => SyntaxKind.FalseKeyword,
			_ => SyntaxKind.BadToken 
		};
	}
	
	public static string? GetText(this SyntaxKind kind)
	{
		return kind switch
		{
			SyntaxKind.PlusToken => "+",
			SyntaxKind.MinusToken => "-",
			SyntaxKind.StarToken => "*",
			SyntaxKind.SlashToken => "/",
			SyntaxKind.ModuloToken => "%",
			SyntaxKind.OpenParenToken => "(",
			SyntaxKind.CloseParenToken => ")",
			SyntaxKind.HatToken => "^",
			SyntaxKind.BangToken => "!",
			SyntaxKind.EqualsToken => "=",
			SyntaxKind.BangEqualsToken => "!=",
			SyntaxKind.EqualsEqualsToken => "==",
			SyntaxKind.GreaterToken => ">",
			SyntaxKind.GreaterEqualsToken => ">=",
			SyntaxKind.LessToken => "<",
			SyntaxKind.LessEqualsToken => "<=",
			SyntaxKind.PipeToken => "|",
			SyntaxKind.PipePipeToken => "||",
			SyntaxKind.AmpersandToken => "&",
			SyntaxKind.AmpersandAmpersandToken => "&&",
			SyntaxKind.TrueKeyword => "true",
			SyntaxKind.FalseKeyword => "false",
			_ => null
		};
	}
	
}