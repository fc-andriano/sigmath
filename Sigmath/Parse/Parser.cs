using Sigmath.Lex;
using Sigmath.Parse.Abstract;

using System;
using System.Collections.Generic;

namespace Sigmath.Parse
{
	public sealed class Parser(Source source, int scanningOffset = 0)
	{
		private readonly Lexer _lexer = new(source);

		private TokenCode _code;
		private TokenKind _kind;

		private readonly Stack<TokenCode> _stack = new(2);

		/* =---- Static Methods ----------------------------------------= */

		public static ConstantNatural ParseConstantNatural(string lexeme, int radix = 10)
			=> ConstantNatural.Parse(lexeme, radix);

		public static ConstantInteger ParseConstantInteger(string lexeme, int radix = 10)
			=> ConstantInteger.Parse(lexeme, radix);

		// --------------------------------------------------------------

		public static BinaryExpressionOperator GetBinaryExpressionOperator(TokenCode op)
		{
			BinaryExpressionOperator binop;

			switch (op)
			{
			case TokenCode.PunctPlus:
				binop = BinaryExpressionOperator.Add;
				break;

			case TokenCode.PunctMinus:
				binop = BinaryExpressionOperator.Subtract;
				break;

			case TokenCode.PunctStar:
				binop = BinaryExpressionOperator.Multiply;
				break;

			case TokenCode.PunctSlash:
				binop = BinaryExpressionOperator.Divide;
				break;

			case TokenCode.PunctPercent:
				binop = BinaryExpressionOperator.Modulo;
				break;

			default:
				throw new InvalidOperationException();
			}

			return binop;
		}

		// --------------------------------------------------------------

		public static bool IsMorePrecedent(TokenCode op1, TokenCode op2)
			=> GetBinaryExpressionOperator(op1).GetPrecedence().CompareTo(GetBinaryExpressionOperator(op2).GetPrecedence()) > 0;

		/* =---- Methods -----------------------------------------------= */

		private void Next()
			=> _stack.Push(_code = _lexer.Lex(scanningOffset));

		// --------------------------------------------------------------

		public TokenCode GetPeekToken()
		{
			if (_stack.Count == 0)
				_stack.Push(_code = _lexer.Lex(scanningOffset));
			else
				_code = _stack.Peek();

			return _code;
		}

		public TokenCode GetNextToken()
		{
			if (_stack.Count == 0)
				_code = _lexer.Lex(scanningOffset);
			else
				_code = _stack.Pop();

			return _code;
		}

		// --------------------------------------------------------------

		public TokenKind GetPeekTokenKind()
			=> this.GetPeekToken().GetTokenKind();

		public TokenKind GetNextTokenKind()
			=> this.GetNextToken().GetTokenKind();

		// --------------------------------------------------------------

		// Parser Methods

		private ConstantNatural ParseConstantNatural(int radix = 10)
			=> ParseConstantNatural(_lexer.GetLexeme(), radix);

		private ConstantInteger ParseConstantInteger(int radix = 10)
			=> ParseConstantInteger(_lexer.GetLexeme(), radix);

		// --------------------------------------------------------------

		public Constant ParseConstant()
		{
			Constant result;

			switch (this.GetNextToken())
			{
			case TokenCode.ConstBinInteger:
				result = this.ParseConstantInteger(2);
				break;

			case TokenCode.ConstOctInteger:
				result = this.ParseConstantInteger(8);
				break;

			case TokenCode.ConstDecInteger
			  or TokenCode.ConstInteger:
				result = this.ParseConstantInteger(10);
				break;

			case TokenCode.ConstHexInteger:
				result = this.ParseConstantInteger(16);
				break;

			default:
				throw new InvalidOperationException();
			}

			return result;
		}

		// --------------------------------------------------------------

		private Expression ParseLeftOperand()
		{
			Expression result;

			switch (this.GetPeekTokenKind())
			{
			case TokenKind.Constant:
				result = this.ParseConstant();
				break;

			case TokenKind.UnaryOperator:
				result = this.ParseUnaryExpression(this.GetNextToken());
				break;

			default:
				throw new InvalidOperationException();
			}

			return result;
		}

		private Expression ParseRightOperand(TokenCode op)
		{
			Expression result, rhs = this.ParseLeftOperand();

			switch (this.GetPeekTokenKind())
			{
			case TokenKind.BinaryOperator:
				if (IsMorePrecedent(_code, op))
					result = this.ParseBinaryExpression(this.GetNextToken(), rhs);
				else
					goto default;

				break;

			default:
				result = rhs;
				break;
			}

			return result;
		}

		// --------------------------------------------------------------

		private Expression ParseUnaryExpression(TokenCode op)
		{
			throw new NotImplementedException();
		}

		private BinaryExpression ParseBinaryExpression(TokenCode op, Expression lhs)
			=> new(GetBinaryExpressionOperator(op), lhs, this.ParseRightOperand(op));

		// --------------------------------------------------------------

		public Expression ParseExpression()
		{
			Expression result, lhs = this.ParseLeftOperand();

			switch (this.GetPeekTokenKind())
			{
			case TokenKind.Invalid:
				throw new InvalidOperationException();

			case TokenKind.BinaryOperator:
				do
					lhs = this.ParseBinaryExpression(this.GetNextToken(), lhs);
				while (this.GetPeekTokenKind() == TokenKind.BinaryOperator);

				result = lhs;

				break;

			default:
				result = lhs;
				break;
			}

			return result;
		}

		// --------------------------------------------------------------

		public Statement Parse()
		{
			Statement result;

			switch (this.GetPeekTokenKind())
			{
			case TokenKind.Constant
			  or TokenKind.Variable:
				result = this.ParseExpression();
				break;

			default:
				throw new InvalidOperationException();
			}

			return result;
		}

		/* =------------------------------------------------------------= */
	}
}
