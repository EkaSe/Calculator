using System;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class MyOperator: ExpressionPart
	{
		protected int operandCount; 
		public int OperandCount {
			get { return operandCount; }
			private set { operandCount = value; }
		}

		public MyOperator (int priority, int operandCount): base (operandCount) {
			this.Priority = priority;
			this.operandCount = operandCount;
		}

		abstract public void PushOperands (MyStack<Operand> stack, ref Node<Operand> currentOperand);

		abstract public Operand Perform (MyStack<Operand> operandStack);

		abstract public int Search (string input, int startPosition);

		protected int SearchBySign (string input, int startPosition, char sign) {
			int operatorPosition = -1;
			bool operatorFound = false;
			int i = startPosition;
			while (!operatorFound && i < input.Length) {
				char currentSymbol = input [i];
				if (currentSymbol == sign) {
					operatorPosition = i;
					operatorFound = true;
				}
				i++;
			}
			return operatorPosition;
		}
	}

	public class OperatorSearch {
		static MyList<MyOperator> OperatorList = new MyList<MyOperator> ();

		static public void RegisterOperator (MyOperator newOperator) {
			OperatorList.Add (newOperator);
		}

		static public int Run (string input, int startPosition, out MyOperator nextOperator) {
			int position = -1;
			nextOperator = null;
			for (int i = 0; i < OperatorList.Length; i++) {
				MyOperator currentOperator = OperatorList [i];
				int currentPosition = currentOperator.Search (input, startPosition);
				if (currentPosition >= 0 && (currentPosition < position || position < 0)) {
					position = currentPosition;
					nextOperator = currentOperator;
				}
			}
			return position;
		}
	}
}

