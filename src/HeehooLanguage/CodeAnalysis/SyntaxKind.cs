namespace HeehooLanguage.CodeAnalysis;

public enum SyntaxKind
{
    OpenParenToken,
    CloseParenToken,
    
    NumberToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    ModuloToken,
    OpenParenToken,
    ClosedParenToken,
    HatToken,
    
    WhitespaceToken,
    BadToken,
    EofToken,
    
    BinaryExpression,
    NumberExpression,
    GroupedExpression
}