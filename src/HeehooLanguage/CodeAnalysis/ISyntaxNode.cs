namespace HeehooLanguage.CodeAnalysis;

public interface ISyntaxNode
{
	public SyntaxKind Kind { get; }
	public IEnumerable<ISyntaxNode> GetChildren();
}