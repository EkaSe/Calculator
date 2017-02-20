using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class StatementSearcher
	{
		public StatementSearcher ()
		{
		}

		static MyList<Statement> StatementList = new MyList<Statement> ();

		static public void RegisterStatement (Statement newStatement) {
			StatementList.Add (newStatement);
		}

		static public int Run (string input, int startPosition, out Statement result) {
			int position = -1;
			result = null;
			for (int i = 0; i < StatementList.Length; i++) {
				Statement current = StatementList [i];
				/*int currentPosition = current.Search (input, startPosition);
				if (currentPosition >= 0 && (currentPosition < position || position < 0)) {
					position = currentPosition;
					result = (Statement) current.Clone();
				}*/
			}
			return position;
		}
	}
}

