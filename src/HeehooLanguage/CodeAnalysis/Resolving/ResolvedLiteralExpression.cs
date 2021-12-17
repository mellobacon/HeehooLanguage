namespace HeehooLanguage.CodeAnalysis.Resolving;

public class ResolvedLiteralExpression : IResolvedExpression
{
	public ResolvedLiteralExpression(object? value)
	{
		Value = value;
	}

	public object? Value { get; }
	public ResolvedKind Kind => ResolvedKind.LiteralExpression;
	public Type ResultType => Value?.GetType() ?? typeof(object);
}