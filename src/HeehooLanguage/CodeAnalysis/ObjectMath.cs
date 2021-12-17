namespace HeehooLanguage.CodeAnalysis;

public static class ObjectMath
{
	public static object? Add(object left, object right)
	{
		if (left.GetType().IsValueType && right.GetType().IsValueType)
		{
			return (dynamic)left + (dynamic)right;
		}

		return default;
	}

	public static object? Subtract(object left, object right)
	{
		if (left.GetType().IsValueType && right.GetType().IsValueType)
		{
			return (dynamic)left - (dynamic)right;
		}

		return default;
	}

	public static object? Multiply(object left, object right)
	{
		if (left.GetType().IsValueType && right.GetType().IsValueType)
		{
			return (dynamic)left * (dynamic)right;
		}

		return default;
	}
	
	public static object? Divide(object left, object right)
	{
		if (left.GetType().IsValueType && right.GetType().IsValueType)
		{
			return (dynamic)left / (dynamic)right;
		}

		return default;
	}
	
	public static object? Mod(object left, object right)
	{
		if (left.GetType().IsValueType && right.GetType().IsValueType)
		{
			return (dynamic)left % (dynamic)right;
		}

		return default;
	}

	public new static bool Equals(object left, object right)
	{
		if (left.GetType().IsValueType && right.GetType().IsValueType)
		{
			return (dynamic)left == (dynamic)right;
		}

		return default;
	}

	public static bool NotEquals(object left, object right)
	{
		return !Equals(left, right);
	}

	public static object? LogicalAnd(object left, object right)
	{
		if (left.GetType().IsValueType && right.GetType().IsValueType)
		{
			return (dynamic)left && (dynamic)right;
		}

		return default;
	}
	
	public static object? LogicalOr(object left, object right)
	{
		if (left.GetType().IsValueType && right.GetType().IsValueType)
		{
			return (dynamic)left || (dynamic)right;
		}

		return default;
	}
	
	public static object? BitwiseAnd(object left, object right)
	{
		if (left.GetType().IsValueType && right.GetType().IsValueType)
		{
			return (dynamic)left & (dynamic)right;
		}

		return default;
	}
	
	public static object? BitwiseOr(object left, object right)
	{
		if (left.GetType().IsValueType && right.GetType().IsValueType)
		{
			return (dynamic)left | (dynamic)right;
		}

		return default;
	}
}