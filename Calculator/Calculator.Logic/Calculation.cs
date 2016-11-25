﻿using System;
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

		static Node<double> PerformOperation (OperatorCode currentOperator, MyLinkedList<double> operands, Node<double> currentOperand) {
			double operand1;
			double operand2;
			double result = currentOperand.Element;
			switch (currentOperator) {
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
			return currentOperand;
		}

		static double Calculate (MyLinkedList<OperatorCode> operators, MyLinkedList<double> operands) {
			double result = operands.FirstNode.Element;
			Node<double> currentOperand = operands.FirstNode;
			for (Node<OperatorCode> currentOperator = operators.FirstNode; currentOperator != null; 
				currentOperator = currentOperator.Next) {
				Node<OperatorCode> mostPriorityOperator = currentOperator;
				while (mostPriorityOperator.Next != null && Priority (mostPriorityOperator.Element) < Priority (mostPriorityOperator.Next.Element)) {
					mostPriorityOperator = mostPriorityOperator.Next; 
					currentOperand = currentOperand.Next;
				}
				while (mostPriorityOperator != currentOperator) {
					currentOperand = PerformOperation (mostPriorityOperator.Element, operands, currentOperand);
					currentOperand = currentOperand.Previous;
					mostPriorityOperator = mostPriorityOperator.Previous;
					operators.RemoveAfter (mostPriorityOperator);
				}
				currentOperand = PerformOperation (currentOperator.Element, operands, currentOperand);
				result = currentOperand.Element;
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
			ParsedStream expression = new ParsedStream (input);
			if (expression.IsEnd)
				return "Invalid expression: no operand found";
			expression.ReadOperand (out currentOperand, getValueByAlias); 
			operands.Add (currentOperand);
			while (!expression.IsEnd) {
				expression.ReadOperator (out currentOperator);
				operators.Add (currentOperator);
				if (currentOperator != OperatorCode.factorial)
					expression.ReadOperand (out currentOperand, getValueByAlias);
				operands.Add (currentOperand);				
			}
			result = Parser.DoubleToString (Calculate (operators, operands));
			return result;
		}
	}
}