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
		private Func<string, int, bool> endCondition;
		private bool isEnd;
		public bool IsEnd {
			get { 
				if (currentPosition >= expression.Length || endCondition (expression, currentPosition))
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
			endCondition = (inputString, position) => {
				return false;
			};
		}
		
		public ParsedStream (string input, Func <string, int, bool> endCondition) {
			expression = Parser.SkipSpaces (input);
			currentPosition = 0;
			if (expression.Length > 0) 
				isEnd = false;
			else isEnd = true;
			this.endCondition = endCondition;
		}

		public Operand ReadOperand () {
			Operand operand;
			currentPosition = OperandSearch.Run (expression, currentPosition, out operand);
			if (currentPosition == -1 || currentPosition > expression.Length)
				isEnd = true;
			return operand;
		}

		public void ReadOperator (out Operator firstOperator) {
			currentPosition = OperatorSearch.Run (expression, currentPosition, out firstOperator);
			if (currentPosition == -1)
				isEnd = true;
			currentPosition++;
		}

		public Operator ReadOperator () {
			Operator result = null;
			ReadOperator (out result);
			return result;
		}
		
		public string GetRest () {
			return expression.Substring (currentPosition);
		}
	}
}