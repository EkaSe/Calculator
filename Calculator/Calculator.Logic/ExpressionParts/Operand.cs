using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Operand
	{
		protected double value;

		public Operand () {}
		public Operand (double number) {
			value = number;
		}

		public double Value {
			get { 
				return this.value;
			}
			set {
				this.value = value;
			}
		}

		protected double Evaluate () {
			return this.value;
		}

		public int Search (string input, int startPosition) {
			double number = 0;
			double currentDecimal = -1;
			double mantissaLength = 1;
			bool operandEnd = false;
			int i = startPosition;
			int endPosition = -1;
			while (!operandEnd) {
				char currentSymbol = input [i];
				int currentDigit = Parser.CharToDigit (currentSymbol);
				if (currentDigit >= 0) {
					currentDecimal = (double) currentDigit;
					if (mantissaLength == 1)
						number = number * 10 + currentDecimal;
					else {
						number = number + currentDecimal * mantissaLength;
						mantissaLength *= 0.1;
					}
				} else {
					if (currentSymbol == '.')
						mantissaLength = 0.1;
					else if (currentDecimal >= 0) {
						operandEnd = true;
						endPosition = i - 1;
					}
				}
				i++;
				if (i == input.Length) {
					operandEnd = true;
					if (currentDecimal >= 0)
						endPosition = input.Length - 1;
				}
			}
			if (endPosition > 0)
				this.value = number;
			return endPosition;
		}
	}

	public class OperandSearch {
		static public int Run (string input, int startPosition, out Operand operand) {
			operand = null;
			int i = startPosition;
			if (startPosition >= input.Length) 
				return -1;
			int endPosition = -1;
			if (input [startPosition] == '-' || input [startPosition] == '+' || input [startPosition] == ' ') 
				i++;
			char currentSymbol = input [i];
			if (Parser.IsLetter (currentSymbol) || currentSymbol == '_') {
				string alias = null;
				endPosition = Parser.FindName (input, startPosition, out alias);
				double number = 0;
				BuiltInFunc BIF = null;
				if (BIFSearch.Run (alias, out BIF) && endPosition != input.Length - 1 && input [endPosition + 1] == '(') {
					//treat aliasString as variable if no arguments follow?
					string arguments; 
					endPosition = Parser.FindClosingParenthesis (input, endPosition + 1, out arguments);
					BIF.SetArguments (arguments);
					operand = BIF;
				} // else variable search
				endPosition = Parser.FindAlias (input, i, out number);
				operand = new Operand (number);
			}
			else if (currentSymbol == '(') {
				string substring; 
				int parenthesisEnd = Parser.FindClosingParenthesis (input, i, out substring);
				operand = new Operand (Parser.StringToDouble (Interpreter.ProcessExpression (substring)));
				endPosition = parenthesisEnd;
			} else if (Parser.CharToDigit (currentSymbol) >= 0) {
				operand = new Operand ();
				endPosition = operand.Search (input, i);
			} else
				throw new Exception ("Invalid expression: no operand found");
			//replace with unary minus?!
			if (input [startPosition] == '-')
				operand.Value = - operand.Value;
			return endPosition;
		}
	}
}

