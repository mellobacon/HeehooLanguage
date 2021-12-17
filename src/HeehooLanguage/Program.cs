using System.Drawing;
using HeehooLanguage.CodeAnalysis;
using HeehooLanguage.CodeAnalysis.Resolving;
using WaifuShork.Common.Extensions;

var lexer = new Lexer("1 + 2 + 3");
var parser = new Parser(lexer);
var syntaxTree = parser.Parse(out var errors);
var resolver = new Resolver(syntaxTree, errors);
var expression = resolver.ResolveExpression(syntaxTree.Root);
if (expression is null)
{
    Console.Out.WriteLine("welp, you fucked up".ColorizeForeground(Color.DarkRed));
    return;
}

var interpreter = new Interpreter(expression);
var value = interpreter.Interpret();
Console.Out.WriteLine(value ?? "interpreter most likely failed :/");

if (!errors.Any())
{
    SyntaxTree.WriteTo(Console.Out, syntaxTree.Root);
    return;
}

foreach (var error in errors)
{
    Console.Out.WriteLine(error.ColorizeForeground(Color.DarkRed));
}