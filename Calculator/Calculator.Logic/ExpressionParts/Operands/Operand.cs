using System;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class Operand: Token
	{
		protected double value;

		public Operand (): base(0) {
			Priority = 100;
		}

		virtual public double Value {
			get { return this.value; }
			set { this.value = value; }
		}

		override public double Evaluate () {
			return Value;
		}
	}

	public class OperandSearch {
		static public int Run (string input, int startPosition, out Token operand) {
			//returns position of the first symbol AFTER operand or -1 if not found
			operand = null;
			int i = startPosition;
			if (startPosition >= input.Length) 
				return -1;
			int endPosition = -1;
			if (input [startPosition] == '-') {
				operand = new Number (0);
				return startPosition;
			}
			if (input [startPosition] == '+' || input [startPosition] == ' ') 
				i++;
			char currentSymbol = input [i];
			if (Parser.IsLetter (currentSymbol) || currentSymbol == '_') {
				endPosition = BIFSearch.Run (input, i, out operand);
				if (endPosition < 0) {
					endPosition = VarSearch.Run (input, i, out operand);
				} 
			}
			else if (currentSymbol == '(') {
				string substring; 
				int parenthesisEnd = Parser.FindClosingParenthesis (input, i, out substring);
				operand = new Subtree (substring);
				endPosition = parenthesisEnd;
			} else if (Parser.CharToDigit (currentSymbol) >= 0) {
				double number = 0;
				endPosition = Parser.FindNumber (input, i, out number);
				operand = new Number (number);
			} else
				throw new Exception ("Invalid expression: no operand found");
			return endPosition + 1;
		}
	}
}

