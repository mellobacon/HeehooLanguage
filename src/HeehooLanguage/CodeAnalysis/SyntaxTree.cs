using System.Drawing;
using HeehooLanguage.CodeAnalysis.Expressions;
using WaifuShork.Common.Extensions;

namespace HeehooLanguage.CodeAnalysis;

public class SyntaxTree
{
	public SyntaxTree(IExpression root)
	{
		Root = root;
	}
	
	public IExpression Root { get; }

	public void WriteTo(TextWriter writer)
	{
		PrettyPrintTo(writer, Root);
	}
	
	private static void PrettyPrintTo(TextWriter writer, ISyntaxNode node, string indent = "", bool isLast = true)
	{
		var marker = isLast 
			? "└──".ColorizeForeground(Color.Coral) 
			: "├──".ColorizeForeground(Color.Coral);
		
		writer.Write(indent);
		writer.Write(marker);


		writer.Write($"{node.Kind:G}".ColorizeForeground(Color.Magenta));
		if (node is SyntaxToken token && token.Value is not null)
		{
			writer.Write(": ".ColorizeForeground(Color.Magenta));
			writer.Write($"{token.Value}".ColorizeForeground(Color.Aquamarine));
		}
		
		writer.WriteLine();
		
		indent += isLast ? "    " : "│   ".ColorizeForeground(Color.Coral);

		var lastChild = node.GetChildren().LastOrDefault();
		foreach (var child in node.GetChildren())
		{
			PrettyPrintTo(writer, child, indent, child == lastChild);
		}
	}
}