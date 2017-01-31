using System;

namespace Calculator.Logic
{
	public class Subtree: Operand
	{
		public Expression tree;

		public Subtree (string input): base () {
			tree = new Expression (input);
			//branchCount = tree.Root.branchCount;
		}

		override public double Value {
			get { return tree.Calculate(); }
			set { }
		}

		Subtree (Expression newTree) : base () {
			tree = newTree;
		}

		override public Token Clone () {
			return new Subtree (tree.Clone ());
		}

		override public string Draw () {
			return tree.Draw ();
		}
	}
}

