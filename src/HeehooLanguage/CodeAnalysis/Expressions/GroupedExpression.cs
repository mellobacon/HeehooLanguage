namespace HeehooLanguage.CodeAnalysis.Expressions;

public class GroupedExpression : IExpression
{
	public GroupedExpression(SyntaxToken openParen, IExpression expression, SyntaxToken closeParen)
	{
		OpenParen = openParen;
		Expression = expression;
		CloseParen = closeParen;
	}
	
	public SyntaxToken OpenParen { get; }
	public IExpression Expression { get; }
	public SyntaxToken CloseParen { get; }
	
	public SyntaxKind Kind => SyntaxKind.GroupedExpression;
	public IEnumerable<ISyntaxNode> GetChildren()
	{
		yield return OpenParen;
		yield return Expression;
		yield return CloseParen;
	}
}