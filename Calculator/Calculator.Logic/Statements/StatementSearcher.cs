using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class ParsingResult {
		public Statement result;
		public bool isMatch = true;
		public bool isComplete = false;

		public ParsingResult (Statement result) {
			this.result = result;
			isMatch = true;
			isComplete = true;
		}

		public ParsingResult (Statement result, bool isMatch, bool isComplete) {
			this.result = result;
			this.isMatch = isMatch;
			this.isComplete = isComplete;
		}
	}

	abstract public class StatementParser {
		abstract public ParsingResult Run (string input);
	}

	public class StatementSearcher
	{
		public StatementSearcher ()
		{
		}

		static MyList<StatementParser> StatementList = new MyList<StatementParser> ();

		static public void Register (StatementParser newStatement) {
			StatementList.Add (newStatement);
		}

		static public Statement Run (string input) {
			Statement result = null;
			bool isFound = false;
			ParsedStream stream = new ParsedStream (input);
			string inputStart = "";
			ParsingResult[] results = new ParsingResult [StatementList.Length];
			while (!stream.IsEnd && !isFound) {
				inputStart = inputStart + stream.GetEntity ();
				for (int i = 0; i < StatementList.Length; i++) {
					StatementParser current = StatementList [i];
					if (results [i] == null || results [i].isMatch)
						results [i] = current.Run (inputStart);
					if (results [i].isComplete) {
						if (!isFound) {
							isFound = true;
							result = results [i].result;
						} else
							throw new Exception ("Statement type could not be determined");
					}
						
					/*int currentPosition = current.Search (input, startPosition);
				if (currentPosition >= 0 && (currentPosition < position || position < 0)) {
					position = currentPosition;
					result = (Statement) current.Clone();
				}*/
				}
			}
			if (!isFound)
				result = new ExpressionBuilder (input).ToExpression ();
			return result;
		}
	}
}

