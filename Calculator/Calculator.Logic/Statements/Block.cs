using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class Block: Statement
	{
		MyList <string> statements = new MyList<string> ();
		public int Count { 
			get { return statements.Length; }
		}

		public Block (string input) {
			SetSeparateLines (input);
		}

		public Block (VarSet globals, string input) {
			locals = new VarSet (globals);
			SetSeparateLines (input);
		}

		private void SetSeparateLines (string input) {
			StringBuilder statement = new StringBuilder ();
			for (int i = 0; i < input.Length; i++) {
				if (input [i] == ';' || input [i] == '\n') {
					if (statement.Length > 0)
						statements.Add (statement.ToString ());
					statement = new StringBuilder ();
				} else 
					statement.Append (input [i]);
			}
			if (statement.Length > 0)
				statements.Add (statement.ToString ());
		}

		protected override string Execute () {
			string output = "";
			for (int i = 0; i < Count; i++) {
				Statement result = StatementSearcher.Run (statements [i]);
				if (i == 0) 
					output = result.Process ();
				else
					output = output + ", " + result.Process ();
			}
			return output;
		}
	}

	public class BlockParser : StatementParser {
		override public ParsingResult Run (string input) {
			Block result = null;
			bool isMatch = false;
			bool isComplete = false;
			string content = "";
			if (input [0] == '{') {
				int position = Parser.FindClosing (input, 0, out content, '{');
				if (position >= 0) {
					result = new Block (content);
					isMatch = true;
					isComplete = true;
				} else
					isMatch = true;
			}
			return new ParsingResult (result, isMatch, isComplete);
		}
	}
}

