namespace HeehooLanguage.CodeAnalysis.Expressions;

public class UnaryExpression : IExpression
{
	public UnaryExpression(SyntaxToken op, IExpression operand)
	{
		Operator = op;
		Operand = operand;
	}
	
	public SyntaxToken Operator { get; }
	public IExpression Operand { get; }
	
	public SyntaxKind Kind => SyntaxKind.UnaryExpression;
	public IEnumerable<ISyntaxNode> GetChildren()
	{
		yield return Operator;
		yield return Operand;
	}
}