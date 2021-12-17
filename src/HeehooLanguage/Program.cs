using System.Drawing;
using HeehooLanguage.CodeAnalysis;
using HeehooLanguage.CodeAnalysis.Resolving;
using WaifuShork.Common.Extensions;

var lexer = new Lexer("1 + 2 * 3");
var parser = new Parser(lexer);
var syntaxTree = parser.Parse(out var errors);
if (syntaxTree is null)
{
    if (errors.Count > 0)
    {
        foreach (var error in errors)
        {
            Console.Out.WriteLine(error.ColorizeForeground(Color.DarkRed));
        }
    }
    else
    {
        Console.WriteLine("unable to properly build syntax tree");
    }
    return;
}

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
    syntaxTree.WriteTo(Console.Out);
    return;
}

foreach (var error in errors)
{
    Console.Out.WriteLine(error.ColorizeForeground(Color.DarkRed));
}