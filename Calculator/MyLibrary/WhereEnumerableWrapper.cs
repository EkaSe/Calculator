using System;

namespace MyLibrary
{
	public class WhereEnumerableWrapper <T> : IMyEnumerable <T>
	{
		void Add (T element);

		public IMyEnumerator<T> Enumerator => (IMyEnumerator<T>) new WhereEnumerator<T> (this);

		public class WhereEnumerator<T> : IMyEnumerator<T> {
			MyList <T> collection;
			int position;

			public WhereEnumerator (MyList <T> list) {
				collection = list;
				position = -1;
			}

			public T Current {
				get { return collection [position]; }
			}

			public bool HasNext {
				get { 
					if (collection.Length > 0 && position < collection.Length - 1)
						return true;
					else
						return false;
				}
			}

			public void Next() {
				position++;
			}

			public void Reset() {
				position = -1;
			}
		}
	}
}

