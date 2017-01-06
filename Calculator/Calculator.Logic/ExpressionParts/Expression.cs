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
		abstract public Token Clone ();
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

		public Expression (MultiNode <Token> newRoot) {
			Root = newRoot;
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
			/*string outlet = null;
			Func <string, int, bool> endCondition = (inputString, position) => {
				return false;
			};*/
			//How to avoid repeating of code below? Can we call one constructor from another?
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
				if (activeNode == Root && Root.Element.Priority >= newPart.Priority) {
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

		public Expression (string input, Func <string, int, bool> endCondition, out string outlet) {
			ParsedStream stream = new ParsedStream (input, endCondition);
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
				if (activeNode == Root && Root.Element.Priority >= newPart.Priority) {
					this.InsertBefore (newPart, 0);
				} else {
					this.InsertAfter (newPart, 0, 1);
				}
				if (newPart.branchCount == 2) {
					newPart = stream.ReadOperand ();
					this.AddNext (newPart, 1);
				} 
			}
			outlet = stream.GetRest();
		}

		public void Draw () {
		}

		public double Calculate () {
			if (Root.DescendantCount == 0)
				return ((Operand)Root.Element).Value;
			MyStack<Operand> operandStack = new MyStack<Operand> ();
			for (int i = 0; i < Root.DescendantCount; i++) {
				Expression subTree = new Expression (Root.Descendants [i]);
				operandStack.Push (new Number(subTree.Calculate ()));
			}
			((Operator)Root.Element).Perform (operandStack);
			return (operandStack.Pop()).Value;
		}
	}
}

