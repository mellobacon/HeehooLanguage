using HeehooLanguage.CodeAnalysis.Expressions;

namespace HeehooLanguage.CodeAnalysis.Resolving;

public class Resolver
{
	private readonly List<string> m_errors = new();
	
	public Resolver(SyntaxTree syntaxTree, IEnumerable<string> currentErrors)
	{
		m_errors.AddRange(currentErrors);
	}

	public IResolvedExpression? ResolveExpression(IExpression expression)
	{
		switch (expression)
		{
			case LiteralExpression literalExpression:
				return ResolveLiteralExpression(literalExpression);
			case UnaryExpression unaryExpression:
				return ResolveUnaryExpression(unaryExpression);
			case BinaryExpression binaryExpression:
				return ResolveBinaryExpression(binaryExpression);
			default:
				m_errors.Add($"lol what? I'm not sure what you're trying to do: {expression.Kind:G}");
				return null;
		}
	}

	private IResolvedExpression ResolveLiteralExpression(LiteralExpression expression)
	{
		var value = expression.Value ?? 0;
		return new ResolvedLiteralExpression(value);
	}

	private IResolvedExpression? ResolveUnaryExpression(UnaryExpression expression)
	{
		var operand = ResolveExpression(expression.Operand);
		var op = ResolvedOperator.Unary.Resolve(expression.Operator.Kind, operand?.ResultType ?? typeof(int));
		if (op is not null)
		{
			return new ResolvedUnaryExpression(op, operand);
		}
		
		m_errors.Add($"no operator found for '{expression.Operator.Token}' and '{operand?.ResultType}' fuckhead");
		return operand;
	}
	
	private IResolvedExpression? ResolveBinaryExpression(BinaryExpression expression)
	{
		var left = ResolveExpression(expression.Left);
		var right = ResolveExpression(expression.Right);
		var op = ResolvedOperator.Binary.Resolve(expression.Operator.Kind, left?.ResultType ?? typeof(object), right?.ResultType ?? typeof(object));

		if (op is not null)
		{
			return new ResolvedBinaryExpression(left, op, right);
		}
		
		m_errors.Add("no operator found for this shit");
		return left;
	}
}