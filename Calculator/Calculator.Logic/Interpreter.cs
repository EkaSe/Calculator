using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class Interpreter
	{

		static double Calculate (MyLinkedList<Operator> operators, MyLinkedList<Operand> operands) {
			double result = operands.FirstNode.Element.Value;
			Node<Operand> currentOperand = operands.FirstNode;
			MyStack <Operator> operatorStack = new MyStack<Operator> ();
			MyStack <Operand> operandStack = new MyStack<Operand> ();
			operandStack.Push (currentOperand.Element);
			currentOperand = currentOperand.Next;
			for (Node<Operator> currentOperator = operators.FirstNode; currentOperator != null; currentOperator = currentOperator.Next) {
				operatorStack.Push (currentOperator.Element);
				currentOperator.Element.PushOperands (operandStack, ref currentOperand);
				if (currentOperator.Next == null || currentOperator.Element.Priority >= currentOperator.Next.Element.Priority) {
					while (operatorStack.Length > 0) {
						Operator performedOperator = operatorStack.Pop ();
						performedOperator.Perform (operandStack);
					}
				}
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
		
		static public string ProcessExpression (string input, Func <string, int, bool> endCondition, out string outlet)
		{
			MyLinkedList<Operator> operators = new MyLinkedList<Operator> ();
			MyLinkedList<Operand> operands = new MyLinkedList<Operand> ();
			Operator currentOperator;
			string result;
			ParsedStream expression = new ParsedStream (input, endCondition);
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
			outlet = expression.GetRest();
			return result;
		}

		static public string ProcessExpression (string input)
		{
			string outlet = null;
			Func <string, int, bool> endCondition = (inputString, position) => {
				return false;
			};
			return ProcessExpression (input, endCondition, out outlet);
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