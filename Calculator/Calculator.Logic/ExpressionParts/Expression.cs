using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class Expression
	{
		public Token Root;

		public Expression (Token newRoot) {
			if (newRoot is Subtree)
				Root = ((Subtree)newRoot).tree.Root;			
			else 
				Root = newRoot;
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
			ExpressionBuilder result = new ExpressionBuilder (Root.Clone ());
			for (int i = 0; i < Root.branchCount; i++) {
				Expression branch = (new Expression (Root.Arguments [i])).Clone ();
				result.AddNext (branch, i);
			}
			return result.ToExpression ();
		}

		public double Calculate () {
			return Root.Evaluate ();
		}
	}
}

