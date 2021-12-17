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
    HatToken,
    BangToken,
    BangEqualsToken,
    EqualsToken,
    EqualsEqualsToken,
    GreaterToken,
    GreaterEqualsToken,
    LessToken,
    LessEqualsToken,
    PipeToken,
    PipePipeToken,
    AmpersandToken,
    AmpersandAmpersandToken,
    
    FalseKeyword,
    TrueKeyword,
    
    WhitespaceToken,
    BadToken,
    EofToken,
    
    UnaryExpression,
    BinaryExpression,
    LiteralExpression,
    GroupedExpression
}