namespace HeehooLanguage.CodeAnalysis;

public class Lexer
{
    private readonly string _text;
    private int _start;
    private int _position;
    private SyntaxKind _kind;
    private object? _value;
    private char Current => Peek(0);
    
    public Lexer(string text)
    {
        _text = text;
    }

    private char Peek(int offset)
    {
        int index = _position + offset;
        return index >= _text.Length ? '\0' : _text[index];
    }

    private void Advance(int offset)
    {
        _position += offset;
    }

    private string LexNumberTokens()
    {
        var dotcount = 0;
        object? value = null;
        while (char.IsDigit(Current) || Current == '.')
        {
            if (Current == '.')
            {
                dotcount++;
            }
            Advance(1);
        }

        int length = _position - _start;
        string text = _text.Substring(_start, length);

        if (float.TryParse(text, out float f))
        {
            value = f;   
        }
        else if (int.TryParse(text, out int i))
        {
            value = i;
        }

        _kind = value != null ? SyntaxKind.NumberToken : SyntaxKind.BadToken;

        _value = value;
        return text;
    }

    private void LexWhitespaceTokens()
    {
        while (char.IsWhiteSpace(Current))
        {
            Advance(1);
        }

        _kind = SyntaxKind.WhitespaceToken;
    }

    public SyntaxToken Lex()
    {
        _start = _position;
        _kind = SyntaxKind.BadToken;
        _value = null;
        string? text = null;

        switch (Current)
        {
            case '\0':
                _kind = SyntaxKind.EofToken;
                break;
            case '+':
                _kind = SyntaxKind.PlusToken;
                text = "+";
                Advance(1);
                break;
            case '-':
                _kind = SyntaxKind.MinusToken;
                text = "-";
                Advance(1);
                break;
            case '*':
                _kind = SyntaxKind.StarToken;
                text = "*";
                Advance(1);
                break;
            case '/':
                _kind = SyntaxKind.SlashToken;
                text = "/";
                Advance(1);
                break;
            case '%':
                _kind = SyntaxKind.ModuloToken;
                text = "%";
                Advance(1);
                break;
            default:
                if (char.IsDigit(Current))
                {
                    text = LexNumberTokens();
                }
                else if (char.IsWhiteSpace(Current))
                {
                    LexWhitespaceTokens();
                    text = "█";
                }
                break;
        }

        return new SyntaxToken(text, _kind, _value);
    }
}