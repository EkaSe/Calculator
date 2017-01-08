using System;

namespace Calculator.Logic
{
	public class Subtree: Operand
	{
		public Expression tree;

		public Subtree (string input): base () {
			tree = new Expression (input);
			branchCount = tree.Root.DescendantCount;
		}

		override public double Value {
			get { return tree.Calculate(); }
			set { }
		}

		Subtree (Expression newTree) {
			tree = newTree;
		}

		override public Token Clone () {
			return new Subtree (tree.Clone ());
		}
	}
}

