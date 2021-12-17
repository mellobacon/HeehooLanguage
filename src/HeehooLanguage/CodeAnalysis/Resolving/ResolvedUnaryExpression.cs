namespace HeehooLanguage.CodeAnalysis.Resolving;

public class ResolvedUnaryExpression : IResolvedExpression
{
	public ResolvedUnaryExpression(ResolvedOperator.Unary op, IResolvedExpression? operand)
	{
		Operator = op;
		Operand = operand;
	}
	
	public ResolvedOperator.Unary Operator { get; }
	public IResolvedExpression? Operand { get; }
	
	public ResolvedKind Kind => ResolvedKind.UnaryExpression;
	public Type ResultType => Operator.ResultType;
}