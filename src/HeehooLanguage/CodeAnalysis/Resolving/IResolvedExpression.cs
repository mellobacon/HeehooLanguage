namespace HeehooLanguage.CodeAnalysis.Resolving;

public interface IResolvedExpression : IResolvedNode
{
	Type ResultType { get; }
}