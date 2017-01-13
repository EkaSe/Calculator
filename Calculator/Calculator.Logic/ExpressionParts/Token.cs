using System;

namespace Calculator.Logic
{
	abstract public class Token {
		public int Priority;
		public int branchCount;
		public Token Ancestor;
		public Token[] Arguments;
		public int Index;

		public Token (int count){
			Ancestor = null;
			Arguments = null;
			Index = -1;
			branchCount = count;
			if (branchCount > 0)
				Arguments = new Token[branchCount];
		}

		public Token (int count, Token root, int index) {
			Ancestor = root;
			Arguments = null;
			Index = index;
			branchCount = count;
			if (branchCount > 0)
				Arguments = new Token[branchCount];
		}

		abstract public Token Clone ();
		abstract public double Evaluate ();
		//abstract public Value Evaluate ();
	}
}

