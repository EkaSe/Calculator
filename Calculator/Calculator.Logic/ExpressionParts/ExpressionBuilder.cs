using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class ExpressionBuilder
	{
		private Token Root;
		private Token activeNode;

		public ExpressionBuilder (Token newRoot) {
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
			//replaces currentNode.Next [inputIndex] with newNode
			Token newNode;
			if (insertion is Subtree)
				newNode = ((Subtree) insertion).tree.Root;
			else 
				newNode = insertion;
			activeNode.Arguments [inputIndex] = newNode;
			newNode.Index = inputIndex;
			newNode.Ancestor = activeNode;
		}

		public void AddNext (Expression insertion, int inputIndex) {
			Token newNode = insertion.Root;
			activeNode.Arguments [inputIndex] = newNode;
			newNode.Index = inputIndex;
			newNode.Ancestor = activeNode;
		}

		private ExpressionBuilder (ParsedStream stream) {
			if (stream.IsEnd)
				throw new CalculatorException ("Invalid expression: no operand found");
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

		public ExpressionBuilder (string input): this (new ParsedStream (input)) { }

		public ExpressionBuilder (string input, Func <string, int, bool> endCondition, out string outlet): 
		this (new ParsedStream (input, endCondition)){
			ParsedStream stream = new ParsedStream (input, endCondition);
			new ExpressionBuilder (stream).ToExpression ();
			outlet = stream.GetRest();
		}

		public Expression ToExpression () {
			return new Expression (Root);
		}
	}


	public class ExpressionParser : StatementParser {
		override public ParsingResult Run (string input) {
			//try
			Expression result = new ExpressionBuilder (input).ToExpression ();
			return new ParsingResult (result);
		}
	}
}

