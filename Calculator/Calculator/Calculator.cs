using System;
using System.Collections.Generic;
using System.Globalization;

namespace Calculator
{
	public class Calculator
	{
		static NumberFormatInfo formatProvider;

		static void InitProvider () {
			formatProvider = new NumberFormatInfo();
			formatProvider.NumberDecimalSeparator = ".";
		}

		public enum OperatorCode {
			plus,
			minus,
			multiply,
			divide,
			degree,
			factorial,
			unknown
		};

		static public int CharToDigit (char symbol) {
			int code = (int) symbol;
			int digit;
			if ((code >= (int) '0') && (code <= (int) '9'))
				digit = code - (int) '0';
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

		static public int FindOperand (string input, int startPosition, out double firstOperand) {
			firstOperand = 0;
			double currentDecimal = -1;
			double mantissaLength = 1;
			bool operandEnd = false;
			int i = startPosition;
			int endPosition = -1;
			while (!operandEnd) {
				char currentSymbol = input [i];
				int currentDigit = CharToDigit (currentSymbol);
				if (currentDigit >= 0) {
					currentDecimal = (double) currentDigit;
					if (mantissaLength == 1)
						firstOperand = firstOperand * 10 + currentDecimal;
					else {
						firstOperand = firstOperand + currentDecimal * mantissaLength;
						mantissaLength *= 0.1;
					}
				} else {
					switch (currentSymbol) {
					case '.':
						mantissaLength = 0.1;
						break;
					case '(':
						string substring; 
						int parenthesisEnd = FindClosingParenthesis (input, i, out substring);
						firstOperand = Convert.ToDouble (ProcessExpression (substring), formatProvider);
						endPosition = parenthesisEnd;
						operandEnd = true;
						break;
					default:
						if (currentDecimal >= 0) {
							operandEnd = true;
							endPosition = i - 1;
						}
						break;
					}
				}
				i++;
				if (i == input.Length) {
					operandEnd = true;
					if (currentDecimal >= 0)
						endPosition = input.Length - 1;
				}
			}
			if (input [startPosition] == '-')
				firstOperand = -firstOperand;
			return endPosition;
		}

		static public int FindOperator (string input, int startPosition, out OperatorCode firstOperator) {
			int operatorPosition = -1;
			bool operatorFound = false;
			int i = startPosition;
			firstOperator = OperatorCode.unknown;
			while ((!operatorFound) && (i < input.Length)) {
				char currentSymbol = input [i];
				switch (currentSymbol) {
				case '+': 
					firstOperator = (int)OperatorCode.plus;
					operatorPosition = i;
					operatorFound = true;
					break;
				case '-': 
					firstOperator = OperatorCode.minus;
					operatorPosition = i;
					operatorFound = true;
					break;
				case '*': 
					firstOperator = OperatorCode.multiply;
					operatorPosition = i;
					operatorFound = true;
					break;
				case '/': 
					firstOperator = OperatorCode.divide;
					operatorPosition = i;
					operatorFound = true;
					break;
				case '!': 
					firstOperator = OperatorCode.factorial;
					operatorPosition = i;
					operatorFound = true;
					break;
				case '^': 
					firstOperator = OperatorCode.degree;
					operatorPosition = i;
					operatorFound = true;
					break;
				}
				i++;
			}
			return operatorPosition;
		}

		static int FindClosingParenthesis (string input, int startPosition, out string substring) {
			int parenthesisCount = 1;
			int j = startPosition;
			substring = "";
			do {
				j++;
				if (j == input.Length)
					return -1;
				if (String.Equals (Convert.ToString (input [j]), ")"))
					parenthesisCount--;
				if (String.Equals (Convert.ToString (input [j]), "("))
					parenthesisCount++;
				if (parenthesisCount < 0)
					return -1;
			} while (parenthesisCount != 0);
			substring = input.Substring (startPosition + 1, j - startPosition - 1);
			return j;
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
			InitProvider ();
			List<double> expression = new List<double> ();
			double currentOperand = 0;
			OperatorCode currentOperator;
			string result;
			int currentPosition = 0;
			currentPosition = FindOperand (input, currentPosition, out currentOperand);
			if (currentPosition == -1)
				return "Invalid expression: no operand found";
			else 
				expression.Add (currentOperand);
			currentPosition++;
			while ((currentPosition < input.Length) && (currentPosition > 0)) {
				currentPosition = FindOperator (input, currentPosition, out currentOperator);
				expression.Add ((double) currentOperator);
				currentPosition++;
				if (currentOperator != OperatorCode.factorial)
					currentPosition = FindOperand (input, currentPosition, out currentOperand);
				expression.Add (currentOperand);
				currentPosition++;
			}
			result = Convert.ToString (Calculate (expression), formatProvider);
			return result;
		}
	}
}