using System;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class Token {
		public int Priority;
		public int branchCount;
		public Token (int count){
			branchCount = count;
		}
		virtual public Token Clone () {
			return this;
		}
	}

	public class MultiNode <T> {
		public MultiNode<T> Ancestor;
		public MultiNode<T>[] Descendants;
		public int DescendantCount;
		public T Element;
		public int Index;

		public MultiNode (T input) {
			Element = input;
			Ancestor = null;
			Descendants = null;
			DescendantCount = 0;
			Index = -1;
		}

		public MultiNode (T input, int nextCount) {
			Element = input;
			Ancestor = null;
			Descendants = new MultiNode<T>[nextCount];
			DescendantCount = nextCount;
			Index = -1;
		}
	}

	public class Expression
	{
		public MultiNode <Token> Root;
		private MultiNode <Token> activeNode;

		public Expression (Token rootElement) {
			Root = new MultiNode<Token> (rootElement, rootElement.branchCount);
			activeNode = Root;
		}

		public void InsertBefore (Token insertion, int linkIndex) {
			MultiNode <Token> newNode = new MultiNode<Token> (insertion, insertion.branchCount);
			MultiNode <Token> ancestor = activeNode.Ancestor;
			newNode.Ancestor = ancestor;
			if (ancestor == null)
				Root = newNode;
			else
				ancestor.Descendants [activeNode.Index] = newNode;
			newNode.Index = activeNode.Index;
			activeNode.Ancestor = newNode;
			newNode.Descendants [linkIndex] = activeNode;
			activeNode.Index = linkIndex;
			activeNode = newNode;
		}

		public void InsertAfter (Token insertion, int newIndex, int scionIndex) {
			MultiNode <Token> newNode = new MultiNode<Token> (insertion, insertion.branchCount);
			MultiNode <Token> scion = activeNode.Descendants [scionIndex];
			newNode.Ancestor = activeNode;
			activeNode.Descendants [scionIndex] = newNode;
			newNode.Index = scionIndex;
			newNode.Descendants [newIndex] = scion;
			scion.Index = newIndex;
			scion.Ancestor = newNode;
			activeNode = newNode;
		}

		public void AddNext (Token insertion, int inputIndex) {
			//replace currentNode.Next [inputIndex] with newNode
			MultiNode <Token> newNode = new MultiNode<Token> (insertion, insertion.branchCount);
			activeNode.Descendants [inputIndex] = newNode;
			newNode.Index = inputIndex;
			newNode.Ancestor = activeNode;
		}

		public Expression (string input) {
			ParsedStream stream = new ParsedStream (input);
			if (stream.IsEnd)
				throw new Exception ("Invalid expression: no operand found");
			Root = new MultiNode<Token> (stream.ReadOperand (), 0);
			activeNode = Root;
			Token newPart;
			while (!stream.IsEnd) {
				newPart = stream.ReadOperator ();
				while (activeNode != Root && activeNode.Element.Priority >= newPart.Priority) {
					activeNode = activeNode.Ancestor;
				}
				if (activeNode == Root && Root.Element.Priority > newPart.Priority) {
					this.InsertBefore (newPart, 0);
				} else {
					this.InsertAfter (newPart, 0, 1);
				}
				if (newPart.branchCount == 2) {
					newPart = stream.ReadOperand ();
					this.AddNext (newPart, 1);
				} 
			}
		}

		public void Draw () {
		}

		public double Calculate () {
		}

		/*public double Calculate (MyLinkedList<Operator> operators, MyLinkedList<Operand> operands) {
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
		}*/
	}
}

