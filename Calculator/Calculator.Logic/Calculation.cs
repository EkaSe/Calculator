using System;
using System.Collections.Generic;
using System.Text;
using MyLibrary;

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

		static protected int Priority (object currentOperator){
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

		static double Calculate (MyLinkedList expression) {
			double result = Parser.ObjectToDouble (expression.FirstNode.Element);
			for (int priorityCount = 5; priorityCount > 0; priorityCount--) {
				Node current = expression.FirstNode;
				while (current.Next != null) {
					double operand1 = Parser.ObjectToDouble (current.Element);
					current = current.Next;
					object currentOperator = current.Element;
					current = current.Next;
					double operand2 = Parser.ObjectToDouble (current.Element);
					if (Priority (currentOperator) == priorityCount) {
						switch ((int) currentOperator) {
						case (int) OperatorCode.plus:
							result = operand1 + operand2;
							break;
						case (int) OperatorCode.minus:
							result = operand1 - operand2;
							break;
						case (int) OperatorCode.multiply:
							result = operand1 * operand2;
							break;
						case (int) OperatorCode.divide:
							result = operand1 / operand2;
							break;
						}
						expression.InsertAfter (result, current);
						current = current.Next;
						expression.RemoveBefore (current);
						expression.RemoveBefore (current);
						expression.RemoveBefore (current);
					}
				}
			}
			return result;
		}

		static public string ProcessExpression (string input, Func<string, double> getValueByAlias)
		{
			MyLinkedList expression = new MyLinkedList ();
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
				expression.Add (currentOperator);
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
