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

		public Token ReadOperand () {
			Token operand;
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

		public string Get (int count) {
			string result = expression.Substring (currentPosition, count);
			currentPosition += count;
			return result;
		}

		public string Get () {
			return Get (1);
		}

		public string GetEntity () {
			//alias - variable or key word
			//number?
			//!alias => Get();
			//{}, ()
			//line end?
			//sign!
			string result;
			char start = expression [currentPosition];
			if (start == '(' || start == '{')
				currentPosition = Parser.FindClosing (expression, currentPosition + 1, out result, start);
			else if ((int)start >= (int)'0' && (int)start <= (int)'9') {
				double number;
				currentPosition = Parser.FindNumber (expression, currentPosition, out number);
				result = Parser.DoubleToString (number);
			} else if (Parser.IsIdentifierChar (start, true))
				currentPosition = Parser.FindName (expression, currentPosition, out result);
			else
				result = Get ();
			return result;
		}

		public string GetStatement () {
			bool isSupressed = false;
			return GetStatement (out isSupressed);
		}

		public string GetStatement (out bool isSupressed) {
			isSupressed = false;
			StringBuilder result = new StringBuilder ();
			bool statementEnd = IsEnd;
			while (!statementEnd) {
				switch (expression [currentPosition]) {
				case '{':
					string block;
					currentPosition = Parser.FindClosing (expression, currentPosition + 1, 
						out block, expression [currentPosition]);
					result.Append (block);
					break;
				case ';':
					currentPosition++;
					statementEnd = true;
					isSupressed = true;
					break;
				case '\n':
					currentPosition++;
					statementEnd = true;
					break;
				default:
					result.Append (Get ());
					break;
				}
				statementEnd = IsEnd || statementEnd;
			}
			return result.ToString ();
		}


	}
}