using System;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class UnaryOp: MyOperator
	{
		public UnaryOp (int priority) : base (priority, 1) { }

		override public Operand Perform (MyStack<Operand> operandStack) {
			Operand result = PerformOperation (operandStack.Pop ());
			return result;
		}

		abstract protected Operand PerformOperation (Operand arg);

		override public void PushOperands (MyStack<Operand> stack, ref Node<Operand> currentOperand) {}
	}
}

