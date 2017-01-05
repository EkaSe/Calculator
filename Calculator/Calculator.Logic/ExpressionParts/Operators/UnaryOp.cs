using System;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class UnaryOp: Operator
	{
		public UnaryOp (int priority) : base (priority, 1) { }

		override public void Perform (MyStack<Operand> operandStack) {
			Operand result = PerformOperation (operandStack.Pop ());
			operandStack.Push (result);
		}

		abstract protected Operand PerformOperation (Operand arg);

		override public void PushOperands (MyStack<Operand> stack, ref Node<Operand> currentOperand) {}
	}
}

