using System;
using System.Collections.Generic;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class Interpreter
	{

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
				result = 3;
				break;
			case OperatorCode.factorial:
				result = 4;
				break;
			}
			return result;
		}

		static Node<double> PerformOperation (OperatorCode currentOperator, MyLinkedList<double> operands, Node<double> currentOperand) {
			double operand1;
			double operand2;
			operand1 = currentOperand.Element;
			currentOperand = currentOperand.Next;
			operand2 = currentOperand.Element;
			double result = PerformOperation (currentOperator, operand1, operand2);
			operands.InsertAfter (result, currentOperand);
			currentOperand = currentOperand.Next;
			operands.RemoveBefore (currentOperand);
			operands.RemoveBefore (currentOperand);
			return currentOperand;
		}

		static double PerformOperation (OperatorCode currentOperator, double operand1, double operand2) {
			double result = operand1;
			switch (currentOperator) {
			case OperatorCode.plus:
				result = operand1 + operand2;
				break;
			case OperatorCode.minus:
				result = operand1 - operand2;
				break;
			case OperatorCode.multiply:
				result = operand1 * operand2;
				break;
			case OperatorCode.divide:
				result = operand1 / operand2;
				break;
			}
			return result;
		}

		static double Calculate (MyLinkedList<OperatorCode> operators, MyLinkedList<double> operands) {
			double result = operands.FirstNode.Element;
			Node<double> currentOperand = operands.FirstNode;
			MyStack <double> operandStack = new MyStack<double> ();
			MyStack <OperatorCode> operatorStack = new MyStack<OperatorCode> ();
			for (Node<OperatorCode> currentOperator = operators.FirstNode; currentOperator != null; 
				currentOperator = currentOperator.Next) {
				operatorStack.Push (currentOperator.Element);
				operandStack.Push (currentOperand.Element);
				currentOperand = currentOperand.Next;
				if (currentOperator.Next == null || Priority (currentOperator.Element) >= Priority (currentOperator.Next.Element)) {
					while (operatorStack.Length > 0) {
						currentOperand.Element = PerformOperation (operatorStack.Pop(), operandStack.Pop(), currentOperand.Element);
					}
				}
				result = currentOperand.Element;
			}
			return result;
		}

		static public string ProcessStatement (string input) {
			string statement = Parser.SkipSpaces (input);
			string result;
			Variables.CreateLocals (); 
			int assignPosition = statement.IndexOf ('=');
			if (assignPosition > 0) {
				string expression = statement.Substring (assignPosition + 1);
				if (expression.IndexOf ('=') >= 0)
					throw new Exception ("Invalid expression: Assignment under assignment");
				string assignee = statement.Substring (0, assignPosition);
				if (Variables.CheckVariable (assignee) && (expression.IndexOf(assignee) < 0 || Variables.IsVariable (assignee))) {
					string value = ProcessExpression (expression);
					result = assignee + " = " + value;
					Variables.AssignLocal (assignee, Parser.StringToDouble (value));
				} else
					throw new Exception ("Invalid expression: Cannot assign value to " + assignee);
			} else {
				result = ProcessExpression (statement);
			}
			Variables.MergeLocals ();
			return result;
		}

		static public string ProcessExpression (string input)
		{
			MyLinkedList<OperatorCode> operators = new MyLinkedList<OperatorCode> ();
			MyLinkedList<double> operands = new MyLinkedList<double> ();
			double currentOperand = 0;
			OperatorCode currentOperator;
			string result;
			ParsedStream expression = new ParsedStream (input);
			if (expression.IsEnd)
				throw new Exception ("Invalid expression: no operand found");
			currentOperand = expression.ReadOperand (); 
			operands.Add (currentOperand);
			while (!expression.IsEnd) {
				expression.ReadOperator (out currentOperator);
				operators.Add (currentOperator);
				if (currentOperator != OperatorCode.factorial)
					currentOperand = expression.ReadOperand ();
				operands.Add (currentOperand);                
			}
			result = Parser.DoubleToString (Calculate (operators, operands));
			return result;
		}

		static public void Run (Func<string> getExpression, Func<string, bool> outputAction) {
			Variables.ClearDictionaries ();
			bool finish = false;
			while (!finish) {
				string input = getExpression ();
				string output;
				if (Parser.ToLowerCase (input) != "q") {
					try {
						output = ProcessStatement (input);
					} catch (Exception e) {
						output = e.Message;
					}
				} else output = "q";
				finish = outputAction (output);
			}
		}
	}
}