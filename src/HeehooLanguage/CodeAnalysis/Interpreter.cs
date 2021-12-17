using System.Linq.Expressions;
using HeehooLanguage.CodeAnalysis.Expressions;
using HeehooLanguage.CodeAnalysis.Resolving;
using JetBrains.Annotations;

namespace HeehooLanguage.CodeAnalysis;

public class Interpreter
{
	private readonly IResolvedExpression m_root;

	public Interpreter(IResolvedExpression root)
	{
		m_root = root;
	}

	public object? Interpret()
	{
		return InterpretExpression(m_root);
	}

	private object? InterpretExpression(IResolvedExpression? expression)
	{
		if (expression is null)
		{
			return null;
		}
		
		switch (expression)
		{
			case ResolvedLiteralExpression literal:
			{
				return literal.Value;
			}
			case ResolvedUnaryExpression unary:
			{
				var operand = InterpretExpression(unary.Operand);
				if (operand is null)
				{
					return null;
				}
				switch (unary.Operator.OperatorKind)
				{
					case ResolvedOperatorKind.Identity:
						return operand;
					case ResolvedOperatorKind.Negation:
						if (!operand.GetType().IsValueType)
						{
							return null;
						}
						if (operand is bool boolOp)
						{
							return !boolOp;
						}
						return -(dynamic)operand;
					case ResolvedOperatorKind.LogicalNegation:
						return !(bool)operand;
					default:
						return null;
				}
			}
			case ResolvedBinaryExpression binary:
			{
				var left = InterpretExpression(binary.Left);
				var right = InterpretExpression(binary.Right);

				if (left is null || right is null)
				{
					return null;
				}
				
				switch (binary.Operator.OperatorKind)
				{
					case ResolvedOperatorKind.Addition:
						return ObjectMath.Add(left, right);
					case ResolvedOperatorKind.Subtraction:
						return ObjectMath.Subtract(left, right);
					case ResolvedOperatorKind.Multiplication:
						return ObjectMath.Multiply(left, right);
					case ResolvedOperatorKind.Division:
						return ObjectMath.Divide(left, right);
					case ResolvedOperatorKind.Remainder:
						return ObjectMath.Mod(left, right);	
					case ResolvedOperatorKind.Equals:
						return ObjectMath.Equals(left, right);
					case ResolvedOperatorKind.NotEquals:
						return ObjectMath.NotEquals(left, right);
					case ResolvedOperatorKind.LogicalAnd:
						return ObjectMath.LogicalAnd(left, right);
					case ResolvedOperatorKind.LogicalOr:
						return ObjectMath.LogicalOr(left, right);
					case ResolvedOperatorKind.BitwiseAnd:
						return ObjectMath.BitwiseAnd(left, right);
					case ResolvedOperatorKind.BitwiseOr:
						return ObjectMath.BitwiseOr(left, right);
					default:
						return null;
				}
			}
			default:
				return null;
		}
	}
}