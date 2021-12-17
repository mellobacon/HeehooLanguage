namespace HeehooLanguage.CodeAnalysis.Resolving;

public class ResolvedOperator
{
	public class Binary
	{
		private Binary(SyntaxKind syntaxKind, ResolvedOperatorKind opKind, Type type)
			: this(syntaxKind, opKind, type, type, type) { }

		private Binary(SyntaxKind syntaxKind, ResolvedOperatorKind opKind, Type operandType, Type resultType)
			: this(syntaxKind, opKind, operandType, operandType, resultType) { }
		
		public Binary(SyntaxKind syntaxKind, ResolvedOperatorKind opKind, Type leftType, Type rightType, Type resultType)
		{
			SyntaxKind = syntaxKind;
			OperatorKind = opKind;
			LeftType = leftType;
			RightType = rightType;
			ResultType = resultType;
		}
		
		public SyntaxKind SyntaxKind { get; }
		public ResolvedOperatorKind OperatorKind { get; }
		public Type LeftType { get; }
		public Type RightType { get; }
		public Type ResultType { get; }

		private static readonly Binary[] s_operators =
		{
			// int <op> int
			new(SyntaxKind.PlusToken, ResolvedOperatorKind.Addition, typeof(int)),
			new(SyntaxKind.MinusToken, ResolvedOperatorKind.Subtraction, typeof(int)),
			new(SyntaxKind.StarToken, ResolvedOperatorKind.Multiplication, typeof(int)),
			new(SyntaxKind.SlashToken, ResolvedOperatorKind.Division, typeof(int)),
			new(SyntaxKind.ModuloToken, ResolvedOperatorKind.Remainder, typeof(int)),
			
			// float <op> float
			new(SyntaxKind.PlusToken, ResolvedOperatorKind.Addition, typeof(float)),
			new(SyntaxKind.MinusToken, ResolvedOperatorKind.Subtraction, typeof(float)),
			new(SyntaxKind.StarToken, ResolvedOperatorKind.Multiplication, typeof(float)),
			new(SyntaxKind.SlashToken, ResolvedOperatorKind.Division, typeof(float)),
			new(SyntaxKind.ModuloToken, ResolvedOperatorKind.Remainder, typeof(float)),
			
			// float <op> int
			new(SyntaxKind.PlusToken, ResolvedOperatorKind.Addition, typeof(float), typeof(int), typeof(float)),
			new(SyntaxKind.MinusToken, ResolvedOperatorKind.Subtraction, typeof(float), typeof(int), typeof(float)),
			new(SyntaxKind.StarToken, ResolvedOperatorKind.Multiplication, typeof(float), typeof(int), typeof(float)),
			new(SyntaxKind.SlashToken, ResolvedOperatorKind.Division, typeof(float), typeof(int), typeof(float)),
			new(SyntaxKind.ModuloToken, ResolvedOperatorKind.Remainder, typeof(float), typeof(int), typeof(float)),

			// int <op> float
			new(SyntaxKind.PlusToken, ResolvedOperatorKind.Addition, typeof(int), typeof(float), typeof(float)),
			new(SyntaxKind.MinusToken, ResolvedOperatorKind.Subtraction, typeof(int), typeof(float), typeof(float)),
			new(SyntaxKind.StarToken, ResolvedOperatorKind.Multiplication, typeof(int), typeof(float), typeof(float)),
			new(SyntaxKind.SlashToken, ResolvedOperatorKind.Division, typeof(int), typeof(float), typeof(float)),
			new(SyntaxKind.ModuloToken, ResolvedOperatorKind.Remainder, typeof(int), typeof(float), typeof(float)),
			
			// bitwise operations can't be performed on floating point values, only integers and booleans
			// int <op> int
			new(SyntaxKind.HatToken, ResolvedOperatorKind.BitwiseOr, typeof(int), typeof(int), typeof(int)),
			new(SyntaxKind.AmpersandToken, ResolvedOperatorKind.BitwiseAnd, typeof(int), typeof(int), typeof(int)),
			// bool <op> bool
			new(SyntaxKind.HatToken, ResolvedOperatorKind.BitwiseOr, typeof(bool), typeof(bool), typeof(bool)),
			new(SyntaxKind.AmpersandToken, ResolvedOperatorKind.BitwiseAnd, typeof(bool), typeof(bool), typeof(bool)),
			
			// bool <op> bool 
			new(SyntaxKind.EqualsEqualsToken, ResolvedOperatorKind.Equals, typeof(bool), typeof(bool), typeof(bool)),
			// int <op> int
			new(SyntaxKind.EqualsEqualsToken, ResolvedOperatorKind.Equals, typeof(int), typeof(int), typeof(bool)),
			// float <op> float
			new(SyntaxKind.EqualsEqualsToken, ResolvedOperatorKind.Equals, typeof(float), typeof(float), typeof(bool)),
			// int <op> float
			new(SyntaxKind.EqualsEqualsToken, ResolvedOperatorKind.Equals, typeof(int), typeof(float), typeof(bool)),
			// float <op> int
			new(SyntaxKind.EqualsEqualsToken, ResolvedOperatorKind.Equals, typeof(float), typeof(int), typeof(bool)),
			
			// bool <op> bool 
			new(SyntaxKind.BangEqualsToken, ResolvedOperatorKind.NotEquals, typeof(bool), typeof(bool), typeof(bool)),
			// int <op> int
			new(SyntaxKind.BangEqualsToken, ResolvedOperatorKind.NotEquals, typeof(int), typeof(int), typeof(bool)),
			// float <op> float
			new(SyntaxKind.BangEqualsToken, ResolvedOperatorKind.NotEquals, typeof(float), typeof(float), typeof(bool)),
			// int <op> float
			new(SyntaxKind.BangEqualsToken, ResolvedOperatorKind.NotEquals, typeof(int), typeof(float), typeof(bool)),
			// float <op> int
			new(SyntaxKind.BangEqualsToken, ResolvedOperatorKind.NotEquals, typeof(float), typeof(int), typeof(bool)),
			
			// bool <op> bool
			new(SyntaxKind.AmpersandAmpersandToken, ResolvedOperatorKind.LogicalAnd, typeof(bool), typeof(bool), typeof(bool)),
			new(SyntaxKind.AmpersandAmpersandToken, ResolvedOperatorKind.LogicalOr, typeof(bool), typeof(bool), typeof(bool)),
		};

