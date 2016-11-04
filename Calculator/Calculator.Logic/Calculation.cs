using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Logic
{
	public class Calculation
	{
		public enum OperatorCode {
			plus,
			minus,
			multiply,
			divide,
			degree,
			factorial,
			unknown
		};

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

		static public string ProcessExpression (string input, Func<string, double> getValueByAlias)
		{
			List<double> expression = new List<double> ();
			double currentOperand = 0;
			OperatorCode currentOperator;
			string result;
			int currentPosition = 0;
			currentPosition = Parser.FindOperand (input, currentPosition, out currentOperand, getValueByAlias);
			if (currentPosition == -1)
				return "Invalid expression: no operand found";
			else 
				expression.Add (currentOperand);
			currentPosition++;
			while (currentPosition < input.Length && currentPosition > 0) {
				currentPosition = Parser.FindOperator (input, currentPosition, out currentOperator);
				expression.Add ((double) currentOperator);
				currentPosition++;
				if (currentOperator != OperatorCode.factorial)
					currentPosition = Parser.FindOperand (input, currentPosition, out currentOperand, getValueByAlias);
				expression.Add (currentOperand);
				currentPosition++;
			}
			result = Parser.DoubleToString (Calculate (expression));
			return result;
		}
	}
}
