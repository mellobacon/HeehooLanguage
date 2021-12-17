using System.Drawing;
using HeehooLanguage.CodeAnalysis;
using WaifuShork.Common.Extensions;

var lexer = new Lexer("1 + 2 + 3");
var parser = new Parser(lexer);
var syntaxTree = parser.Parse(out var errors);
if (!errors.Any())
{
    SyntaxTree.WriteTo(Console.Out, syntaxTree.Root);
    return;
}

foreach (var error in errors)
{
    Console.Out.WriteLine(error.ColorizeForeground(Color.DarkRed));
}


/*while (true)
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
}*/