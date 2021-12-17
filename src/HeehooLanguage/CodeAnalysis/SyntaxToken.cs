namespace HeehooLanguage.CodeAnalysis;

public struct SyntaxToken
{
    public string? Token { get; }
    public SyntaxKind Kind { get; }
    public object? Value { get; }
    
    public SyntaxToken(string? token, SyntaxKind kind, object? value)
    {
        Token = token;
        Kind = kind;
        Value = value;
    }
}