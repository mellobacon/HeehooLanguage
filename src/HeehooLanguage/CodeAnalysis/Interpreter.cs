using HeehooLanguage.CodeAnalysis.Expressions;
using HeehooLanguage.CodeAnalysis.Resolving;

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
				switch (unary.Operator.OperatorKind)
				{
					case ResolvedOperatorKind.Identity:
					{
						return ResolveIdentity(operand);
					}
					case ResolvedOperatorKind.Negation:
					{
						return ResolveNegation(operand);
					}
					case ResolvedOperatorKind.LogicalNegation:
					{
						return ResolveLogicalNegation(operand);
					}
					default:
						return null;
				}
			}
			case ResolvedBinaryExpression binary:
			{
				var left = InterpretExpression(binary.Left);
				var right = InterpretExpression(binary.Right);

				switch (binary.Operator.OperatorKind)
				{
					case ResolvedOperatorKind.Addition:
						return ResolveAddition(left, right);
					case ResolvedOperatorKind.Subtraction:
						return ResolveSubtraction(left, right);
					case ResolvedOperatorKind.Multiplication:
						return ResolveMultiplication(left, right);
					case ResolvedOperatorKind.Division:
						return ResolveDivision(left, right);
					case ResolvedOperatorKind.Remainder:
						return ResolveRemainder(left, right);
					case ResolvedOperatorKind.Equals:
						return ResolveEquals(left, right);
					case ResolvedOperatorKind.NotEquals:
						return ResolveNotEquals(left, right);
					case ResolvedOperatorKind.LogicalAnd:
						return ResolveLogicalAnd(left, right);
					case ResolvedOperatorKind.LogicalOr:
						return ResolveLogicalOr(left, right);
					case ResolvedOperatorKind.BitwiseAnd:
						return ResolveBitwiseAnd(left, right);
					case ResolvedOperatorKind.BitwiseOr:
						return ResolveBitwiseOr(left, right);
					default:
						return null;
				}
			}
			default:
				return null;
		}
	}

	private object? ResolveAddition(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}

		if (left is int && right is int)
		{
			return (int) left + (int) right;
		}
		if (left is float && right is int)
		{
			return (float)left + (int)right;
		}
		if (left is int && right is float)
		{
			return (int)left + (float)right;
		}

		if (left is float && right is float)
		{
			return (float)left + (float)right;
		}
		return null;
	}

	private object? ResolveSubtraction(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}
		
		if (left is int && right is int)
		{
			return (int) left - (int) right;
		}
		if (left is float && right is int)
		{
			return (float)left - (int)right;
		}
		if (left is int && right is float)
		{
			return (int)left - (float)right;
		}

		if (left is float && right is float)
		{
			return (float)left - (float)right;
		}
		return null;
	}

	private object? ResolveMultiplication(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}
		
		if (left is int && right is int)
		{
			return (int) left * (int) right;
		}
		if (left is float && right is int)
		{
			return (float)left * (int)right;
		}
		if (left is int && right is float)
		{
			return (int)left * (float)right;
		}
		if (left is float && right is float)
		{
			return (float)left * (float)right;
		}

		return null;
	}

	private object? ResolveDivision(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}
		
		if (left is int && right is int)
		{
			return (int) left / (int) right;
		}
		if (left is float && right is int)
		{
			return (float)left / (int)right;
		}
		if (left is int && right is float)
		{
			return (int)left / (float)right;
		}
		if (left is float && right is float)
		{
			return (float)left / (float)right;
		}
		return null;
	}

	private object? ResolveRemainder(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}
		
		if (left is int && right is int)
		{
			return (int) left % (int) right;
		}
		if (left is float && right is int)
		{
			return (float)left % (int)right;
		}
		if (left is int && right is float)
		{
			return (int)left % (float)right;
		}

		return null;
	}

	private object? ResolveEquals(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}
		
		if (left is int && right is int)
		{
			return (int)left == (int)right;
		}
		if (left is float && right is int)
		{
			return Math.Abs((float)left - (int)right) < float.Epsilon;
		}
		if (left is int && right is float)
		{
			return Math.Abs((int)left - (float)right) < float.Epsilon;
		}
		if (left is float && right is float)
		{
			return Math.Abs((float)left - (float)right) < float.Epsilon;
		}

		return null;
	}

	private object? ResolveNotEquals(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}
		
		if (left is int && right is int)
		{
			return (int)left != (int)right;
		}
		if (left is float && right is int)
		{
			return Math.Abs((float)left - (int)right) > float.Epsilon;
		}
		if (left is int && right is float)
		{
			return Math.Abs((int)left - (float)right) > float.Epsilon;
			
		}
		if (left is float && right is float)
		{
			return Math.Abs((float)left - (float)right) > float.Epsilon;
		}

		return null;
	}
	
	private object? ResolveLogicalAnd(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}

		if (left is bool && right is bool)
		{
			return (bool)left && (bool)right;
		}

		return null;
	}

	private object? ResolveLogicalOr(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}

		if (left is bool && right is bool)
		{
			return (bool)left || (bool)right;
		}

		return null;
	}

	private object? ResolveBitwiseAnd(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}

		if (left is int && right is int)
		{
			return (int)left & (int)right;
		}
		if (left is bool && right is bool)
		{
			return (bool)left & (bool)right;
		}

		return null;
	}

	private object? ResolveBitwiseOr(object? left, object? right)
	{
		if (left is null || right is null)
		{
			return null;
		}

		if (left is int && right is int)
		{
			return (int)left | (int)right;
		}
		if (left is bool && right is bool)
		{
			return (bool)left | (bool)right;
		}

		return null;
	}
	
	private object? ResolveIdentity(object? operand)
	{
		if (operand is null)
		{
			return null;
		}

		if (operand is int intOp)
		{
			return intOp;
		}

		if (operand is float floatOp)
		{
			return floatOp;
		}

		if (operand is bool boolOp)
		{
			return boolOp;
		}

		return null;
	}

	private object? ResolveNegation(object? operand)
	{
		if (operand is null)
		{
			return null;
		}	
		
		if (operand is int intOp)
		{
			return -intOp;
		}

		if (operand is float floatOp)
		{
			return -floatOp;
		}

		if (operand is bool boolOp)
		{
			return !boolOp;
		}

		return null;
	}

	private object? ResolveLogicalNegation(object? operand)
	{
		if (operand is null)
		{
			return null;
		}

		if (operand is bool boolOp)
		{
			return boolOp;
		}

		return null;
	}
}