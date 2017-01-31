using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class Expression
	{
		public Token Root;
		private Token activeNode;

		public Expression (Token newRoot) {
			if (newRoot is Subtree)
				Root = ((Subtree)newRoot).tree.Root;			
			else 
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

		public void InsertAfter (Token insertion, int newIndex, int childIndex) {
			Token child = activeNode.Arguments [childIndex];
			insertion.Ancestor = activeNode;
			activeNode.Arguments [childIndex] = insertion;
			insertion.Index = childIndex;
			insertion.Arguments [newIndex] = child;
			child.Index = newIndex;
			child.Ancestor = insertion;
			activeNode = insertion;
		}

		public void AddNext (Token insertion, int inputIndex) {
			//replace currentNode.Next [inputIndex] with newNode
			Token newNode;
			if (insertion is Subtree)
				newNode = ((Subtree)insertion).tree.Root;
			else
				newNode = insertion;
			activeNode.Arguments [inputIndex] = newNode;
			newNode.Index = inputIndex;
			newNode.Ancestor = activeNode;
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

		public string Draw () {
			if (Root.branchCount > 0) {
				string arguments = "";
				for (int i = 0; i < Root.branchCount; i++) {
					Expression branch = (new Expression (Root.Arguments [i])).Clone ();
					string line;
					if (Root.branchCount == 1 || (i > 0 && i < Root.branchCount - 1))
						line = "|";
					else if (i == 0)
						line = "/";
					else
						line = "\\";
					string branchDrawing = Parser.CenterString (line, Parser.TextPlateSize (branch.Draw ()) [0]) 
						+ "\n" + branch.Draw ();
					arguments = Parser.ConcatPlates (arguments, branchDrawing);
				}
				int argWidth = (Parser.TextPlateSize (arguments)) [0];
				StringBuilder result = new StringBuilder ();
				result.AppendLine (Parser.CenterString (Root.Draw (), argWidth));
				result.AppendLine (arguments);
				return result.ToString();
			} else
				return Root.Draw ();
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
			return Root.Evaluate ();
		}
	}
}