		public static Binary? Resolve(SyntaxKind syntaxKind, Type leftType, Type rightType)
		{
			return s_operators.FirstOrDefault(op => 
				op.SyntaxKind == syntaxKind && 
				op.LeftType == leftType && 
				op.RightType == rightType);
		}
	}

	public class Unary
	{
		private Unary(SyntaxKind syntaxKind, ResolvedOperatorKind opKind, Type operandType)
			: this(syntaxKind, opKind, operandType, operandType) {}
		private Unary(SyntaxKind syntaxKind, ResolvedOperatorKind opKind, Type operand, Type resultType)
		{
			SyntaxKind = syntaxKind;
			OperatorKind = opKind;
			OperandType = operand;
			ResultType = resultType;
		}
		
		public SyntaxKind SyntaxKind { get; }
		public ResolvedOperatorKind OperatorKind { get; }
		public Type OperandType { get; }
		public Type ResultType { get; }

		private static readonly Unary[] s_operators =
		{
			// <op> bool
			new(SyntaxKind.BangToken, ResolvedOperatorKind.LogicalNegation, typeof(bool)),
			
			// <op> int
			new(SyntaxKind.MinusToken, ResolvedOperatorKind.Negation, typeof(int)),
			// <op> float
			new(SyntaxKind.MinusToken, ResolvedOperatorKind.Negation, typeof(float)),
			// <op> bool
			new(SyntaxKind.MinusToken, ResolvedOperatorKind.Negation, typeof(bool)),
			
			// <op> int
			new(SyntaxKind.PlusToken, ResolvedOperatorKind.Identity, typeof(int)),
			
			// <op> int
			new(SyntaxKind.PlusToken, ResolvedOperatorKind.Identity, typeof(float)),
		};

		public static Unary? Resolve(SyntaxKind syntaxKind, Type operandType)
		{
			return s_operators.FirstOrDefault(op => 
				op.SyntaxKind == syntaxKind && 
				op.OperandType == operandType);
		}
	}
}