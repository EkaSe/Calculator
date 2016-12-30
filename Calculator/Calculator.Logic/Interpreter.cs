using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class Interpreter
	{

		static double Calculate (MyLinkedList<MyOperator> operators, MyLinkedList<Operand> operands) {
			double result = operands.FirstNode.Element.Value;
			Node<Operand> currentOperand = operands.FirstNode;
			MyStack <MyOperator> operatorStack = new MyStack<MyOperator> ();
			MyStack <Operand> operandStack = new MyStack<Operand> ();
			operandStack.Push (currentOperand.Element);
			currentOperand = currentOperand.Next;
			for (Node<MyOperator> currentOperator = operators.FirstNode; currentOperator != null; currentOperator = currentOperator.Next) {
				operatorStack.Push (currentOperator.Element);
				currentOperator.Element.PushOperands (operandStack, ref currentOperand);
				if (currentOperator.Next == null || currentOperator.Element.Priority >= currentOperator.Next.Element.Priority) {
					while (operatorStack.Length > 0) {
						MyOperator performedOperator = operatorStack.Pop ();
						Operand operationResult = performedOperator.Perform (operandStack);
						operandStack.Push (operationResult);
					}
				}
				/*currentOperator.Element.SkipOperands (ref currentOperand);
				if (currentOperator.Next == null || currentOperator.Element.Priority >= currentOperator.Next.Element.Priority) {
					while (operatorStack.Length > 0) {
						MyOperator performedOperator = operatorStack.Pop ();
						performedOperator.UnSkipOperands (ref currentOperand);
						performedOperator.Perform (operands, ref currentOperand);
					}
				}
				result = currentOperand.Element.Value;*/
			}
			result = operandStack.Pop().Value;
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
			MyLinkedList<MyOperator> operators = new MyLinkedList<MyOperator> ();
			MyLinkedList<Operand> operands = new MyLinkedList<Operand> ();
			MyOperator currentOperator;
			string result;
			ParsedStream expression = new ParsedStream (input);
			if (expression.IsEnd)
				throw new Exception ("Invalid expression: no operand found");
			Operand currentOperand = expression.ReadOperand (); 
			operands.Add (currentOperand);
			while (!expression.IsEnd) {
				expression.ReadOperator (out currentOperator);
				operators.Add (currentOperator);
				if (currentOperator.OperandCount == 2) {
					currentOperand = expression.ReadOperand ();
					operands.Add (currentOperand);
				}
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