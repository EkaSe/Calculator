using System;
using System.Collections.Generic;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class ParsedStream
	{
		private string expression;
		private int currentPosition;
		private bool isEnd;
		public bool IsEnd {
			get { 
				if (currentPosition >= expression.Length)
					isEnd = true;
				return isEnd; 
			}
			private set { isEnd = value; }
		}

		public ParsedStream (string input) {
			expression = Parser.SkipSpaces (input);
			currentPosition = 0;
			if (expression.Length > 0) 
				isEnd = false;
			else isEnd = true;
		}

		public double ReadOperand () {
			double operand;
			currentPosition = Parser.FindOperand (expression, currentPosition, out operand);
			if (currentPosition == -1)
				isEnd = true;
			currentPosition++;
			return operand;
		}

		public void ReadOperator (out Interpreter.OperatorCode firstOperator) {
			currentPosition = Parser.FindOperator (expression, currentPosition, out firstOperator);
			if (currentPosition == -1)
				isEnd = true;
			currentPosition++;
		}
	}
}