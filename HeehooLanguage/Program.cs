using HeehooLanguage.CodeAnalysis;

var lexer = new Lexer("1 + 2 + 3");

while (true)
{
    SyntaxToken token = lexer.Lex();
    
    if (token.Kind == SyntaxKind.EofToken)
    {
        break;
    }
    
    if (token.Value == null)
    {
        Console.Write($"[Token {token.Token}: Type: {token.Kind}]");
    }
    else
    {
        Console.Write($"[Token: {token.Token} Type: {token.Kind} Value: {token.Value}]");
    }
    Console.WriteLine();
}