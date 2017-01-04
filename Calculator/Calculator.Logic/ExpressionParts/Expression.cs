using System;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class ExpressionPart {
		public int Priority;
		public int branchCount;
		public ExpressionPart (int count){
			branchCount = count;
		}
		virtual public ExpressionPart Clone () {
			return this;
		}
	}

	public class MultiNode <T> {
		public MultiNode<T> Previous;
		public MultiNode<T>[] Next;
		public int NextCount;
		public T Element;
		public int Index;

		public MultiNode (T input) {
			Element = input;
			Previous = null;
			Next = null;
			NextCount = 0;
			Index = -1;
		}

		public MultiNode (T input, int nextCount) {
			Element = input;
			Previous = null;
			Next = new MultiNode<T>[nextCount];
			NextCount = nextCount;
			Index = -1;
		}
	}

	public class Expression
	{
		public MultiNode <ExpressionPart> Root;
		private MultiNode <ExpressionPart> activeNode;

		public Expression (ExpressionPart rootElement) {
			Root = new MultiNode<ExpressionPart> (rootElement, rootElement.branchCount);
			activeNode = Root;
		}

		public void InsertBefore (ExpressionPart insertion, int linkIndex) {
			MultiNode <ExpressionPart> newNode = new MultiNode<ExpressionPart> (insertion, insertion.branchCount);
			MultiNode <ExpressionPart> ancestor = activeNode.Previous;
			newNode.Previous = ancestor;
			if (ancestor == null)
				Root = newNode;
			else
				ancestor.Next [activeNode.Index] = newNode;
			newNode.Index = activeNode.Index;
			activeNode.Previous = newNode;
			newNode.Next [linkIndex] = activeNode;
			activeNode.Index = linkIndex;
			activeNode = newNode;
		}

		public void InsertAfter (ExpressionPart insertion, int newIndex, int scionIndex) {
			MultiNode <ExpressionPart> newNode = new MultiNode<ExpressionPart> (insertion, insertion.branchCount);
			MultiNode <ExpressionPart> scion = activeNode.Next [scionIndex];
			newNode.Previous = activeNode;
			activeNode.Next [scionIndex] = newNode;
			newNode.Index = scionIndex;
			newNode.Next [newIndex] = scion;
			scion.Index = newIndex;
			scion.Previous = newNode;
			activeNode = newNode;
		}

		public void AddNext (ExpressionPart insertion, int inputIndex) {
			//replace currentNode.Next [inputIndex] with newNode
			MultiNode <ExpressionPart> newNode = new MultiNode<ExpressionPart> (insertion, insertion.branchCount);
			activeNode.Next [inputIndex] = newNode;
			newNode.Index = inputIndex;
			newNode.Previous = activeNode;
		}

		public Expression (string input) {
			ParsedStream expression = new ParsedStream (input);
			if (expression.IsEnd)
				throw new Exception ("Invalid expression: no operand found");
			Root = new MultiNode<ExpressionPart> (expression.ReadOperand (), 0);
			activeNode = Root;
			ExpressionPart newPart;
			while (!expression.IsEnd) {
				newPart = expression.ReadOperator ();
				while (activeNode != Root && activeNode.Element.Priority >= newPart.Priority) {
					activeNode = activeNode.Previous;
				}
				if (activeNode == Root && Root.Element.Priority > newPart.Priority) {
					this.InsertBefore (newPart, 0);
				} else {
					this.InsertAfter (newPart, 0, 1);
				}
				if (newPart.branchCount == 2) {
					newPart = expression.ReadOperand ();
					this.AddNext (newPart, 1);
				} 
			}
		}
	}
}

