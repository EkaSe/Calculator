using System;
using System.Collections.Generic;

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
			factorial,
		};
		
		static int Priority (OperatorCode currentOperator){
			int result = 0;
			switch ((int) expression [i]) {
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
						expression.RemoveRange (i - 1, i + 1);
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
				switch (currentSymbol) {
				case "0":
				case "1":
				case "2":
				case "3":
				case "4":
				case "5":
				case "6":
				case "7":
				case "8":
				case "9":
					currentDigit = Convert.ToDouble(currentSymbol);
					break;
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
						if (expression [j] == ")")
							parenthesisCount--;
						if (expression [j] == "(")
							parenthesisCount++;
						if (parenthesisCount < 0)
							return "Invalid string: parenthesis";
					} while ((parenthesisCount != 0);
					result = ProcessExpression (expression.GetRange ( i + 1, j - i - 1);
					expression.Add (Convert.ToDouble (result));
					break;	
				default:
					result = "Invalid symbol: " + currentSymbol;
					return result;
				}
				if (mantissaLength == 1)
					currentNumber = currentNumber * 10 + currentDigit;
				else {
					currentNumber = currentNumber + currentDigit * mantissaLength;
					mantissaLength *= 0.1;
				}
			}
			expression.Add (currentNumber);
			result = Convert.ToString (Calculate (expression));
			return result;
		}
	}
}