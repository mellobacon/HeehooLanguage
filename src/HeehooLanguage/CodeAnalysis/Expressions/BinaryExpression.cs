namespace HeehooLanguage.CodeAnalysis.Expressions;

public class BinaryExpression : IExpression
{
	public BinaryExpression(IExpression left, SyntaxToken op, IExpression right)
	{
		Left = left;
		Operator = op;
		Right = right;
	}
	
	public IExpression Left { get; }
	public SyntaxToken Operator { get; }
	public IExpression Right { get; }

	public SyntaxKind Kind => SyntaxKind.BinaryExpression;
	
	public IEnumerable<ISyntaxNode> GetChildren()
	{
		yield return Left;
		yield return Operator;
		yield return Right;
	}
}