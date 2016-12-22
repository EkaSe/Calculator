using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class ClassSearch <T> {
		MyList<T> List = new MyList<T> ();
		Func<T, string, int, int> Search;

		public ClassSearch (Func<T, string, int, int> SearchFunc) {
			Search = SearchFunc;
		}

		public void Register (T newEntity) {
			List.Add (newEntity);
		}

		public int Run (string input, int startPosition, out T nextFound) {
			int position = -1;
			nextFound = default (T);
			for (int i = 0; i < List.Length; i++) {
				T current = List [i];
				int currentPosition = Search (current, input, startPosition);
				if (currentPosition > 0 && (currentPosition < position || position < 0)) {
					position = currentPosition;
					nextFound = current;
				}
			}
			return position;
		}
	}
}

