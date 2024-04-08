namespace Sigmath.Lex
{
	public sealed class Lexer(Source source)
	{
		private readonly Scanner _scanner = new(source);

		/* =---- Static Methods ----------------------------------------= */

		public static Lexer CreateFromText(string text)
			=> new(SourceReader.CreateFromText(text));

		public static Lexer CreateFromFile(string path)
			=> new(SourceReader.CreateFromFile(path));

		/* =---- Methods -----------------------------------------------= */

		public string GetLexeme()
			=> _scanner.GetLexeme();

		public string GetLexeme(int startIndex, int length)
			=> _scanner.GetLexeme(startIndex, length);

		// --------------------------------------------------------------

		public TokenCode LexWhitespaces(int offset)
		{
			if (_scanner.Match(Charset.IsWhitespace, offset))
			{
				int count = 0;

				do
					count++;
				while (_scanner.Match(Charset.IsWhitespace, count + offset));

				_scanner.Skip(count);
			}

			return TokenCode.Ignore;
		}

		public TokenCode LexInlineComment(int offset)
		{
			int count = 0;

			do
				count++;
			while (!_scanner.Match('\n', count + offset));

			_scanner.Skip(count);

			return TokenCode.Ignore;
		}

		public TokenCode LexMultilineComment(int offset)
		{
			int count = 0;

			do
				count++;
			while (!(_scanner.Match('*', count + offset) && _scanner.Match('/', count + offset + 1)));

			_scanner.Skip(count + 1);

			return TokenCode.Ignore;
		}

		// --------------------------------------------------------------

		private TokenCode LexNumberFractionalPart(int offset)
		{
			TokenCode result;

			if (_scanner.Scan(Charset.IsDigit, offset))
			{
				if (_scanner.PassNumberExponentSuffix(Charset.IsDigit, offset))
					result = TokenCode.ConstExpReal;
				else
					result = TokenCode.ConstReal;
			}
			else
			{
				throw LexerException.GetInvalidError(this, "number fractional part");
			}

			return result;
		}

		private TokenCode LexNumber(int offset)
		{
			TokenCode result;

			if (_scanner.Pass('0', offset))
			{
				switch (_scanner.Source.Peek(offset))
				{
				case 'B' or 'b':
					if (_scanner.PassNumberWithBasePrefix('b', Charset.IsBinaryDigit, offset))
						result = TokenCode.ConstBinInteger;
					else
						throw LexerException.GetInvalidError(this, "binary number");

					break;

				case 'C' or 'c':
					if (_scanner.PassNumberWithBasePrefix('c', Charset.IsOctalDigit, offset))
						result = TokenCode.ConstOctInteger;
					else
						throw LexerException.GetInvalidError(this, "octal number");

					break;

				case 'D' or 'd':
					if (_scanner.PassNumberWithBasePrefix('d', Charset.IsDigit, offset))
						result = TokenCode.ConstDecInteger;
					else
						throw LexerException.GetInvalidError(this, "decimal number");

					break;

				case 'X' or 'x':
					if (_scanner.PassNumberWithBasePrefix('x', Charset.IsHexadecimalDigit, offset))
						result = TokenCode.ConstHexInteger;
					else
						throw LexerException.GetInvalidError(this, "hexadecimal number");

					break;

				case '.':
					_scanner.Consume(offset);
					result = this.LexNumberFractionalPart(offset);

					break;

				default:
					if (_scanner.Match('0', offset))
					{
						int count = 0;

						do
							count++;
						while (_scanner.Match('0', offset + count));

						_scanner.Skip(count);
					}

					result = TokenCode.ConstInteger;

					break;
				}
			}
			else
			{
				if (_scanner.Scan(Charset.IsDigit, offset))
				{
					if (_scanner.Scan('.', offset))
						result = this.LexNumberFractionalPart(offset);
					else
						result = TokenCode.ConstInteger;
				}
				else
				{
					throw LexerException.GetInvalidError(this, "number");
				}
			}

			return result;
		}

		// --------------------------------------------------------------

		private TokenCode LexPunctuator(int offset)
		{
			TokenCode result;

			switch (_scanner.Source.PeekChar(offset))
			{
			case '~':
				_scanner.Skip();

				if (_scanner.Pass('=', offset))
					result = TokenCode.PunctTildeEqual;
				else
					result = TokenCode.PunctTilde;

				break;

			case '?':
				_scanner.Skip();
				result = TokenCode.PunctQuestion;
				break;

			case '!':
				_scanner.Skip();

				if (_scanner.Pass('=', offset))
					result = TokenCode.PunctExclaimEqual;
				else
					result = TokenCode.PunctExclaim;

				break;

			case '&':
				_scanner.Skip();

				if (_scanner.Pass('&', offset))
					result = TokenCode.PunctAmpAmp;
				else if (_scanner.Pass('=', offset))
					result = TokenCode.PunctAmpEqual;
				else
					result = TokenCode.PunctAmp;

				break;

			case '|':
				_scanner.Skip();

				if (_scanner.Pass('|', offset))
					result = TokenCode.PunctPipePipe;
				else if (_scanner.Pass('=', offset))
					result = TokenCode.PunctPipeEqual;
				else
					result = TokenCode.PunctPipe;

				break;

			case '^':
				_scanner.Skip();

				if (_scanner.Pass('^', offset))
					result = TokenCode.PunctCaretCaret;
				else if (_scanner.Pass('=', offset))
					result = TokenCode.PunctCaretEqual;
				else
					result = TokenCode.PunctCaret;

				break;

			case '=':
				_scanner.Skip();

				if (_scanner.Pass('=', offset))
					result = TokenCode.PunctEqualEqual;
				else if (_scanner.Pass('>', offset))
					result = TokenCode.PunctBigArrow;
				else
					result = TokenCode.PunctEqual;

				break;

			case '+':
				_scanner.Skip();

				if (_scanner.Pass('+', offset))
					result = TokenCode.PunctPlusPlus;
				else if (_scanner.Pass('-', offset))
					result = TokenCode.PunctPlusMinus;
				else if (_scanner.Pass('=', offset))
					result = TokenCode.PunctPlusEqual;
				else
					result = TokenCode.PunctPlus;

				break;

			case '-':
				_scanner.Skip();

				if (_scanner.Pass('-', offset))
					result = TokenCode.PunctMinusMinus;
				else if (_scanner.Pass('+', offset))
					result = TokenCode.PunctMinusPlus;
				else if (_scanner.Pass('=', offset))
					result = TokenCode.PunctMinusEqual;
				else
					result = TokenCode.PunctMinus;

				break;

			case '*':
				_scanner.Skip();

				if (_scanner.Pass('*', offset))
				{
					if (_scanner.Pass('=', offset))
						result = TokenCode.PunctStarStarEqual;
					else
						result = TokenCode.PunctStarStar;
				}
				else if (_scanner.Pass('=', offset))
					result = TokenCode.PunctStarEqual;
				else
					result = TokenCode.PunctStar;

				break;

			case '/':
				_scanner.Skip();

				if (_scanner.Pass('/', offset))
					result = this.LexInlineComment(offset);
				else if (_scanner.Pass('*', offset))
					result = this.LexMultilineComment(offset);
				else if (_scanner.Pass('=', offset))
					result = TokenCode.PunctSlashEqual;
				else
					result = TokenCode.PunctSlash;

				break;

			case '%':
				_scanner.Skip();

				if (_scanner.Pass('=', offset))
					result = TokenCode.PunctPercentEqual;
				else
					result = TokenCode.PunctPercent;

				break;

			case '<':
				_scanner.Skip();

				if (_scanner.Pass('<', offset))
				{
					if (_scanner.Pass('=', offset))
						result = TokenCode.PunctLeftEqual;
					else
						result = TokenCode.PunctLeft;
				}
				else if (_scanner.Pass('-', offset))
					result = TokenCode.PunctLeftArrow;
				else if (_scanner.Pass('=', offset))
					result = TokenCode.PunctLessEqual;
				else
					result = TokenCode.PunctLess;

				break;

			case '>':
				_scanner.Skip();

				if (_scanner.Pass('>', offset))
				{
					if (_scanner.Pass('=', offset))
						result = TokenCode.PunctRightEqual;
					else
						result = TokenCode.PunctRight;
				}
				else if (_scanner.Pass('=', offset))
					result = TokenCode.PunctGreaterEqual;
				else
					result = TokenCode.PunctGreater;

				break;

			case '.':
				_scanner.Skip();

				if (_scanner.Pass('.', offset))
				{
					if (_scanner.Pass('.', offset))
						result = TokenCode.PunctEllipsis;
					else
						result = TokenCode.PunctDotDot;
				}
				else
					result = TokenCode.PunctDot;

				break;

			case ':':
				_scanner.Skip();

				if (_scanner.Pass(':', offset))
					result = TokenCode.PunctColonColon;
				else if (_scanner.Pass('=', offset))
					result = TokenCode.PunctColonEqual;
				else
					result = TokenCode.PunctColon;

				break;

			case ';':
				_scanner.Skip();
				result = TokenCode.PunctSemicolon;
				break;

			case ',':
				_scanner.Skip();
				result = TokenCode.PunctComma;
				break;

			case '(':
				_scanner.Skip();

				if (_scanner.Pass(')', offset))
					result = TokenCode.PunctForall;
				else
					result = TokenCode.BrackLParent;

				break;

			case ')':
				_scanner.Skip();
				result = TokenCode.BrackRParent;
				break;

			case '[':
				_scanner.Skip();

				if (_scanner.Pass(']', offset))
				{
					if (_scanner.Pass('!', offset))
						result = TokenCode.PunctExistsOnly;
					else
						result = TokenCode.PunctExists;
				}
				else
					result = TokenCode.BrackLSquare;

				break;

			case ']':
				_scanner.Skip();
				result = TokenCode.BrackRSquare;
				break;

			case '{':
				_scanner.Skip();
				result = TokenCode.BrackLBraces;
				break;

			case '}':
				_scanner.Skip();
				result = TokenCode.BrackRBraces;
				break;

			default:
				throw LexerException.GetInvalidError(this, "punctuator");
			}

			return result;
		}

		// --------------------------------------------------------------

		public TokenCode Lex(int offset)
		{
			this.LexWhitespaces(offset);
			_scanner.ClearLexeme();

			TokenCode result;

			switch (_scanner.Source.Peek(offset))
			{
			case Source.EOF:
				result = TokenCode.EndOfFile;
				break;

			case (>= '0') and (<= '9'):
				result = this.LexNumber(offset);
				break;

			case '~' or '?' or '!' or '&' or '|' or '^'
			  or '=' or '+' or '-' or '*' or '/' or '%'
			  or '<' or '>' or '.' or ':' or ';' or ','
			  or '(' or ')' or '[' or ']' or '{' or '}':
				result = this.LexPunctuator(offset);
				break;

			default:
				result = TokenCode.Uknown;
				break;
			}

			return result;
		}

		/* =------------------------------------------------------------= */
	}
}
