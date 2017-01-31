using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class BuiltInFunc: Token {

		public string name;

		public BuiltInFunc () : base(0) {
			Priority = 100;
		}

		public BuiltInFunc (string newName, string arguments) : base(0) {
			name = newName;
			SetArguments (arguments);
			Priority = 100;
		}

		public BuiltInFunc (string arguments) : base(0) {
			SetArguments (arguments);
		}

		public void SetArguments (string arguments) {
			MyList <Token> operands = new MyList<Token> (); 
			branchCount = 0;
			string restOfArgs = arguments;
			Func <string, int, bool> endCondition = (input, currentPosition) => {
				if (input [currentPosition] == ',')
					return true;
				else return false;
			};
			while (restOfArgs != "") {
				if (restOfArgs [0] == ',')
					restOfArgs = restOfArgs.Substring (1);
				Expression tree = new Expression (restOfArgs, endCondition, out restOfArgs);
				operands.Add (tree.Root); 
				branchCount++;
			}
			Arguments = new Token[branchCount];
			for (int i = 0; i < branchCount; i++) {
				Arguments [i] = operands [i];
				Arguments [i].Ancestor = this;
				Arguments [i].Index = i;
			}
		}
	}

	static public class BIFSearch {
		static MyDictionary <string, BuiltInFunc> BIFList = new MyDictionary<string, BuiltInFunc> ();

		static public void RegisterBIF (BuiltInFunc newBIF) {
			BIFList.Add (newBIF.name, newBIF);
		}

		static public int Run (string input, int startPosition, out Token operand) {
			operand = null;
			string alias = null;
			int endPosition = Parser.FindName (input, startPosition, out alias);
			alias = Parser.ToLowerCase (alias);
			if (!BIFList.Contains (alias))
				return -1;
			else if (endPosition != input.Length - 1 && input [endPosition + 1] == '(') {
				string arguments; 
				endPosition = Parser.FindClosingParenthesis (input, endPosition + 1, out arguments);
				BuiltInFunc BIF = (BuiltInFunc) BIFList [alias].Clone();
				BIF.SetArguments (arguments);
				operand = BIF;
				return endPosition;
			} else
				return -1;
		}
	}
}