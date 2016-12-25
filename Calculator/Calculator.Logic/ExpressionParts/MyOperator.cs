using System;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class MyOperator
	{
		public int Priority;
		protected int operandCount;

		public MyOperator (int priority, int operandCount) {
			this.Priority = priority;
			this.operandCount = operandCount;
		}

		protected MyList<Operand> ExtractOperands (MyLinkedList<Operand> operands, ref Node<Operand> currentOperand) {
			MyList<Operand> result = new MyList<Operand> ();
			for (int i = 0; i < operandCount; i++) {
				result.Add (currentOperand.Element);
				if (i != operandCount - 1)
					currentOperand = currentOperand.Next;
			}
			return result;
		}

		protected void InsertResult (Operand result, MyLinkedList<Operand> operands, ref Node<Operand> currentOperand) {
			if (currentOperand != null) {
				operands.InsertAfter (result, currentOperand);
				currentOperand = currentOperand.Next;
				for (int i = 0; i < operandCount; i++) {
					operands.RemoveBefore (currentOperand);
				}
			}
			else 
				operands.Add (result);
		}

		public void Perform (MyLinkedList<Operand> operands, ref Node<Operand> currentOperand) {
			Operand result = PerformOperation (ExtractOperands (operands, ref currentOperand));
			InsertResult (result, operands, ref currentOperand);
		}

		public void SkipOperands (ref Node<Operand> currentOperand) {
			for (int i = 0; i < operandCount - 1; i++) {
					currentOperand = currentOperand.Next;
			}
		}

		public void UnSkipOperands (ref Node<Operand> currentOperand) {
			for (int i = 0; i < operandCount - 1; i++) {
				currentOperand = currentOperand.Previous;
			}
		}

		public Operand Perform (MyStack<Operand> operandStack) {
			MyLinkedList<Operand> operands = new MyLinkedList<Operand> ();
			for (int i = 0; i < operandCount; i++) {
				operands.Insert (operandStack.Pop (), 0);
			}
			Node<Operand> currentOperand = operands.FirstNode;
			Operand result = PerformOperation (ExtractOperands (operands, ref currentOperand));
			return result;
		}

		abstract protected Operand PerformOperation (MyList<Operand> operands);

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
				if (currentPosition > 0 && (currentPosition < position || position < 0)) {
					position = currentPosition;
					nextOperator = currentOperator;
				}
			}
			return position;
		}
	}

	public class Plus: MyOperator {
		public Plus (): base(1, 2) {}

		protected override Operand PerformOperation (MyList<Operand> operands) {
			Operand operand1 = operands [0];
			Operand operand2 = operands [1];
			return new Operand (operand1.Value + operand2.Value);
		}

		public override int Search (string input, int startPosition) {
			return SearchBySign (input, startPosition, '+');
		}
	}

	public class Minus: MyOperator {
		public Minus (): base(1, 2) {}

		protected override Operand PerformOperation (MyList<Operand> operands) {
			Operand operand1 = operands [0];
			Operand operand2 = operands [1];
			return new Operand (operand1.Value - operand2.Value);
		}

		public override int Search (string input, int startPosition) {
			return SearchBySign (input, startPosition, '-');
		}
	}

	public class Multiply: MyOperator {
		public Multiply (): base(2, 2) {}

		protected override Operand PerformOperation (MyList<Operand> operands) {
			Operand operand1 = operands [0];
			Operand operand2 = operands [1];
			return new Operand (operand1.Value * operand2.Value);
		}

		public override int Search (string input, int startPosition) {
			return SearchBySign (input, startPosition, '*');
		}
	}

	public class Divide: MyOperator {
		public Divide (): base(2, 2) {}

		protected override Operand PerformOperation (MyList<Operand> operands) {
			Operand operand1 = operands [0];
			Operand operand2 = operands [1];
			return new Operand (operand1.Value / operand2.Value);
		}

		public override int Search (string input, int startPosition) {
			return SearchBySign (input, startPosition, '/');
		}
	}
}

