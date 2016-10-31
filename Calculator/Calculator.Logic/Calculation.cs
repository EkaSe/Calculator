using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Calculator.Logic
{
	public class Calculation
	{
		/*static NumberFormatInfo formatProvider;

		static void InitProvider () {
			formatProvider = new NumberFormatInfo();
			formatProvider.NumberDecimalSeparator = ".";
		}*/

		static List<string> variableNames = new List<string> ();
		static List<double> variableValues = new List<double> ();

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

		static public string DoubleToString (double number) {
			StringBuilder result = new StringBuilder ();
			bool isNegative = false;
			bool isInt = true;
			if (number < 0) {
				isNegative = true;
				number = - number;
			}

			if (number == 0)
				result.Append ('0');

			int integralPart = (int) number;
			int mantissa = (int) (long)(number * 1E9 - integralPart * 1E9);

			int counter = 20;
			if (mantissa != 0) {
				result.Insert (0, '.');
				isInt = false;
			}

			while ((int) mantissa * counter != 0) {
				int digit = mantissa % 10;
				result.Insert (1, (char) (digit + (int) '0'));
				mantissa = (mantissa- digit) / 10;
				counter--;
			}

			counter = 20;
			while (integralPart * counter != 0) {
				int digit = integralPart % 10;
				result.Insert (0, (char) (digit + (int) '0'));
				integralPart = (integralPart - digit) / 10;
				counter--;
			}

			if (isNegative) 
				result.Insert (0, '-');


			if (!isInt) {
				while (char.Equals (result [result.Length - 1], '0')) {
					result.Remove (result.Length - 1, 1);
				}
			}

			return result.ToString ();
		}

		static public double StringToDouble (string input, out int outputCode) {
			double result = 0;
			outputCode = 1;
			bool isNegative = false;
			if (input.Equals ("")) {
				outputCode = 0;
				return 0;
			}
			char symbol = input [0];
			if (symbol == '-') 
				isNegative = true;
			double mantissaLength = 1;
			for (int i = 0; i < input.Length; i++) {
				if (isNegative && (i == 0))
					i++;
				symbol = input [i];
				int digit = CharToDigit (symbol);
				if (digit >= 0) {
					if (mantissaLength == 1)
						result = result * 10 + (double) digit;
					else {
						result = result + (double) digit * mantissaLength;
						mantissaLength *= 0.1;
					}
				} else {
					if ((symbol == '.') && (mantissaLength == 1))
						mantissaLength = 0.1;
					else if (symbol != ' ') 
						outputCode = -1;
				}
			}
			if (isNegative)
				result = -result;
			return result;
		}

		static public double StringToDouble (string input) {
			int outputCodeUnused = 0;
			double result = StringToDouble (input, out outputCodeUnused);
			return result;
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

		static protected bool isLetter (char symbol) {
			bool result = false;
			int code = (int)symbol;
			if ((code >= (int)'A') && (code <= (int)'Z'))
				result = true;
			if ((code >= (int)'a') && (code <= (int)'z'))
				result = true;
			return result;
		}

		static public int FindVariable (string input, int startPosition, out double value) {
			value = 0;
			int endPosition = -1;
			StringBuilder variable = new StringBuilder ();
			bool variableEnd = false;
			int i = startPosition;
			char currentSymbol = input [i];
			if (!isLetter (currentSymbol) && !Char.Equals(currentSymbol, '_'))
				return -1;
			while (!variableEnd) {
				if (i == input.Length)
					variableEnd = true;
				else {
					currentSymbol = input [i];
					if (isLetter (currentSymbol) || currentSymbol.Equals ('_') || (CharToDigit (currentSymbol) != -1)) {
						variable.Append (currentSymbol);
						endPosition = i;
					} else
						variableEnd = true;
				}
				i++;
			}

			string variableName = variable.ToString ();
			int variableIndex = variableNames.IndexOf (variableName);
			if (variableIndex != -1)
				value = variableValues [variableIndex];
			else {
				Console.WriteLine ("Enter value of variable " + variableName);
				string variableValueString = Console.ReadLine ();
				int inputIsDouble = 0;
				double variableValue = StringToDouble (variableValueString, out inputIsDouble);
				if (inputIsDouble == 1) {
					variableNames.Add (variableName);
					variableValues.Add (variableValue);
					value = variableValue;
				} else {
					Console.WriteLine ("Invalid input");
					endPosition = -2;
				}
			}
			return endPosition;
		}

		static public int FindOperand (string input, int startPosition, out double operand) {
			operand = 0;
			double currentDecimal = -1;
			double mantissaLength = 1;
			bool operandEnd = false;
			int i = startPosition;
			if (startPosition >= input.Length) {
				operandEnd = true;
				return -1;
			}
			if (Char.Equals (input [i], ' '))
				i++;
			int endPosition = -1;
			char currentSymbol = input [i];
			if (isLetter (currentSymbol) || currentSymbol.Equals ('_')) {
				endPosition = FindVariable (input, i, out operand);
				operandEnd = true;
			} 
			if (currentSymbol.Equals ('(')) {
				string substring; 
				int parenthesisEnd = FindClosingParenthesis (input, i, out substring);
				operand = StringToDouble (ProcessExpression (substring));
				endPosition = parenthesisEnd;
				operandEnd = true;
			}
			while (!operandEnd) {
				currentSymbol = input [i];
				int currentDigit = CharToDigit (currentSymbol);
				if (currentDigit >= 0) {
					currentDecimal = (double) currentDigit;
					if (mantissaLength == 1)
						operand = operand * 10 + currentDecimal;
					else {
						operand = operand + currentDecimal * mantissaLength;
						mantissaLength *= 0.1;
					}
				} else {
					if (currentSymbol.Equals ('.'))
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
			if (input [startPosition] == '-')
				operand = -operand;
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
				if (Char.Equals (input [j], ')'))
					parenthesisCount--;
				if (Char.Equals (input [j], '('))
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
			result = DoubleToString (Calculate (expression));
			return result;
		}
	}
}
