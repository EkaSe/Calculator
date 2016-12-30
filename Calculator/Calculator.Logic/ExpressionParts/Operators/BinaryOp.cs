using System;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class BinaryOp: MyOperator
	{
		public BinaryOp (int priority) : base (priority, 2) { }

		override public Operand Perform (MyStack<Operand> operandStack) {
			Operand arg2 = operandStack.Pop ();
			Operand arg1 = operandStack.Pop ();
			Operand result = PerformOperation (arg1, arg2);
			return result;
		}

		abstract protected Operand PerformOperation (Operand arg1, Operand arg2);

		override public void PushOperands (MyStack<Operand> stack, ref Node<Operand> currentOperand) {
			stack.Push (currentOperand.Element);
			currentOperand = currentOperand.Next;
		}
	}
}

