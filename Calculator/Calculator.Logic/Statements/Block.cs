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

		public override string Execute () {
			throw new Exception ("Not implemented");
			/*double result = 0;
			int contentPosition = 0;

			Func<string> getExpression = () => {
				if (contentPosition < Content.Length) {
					int start = contentPosition;
					contentPosition = Content.IndexOf ('\n', contentPosition);
					if (contentPosition < 0)
						contentPosition = Content.Length;
					return Content.Substring (start, contentPosition - start);
				} else return "q";
			};

			Func<string, bool> outputAction = (output) => {
				if (output != "q") {
					result = Parser.StringToDouble (output);
					return false;
				} else {
					return true;
				}
			};

			Interpreter.Run (getExpression, outputAction, false);

			return result;*/
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

