using System;
using MyLibrary;

namespace Calculator.Logic
{
	/*
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
*/
	public class Expression
	{
		public Token Root;
		private Token activeNode;

		public Expression (Token newRoot) {
			Root = newRoot;
			activeNode = Root;
		}

		public void InsertBefore (Token insertion, int linkIndex) {
			Token ancestor = activeNode.Ancestor;
			insertion.Ancestor = ancestor;
			if (ancestor == null)
				Root = insertion;
			else
				ancestor.Arguments [activeNode.Index] = insertion;
			insertion.Index = activeNode.Index;
			activeNode.Ancestor = insertion;
			insertion.Arguments [linkIndex] = activeNode;
			activeNode.Index = linkIndex;
			activeNode = insertion;
		}

		public void InsertAfter (Token insertion, int newIndex, int scionIndex) {
			Token scion = activeNode.Arguments [scionIndex];
			insertion.Ancestor = activeNode;
			activeNode.Arguments [scionIndex] = insertion;
			insertion.Index = scionIndex;
			insertion.Arguments [newIndex] = scion;
			scion.Index = newIndex;
			scion.Ancestor = insertion;
			activeNode = insertion;
		}

		public void AddNext (Token insertion, int inputIndex) {
			//replace currentNode.Next [inputIndex] with newNode
			if (insertion.GetType () == typeof(Subtree)) {
				//MultiNode <Token> insertion = ((Subtree) insertion).tree.Root;
				activeNode.Arguments [inputIndex] = insertion;
				insertion.Index = inputIndex;
				insertion.Ancestor = activeNode;
			} else {
				activeNode.Arguments [inputIndex] = insertion;
				insertion.Index = inputIndex;
				insertion.Ancestor = activeNode;
			}
		}

		public void AddNext (Expression insertion, int inputIndex) {
			activeNode.Arguments [inputIndex] = insertion.Root;
			insertion.Root.Index = inputIndex;
			insertion.Root.Ancestor = activeNode;
		}
			
		private Expression (ParsedStream stream) {
			if (stream.IsEnd)
				throw new Exception ("Invalid expression: no operand found");
			Root = stream.ReadOperand ();
			activeNode = Root;
			Token newPart;
			while (!stream.IsEnd) {
				newPart = stream.ReadOperator ();
				while (activeNode != Root && activeNode.Priority >= newPart.Priority) {
					activeNode = activeNode.Ancestor;
				}
				if (activeNode == Root && Root.Priority >= newPart.Priority) {
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

		public Expression (string input): this (new ParsedStream (input)) { }

		public Expression (string input, Func <string, int, bool> endCondition, out string outlet): 
		this (new ParsedStream (input, endCondition)){
			ParsedStream stream = new ParsedStream (input, endCondition);
			Expression temp = new Expression (stream);
			outlet = stream.GetRest();
		}

		public void Draw () {
		}

		public Expression Clone () {
			Expression result = new Expression (Root.Clone ());
			for (int i = 0; i < Root.branchCount; i++) {
				Expression branch = (new Expression (Root.Arguments [i])).Clone ();
				result.AddNext (branch, i);
			}
			return result;
		}

		public double Calculate () {
			if (Root.branchCount == 0)
				return ((Operand)Root).Value;
			else
				return ((Operator)Root).Evaluate ();
		}
	}
}

