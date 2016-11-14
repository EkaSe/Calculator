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

		static private int Priority (OperatorCode currentOperator){
			int result = 0;
			switch (currentOperator) {
			case OperatorCode.plus:
				result = 1;
				break;
			case OperatorCode.minus:
				result = 1;
				break;
			case OperatorCode.multiply:
				result = 2;
				break;
			case OperatorCode.divide:
				result = 2;
				break;
			case OperatorCode.degree:
				result = 4;
				break;
			case OperatorCode.factorial:
				result = 5;
				break;
			}
			return result;
		}

		static double Calculate (MyLinkedList<OperatorCode> operators, MyLinkedList<double> operands) {
			double result = operands.FirstNode.Element;
			for (int priorityCount = 5; priorityCount > 0; priorityCount--) {
				Node<double> currentOperand = operands.FirstNode;
				for(Node<OperatorCode> currentOperator = operators.FirstNode; currentOperator != null; 
					currentOperator = currentOperator.Next) {
					double operand1;
					double operand2;
					if (Priority (currentOperator.Element) == priorityCount) {
						switch (currentOperator.Element) {
						case OperatorCode.plus:
							operand1 = currentOperand.Element;
							currentOperand = currentOperand.Next;
							operand2 = currentOperand.Element;
							result = operand1 + operand2;
							break;
						case OperatorCode.minus:
							operand1 = currentOperand.Element;
							currentOperand = currentOperand.Next;
							operand2 = currentOperand.Element;
							result = operand1 - operand2;
							break;
						case OperatorCode.multiply:
							operand1 = currentOperand.Element;
							currentOperand = currentOperand.Next;
							operand2 = currentOperand.Element;
							result = operand1 * operand2;
							break;
						case OperatorCode.divide:
							operand1 = currentOperand.Element;
							currentOperand = currentOperand.Next;
							operand2 = currentOperand.Element;
							result = operand1 / operand2;
							break;
						}
						operands.InsertAfter (result, currentOperand);
						currentOperand = currentOperand.Next;
						operands.RemoveBefore (currentOperand);
						operands.RemoveBefore (currentOperand);
						operators.Remove (currentOperator);
					} else {
						currentOperand = currentOperand.Next;
					}
				}
			}
			return result;
		}

		static public string ProcessExpression (string input, Func<string, double> getValueByAlias)
		{
			MyLinkedList<OperatorCode> operators = new MyLinkedList<OperatorCode> ();
			MyLinkedList<double> operands = new MyLinkedList<double> ();
			double currentOperand = 0;
			OperatorCode currentOperator;
			string result;
			int currentPosition = 0;
			currentPosition = Parser.FindOperand (input, currentPosition, out currentOperand, getValueByAlias);
			if (currentPosition == -1)
				return "Invalid expression: no operand found";
			else 
				operands.Add (currentOperand);
			currentPosition++;
			while (currentPosition < input.Length && currentPosition > 0) {
				currentPosition = Parser.FindOperator (input, currentPosition, out currentOperator);
				operators.Add (currentOperator);
				currentPosition++;
				if (currentOperator != OperatorCode.factorial)
					currentPosition = Parser.FindOperand (input, currentPosition, out currentOperand, getValueByAlias);
				operands.Add (currentOperand);
				currentPosition++;
			}
			result = Parser.DoubleToString (Calculate (operators, operands));
			return result;
		}
	}
}
