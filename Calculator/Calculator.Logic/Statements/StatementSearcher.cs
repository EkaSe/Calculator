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
			int statementType = -1;
			while (!stream.IsEnd && !isFound) {
				string nextEntity = stream.GetEntity ();
				if (nextEntity == " ")
					nextEntity = nextEntity + stream.GetEntity ();
				inputStart = inputStart + nextEntity;
				for (int i = 0; i < StatementList.Length; i++) {
					StatementParser current = StatementList [i];
					if (results [i] == null || results [i].isMatch)
						results [i] = current.Run (inputStart);
					if (results [i].isComplete) {
						if (!isFound) {
							isFound = true;
							result = results [i].result;
							statementType = i;
						} else {
							isFound = false;
							i = StatementList.Length;
						}
					}
				}
			}
			if (isFound) {
				ParsingResult temp = results [statementType];
				while (!stream.IsEnd && temp.isMatch) {
					string nextEntity = stream.GetEntity ();
					if (nextEntity == " ")
						nextEntity = nextEntity + stream.GetEntity ();
					inputStart = inputStart + nextEntity;
					temp = StatementList [statementType].Run (inputStart);
					if (temp.isComplete)
						result = temp.result;
				}
			} else try {
				result = new ExpressionBuilder (input).ToExpression ();
			} catch {
				throw new Exception ("Statement type could not be determined");
			}
			return result;
		}
	}
}

