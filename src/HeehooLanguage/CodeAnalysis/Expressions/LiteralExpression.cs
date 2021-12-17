namespace HeehooLanguage.CodeAnalysis.Expressions;

public class LiteralExpression : IExpression
{
	public LiteralExpression(SyntaxToken literal)
		: this(literal, literal.Value)
	{
	}

	public LiteralExpression(SyntaxToken literal, object? value)
	{
		Literal = literal;
		Value = value;
	}
	
	public SyntaxToken Literal { get; }
	public object? Value { get; }
	
	public SyntaxKind Kind => SyntaxKind.LiteralExpression;
	public IEnumerable<ISyntaxNode> GetChildren()
	{
		yield return Literal;
	}
}