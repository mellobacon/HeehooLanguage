namespace HeehooLanguage.CodeAnalysis.Resolving;

public enum ResolvedOperatorKind
{
	// unary: (operator)(expression)
	Identity,
	Negation,
	LogicalNegation,
	
	// binary: (expression)(operator)(expression)
	Addition,
	Subtraction,
	Multiplication,
	Division,
	Remainder,
	BitwiseAnd,
	LogicalAnd,
	BitwiseOr,
	LogicalOr,
	Equals,
	NotEquals
}