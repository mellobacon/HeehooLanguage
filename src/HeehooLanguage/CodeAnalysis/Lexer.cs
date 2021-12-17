namespace HeehooLanguage.CodeAnalysis;

// NOTE: Hi future Mello, I've left you some improvement notes, whether you follow them or not,
// is entirely up to you, since your job is the lexer, I just thought I'd review it.
// TODO(1): Biggest not, REPORT ERRORS, please make sure to report errors, so you can pass them on
// to the parser, that way we can append errors to the parser, resolver, and interpreter. 

// TODO(2): Consider checking !IsAtEnd in all of your while loops when going over text, just to 
// make sure that you never end up attempting to read memory that doesn't exist.
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
        checked
        {
            int index = _position + offset; 
            return index >= _text.Length ? '\0' : _text[index];
        }
    }

    private void Advance(int offset)
    {
        checked
        {
            _position += offset;
        }
    }

    private void LexNumberTokens()
    {
        object? value = null;
        // add support for '_' in numbers, but  remember you can't have two consecutive underscores back to back
        // 1_000_000 is valid, but 1__000__000 is not. Same for ensuring that multiple decimals aren't used, like
        // 12..69 or even 12._69, consider these possibilities and how the Lexer would react to these tokens. 
        while (char.IsDigit(Current) || Current == '.')
        {
            Advance(1);
        }

        int length;
        checked
        {
            length = _position - _start;
        }
        
        // just a miner note if you want, you don't need to calculate the length if you want,
        // you can use '_text[_start.._position], the range will be Start include, and
        // End exclusive -- meaning it ranges from the start position to just *before*
        string text = _text.Substring(_start, length);
        
        // why only float? what about double?, what about supporting
        // numbers like 42.12f or 69.7d
        if (text.Contains('.')) 
        {
            if (float.TryParse(text, out float f))
            {
                value = f;   
            }
        }
        else // why only int32? 
        {
            if (int.TryParse(text, out int i))
            {
                value = i;
            }
        }

        // consider using "is not" over "!=" for null checks
        _kind = value is not null ? SyntaxKind.NumberToken : SyntaxKind.BadToken;
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
        _kind = text.GetKind();
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
                // Why in the default case? why not use `case var _ when char.IsDigit(Current) || Current == '.':'
                // Also don't forget to allow '_' for numbers AND the identifiers, like variables starting with underscore.
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
        
        string? text = _kind.GetText();
        // I changed this to string.IsNullOrWhiteSpace because you should be checking 
        // if a string is null, contains only whitespace characters (even zero width),
        // and if it's just empty (aka length == 0)
        if (string.IsNullOrWhiteSpace(text))
        {
            // checked again here, because what happens if we end up overflowing?
            checked
            {
                int length = _position - _start;
                text = _text.Substring(_start, length);
            }
        }
        return new SyntaxToken(text, _kind, _value);
    }
}
