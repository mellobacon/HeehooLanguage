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

    private void LexNumberTokens()
    {
        object? value = null;
        while (char.IsDigit(Current) || Current == '.')
        {
            Advance(1);
        }

        int length = _position - _start;
        string text = _text.Substring(_start, length);

        if (text.Contains('.'))
        {
            if (float.TryParse(text, out float f))
            {
                value = f;   
            }
        }
        else
        {
            if (int.TryParse(text, out int i))
            {
                value = i;
            }
        }

        _kind = value != null ? SyntaxKind.NumberToken : SyntaxKind.BadToken;

        _value = value;
    }

    private void LexWhitespaceTokens()
    {
        while (char.IsWhiteSpace(Current))
        {
            Advance(1);
        }

        _kind = SyntaxKind.WhitespaceToken;
    }

    private void LexLetterTokens()
    {
        while (char.IsLetter(Current))
        {
            Advance(1);
        }
        int length = _position - _start;
        string text = _text.Substring(_start, length);
        _kind = SyntaxFacts.GetKeywordType(text);
        _value = _kind != SyntaxKind.FalseKeyword;
    }

    public SyntaxToken Lex()
    {
        _start = _position;
        _kind = SyntaxKind.BadToken;
        _value = null;

        switch (Current)
        {
            case '\0':
                _kind = SyntaxKind.EofToken;
                break;
            case '(':
                _kind = SyntaxKind.OpenParenToken;
                Advance(1);
                break;
            case ')':
                _kind = SyntaxKind.CloseParenToken;
                Advance(1);
                break;
            case '^':
                _kind = SyntaxKind.HatToken;
                Advance(1);
                break;
            case '*':
                _kind = SyntaxKind.StarToken;
                Advance(1);
                break;
            case '/':
                _kind = SyntaxKind.SlashToken;
                Advance(1);
                break;
            case '%':
                _kind = SyntaxKind.ModuloToken;
                Advance(1);
                break;
            case '+':
                _kind = SyntaxKind.PlusToken;
                Advance(1);
                break;
            case '-':
                _kind = SyntaxKind.MinusToken;
                Advance(1);
                break;
            case '!':
                if (Peek(1) == '=')
                {
                    _kind = SyntaxKind.BangEqualsToken;
                    Advance(2);
                    break;
                }
                _kind = SyntaxKind.BangToken;
                Advance(1);
                break;
            case '=':
                if (Peek(1) == '=')
                {
                    _kind = SyntaxKind.EqualsEqualsToken;
                    Advance(2);
                    break;
                }

                _kind = SyntaxKind.EqualsToken;
                Advance(1);
                break;
            case '>':
                if (Peek(1) == '=')
                {
                    _kind = SyntaxKind.GreaterEqualsToken;
                    Advance(2);
                    break;
                }

                _kind = SyntaxKind.GreaterToken;
                Advance(1);
                break;
            case '<':
                if (Peek(1) == '=')
                {
                    _kind = SyntaxKind.LessEqualsToken;
                    Advance(2);
                    break;
                }

                _kind = SyntaxKind.LessToken;
                Advance(1);
                break;
            case '|':
                if (Peek(1) == '|')
                {
                    _kind = SyntaxKind.PipePipeToken;
                    Advance(2);
                    break;
                }

                _kind = SyntaxKind.PipeToken;
                Advance(1);
                break; 
            case '&':
                if (Peek(1) == '&')
                {
                    _kind = SyntaxKind.AmpersandAmpersandToken;
                    Advance(2);
                    break;
                }

                _kind = SyntaxKind.AmpersandToken;
                Advance(1);
                break;
            default:
                if (char.IsDigit(Current) || Current == '.')
                {
                    LexNumberTokens();
                }
                else if (char.IsWhiteSpace(Current))
                {
                    LexWhitespaceTokens();
                }
                else if (char.IsLetter(Current))
                {
                    LexLetterTokens();
                }
                else
                {
                    Advance(1);
                }
                break;
        }
        string? text = SyntaxFacts.GetText(_kind);
        
        if (text == null)
        {
            int length = _position - _start;
            text = _text.Substring(_start, length);
        }
        return new SyntaxToken(text, _kind, _value);
    }
}
