using System;
using System.Collections.Generic;
using System.Globalization;

namespace Calculator
{
	public class Calculator
	{
		public enum OperatorCode {
			plus,
			minus,
			multiply,
			divide,
			degree,
			factorial
		};

		static public int CharToDigit (char symbol) {
			int code = (int) symbol;
			int digit;
			if ((code > 47) && (code < 58))
				digit = code - 48;
			else
				digit = -1;
			return digit;
		}

		static protected int Priority (double currentOperator){
			int result = 0;
			switch ((int) currentOperator) {
				case (int) OperatorCode.plus:
					result = 1;
					break;
				case (int) OperatorCode.minus:
					result = 1;
					break;
				case (int) OperatorCode.multiply:
					result = 2;
					break;
				case (int) OperatorCode.divide:
					result = 2;
					break;
				case (int) OperatorCode.degree:
					result = 4;
					break;
				case (int) OperatorCode.factorial:
					result = 5;
					break;
			}
			return result;
		}
		
		static double Calculate (List<double> expression) {
			double result = expression[0];
			for (int priorityCount = 5; priorityCount > 0; priorityCount--) {
				for (int i = 1; i < expression.Count; i += 2) {
					if (Priority (expression [i]) == priorityCount) {
						switch ((int) expression [i]) {
						case (int) OperatorCode.plus:
							result = expression [i - 1] + expression [i + 1];
							break;
						case (int) OperatorCode.minus:
							result = expression [i - 1] - expression [i + 1];
							break;
						case (int) OperatorCode.multiply:
							result = expression [i - 1] * expression [i + 1];
							break;
						case (int) OperatorCode.divide:
							result = expression [i - 1] / expression [i + 1];
							break;
						}
						expression.RemoveRange (i - 1, 3);
						expression.Insert (i - 1, result);
						i -= 2;
					}
				}
			}
			return result;
		}

		static public string ProcessExpression (string input)
		{
			List<double> expression = new List<double> ();
			double currentNumber = 0;
			double currentDigit = 0;
			double mantissaLength = 1;
			string result;
			for (int i = 0; i < input.Length; i++) {
				string currentSymbol = Convert.ToString (input [i]);
				if (CharToDigit (input [i]) >= 0) {
					currentDigit = (double) CharToDigit (input [i]);
					if (mantissaLength == 1)
						currentNumber = currentNumber * 10 + currentDigit;
					else {
						currentNumber = currentNumber + currentDigit * mantissaLength;
						mantissaLength *= 0.1;
					}
				} else {
					switch (currentSymbol) {
					case ".": 
						mantissaLength = 0.1;
						break;
					case " ": 
						break;
					case "+": 
						expression.Add (currentNumber);
						currentNumber = 0;
						currentDigit = 0;
						mantissaLength = 1;
						expression.Add ((double)OperatorCode.plus);
						break;
					case "-": 
						expression.Add (currentNumber);
						currentNumber = 0;
						currentDigit = 0;
						mantissaLength = 1;
						expression.Add ((double)OperatorCode.minus);
						break;
					case "*": 
						expression.Add (currentNumber);
						currentNumber = 0;
						currentDigit = 0;
						mantissaLength = 1;
						expression.Add ((double)OperatorCode.multiply);
						break;
					case "/": 
						expression.Add (currentNumber);
						currentNumber = 0;
						currentDigit = 0;
						mantissaLength = 1;
						expression.Add ((double)OperatorCode.divide);
						break;
					case "(": 
						int parenthesisCount = 1;
						int j = i;
						do {
							j++;
							if (j == input.Length)
								return "Invalid string: parenthesis";
							if (String.Equals (Convert.ToString (input [j]), ")"))
								parenthesisCount--;
							if (String.Equals (Convert.ToString (input [j]), "("))
								parenthesisCount++;
							if (parenthesisCount < 0)
								return "Invalid string: parenthesis";
						} while (parenthesisCount != 0);
						result = ProcessExpression (input.Substring (i + 1, j - i - 1));
						currentNumber = Convert.ToDouble (result);
						i = j;
						break;	
					default:
						result = "Invalid symbol: " + currentSymbol;
						return result;
					}
				}
			}
			expression.Add (currentNumber);
			NumberFormatInfo provider = new NumberFormatInfo();
			provider.NumberDecimalSeparator = ".";
			result = Convert.ToString (Calculate (expression), provider);
			return result;
		}
	}
}