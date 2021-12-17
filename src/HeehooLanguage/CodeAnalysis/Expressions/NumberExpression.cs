namespace HeehooLanguage.CodeAnalysis.Expressions;

public class NumberExpression : IExpression
{
	public NumberExpression(SyntaxToken number)
	{
		Number = number;
	}
	
	public SyntaxToken Number { get; }
	
	public SyntaxKind Kind => SyntaxKind.NumberExpression;
	public IEnumerable<ISyntaxNode> GetChildren()
	{
		yield return Number;
	}
}