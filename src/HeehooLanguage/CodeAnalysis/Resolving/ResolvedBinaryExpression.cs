namespace HeehooLanguage.CodeAnalysis.Resolving;

public class ResolvedBinaryExpression : IResolvedExpression
{
	public ResolvedBinaryExpression(IResolvedExpression? left, ResolvedOperator.Binary op, IResolvedExpression? right)
	{
		Left = left;
		Operator = op;
		Right = right;
	}
	
	public IResolvedExpression? Left { get; }
	public ResolvedOperator.Binary Operator { get; }
	public IResolvedExpression? Right { get; }

	public ResolvedKind Kind => ResolvedKind.BinaryExpression;
	public Type ResultType => Operator.ResultType;
}