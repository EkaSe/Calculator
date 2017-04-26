using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLibrary
{
	public class MyStack <T> : IEnumerable <T>
	{
		private MyList <T> list;
		private T top;
		public T Top {
			get { return top; }
			private set { top = value; }
		}

		public MyStack () {
			list = new MyList<T> ();
			list.Length = 0;
			top = default (T);
		}

		public int Length {
			get { return list.Length; }
			private set { list.Length = value; }
		}

		public void Push (T element) {
			list.Add (element);
			top = element;
		}

		public void Add (T element) { Push (element); }

		public T Pop () {
			if (Length == 0) 
				throw new Exception ("Attempt to pop empty stack");
			Length--;
			T result = top;
			if (Length != 0)
				top = list.Elements [Length - 1];
			return result;
		}

		public IEnumerator<T> GetEnumerator() {
			return (IEnumerator <T>) new MyStackEnumerator<T> (this);
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return GetEnumerator ();
		}

		public class MyStackEnumerator<T> : IEnumerator<T> {
			MyList <T> collection;
			int position;

			public MyStackEnumerator (MyStack <T> parent) {
				collection = parent.list;
				position = collection.Length;
			}

			public T Current {
				get { return collection [position]; }
			}

			public bool HasNext => collection.Length > 0 && position > 0;

			public void Next() {
				position--;
			}

			public void Reset() {
				position = collection.Length;
			}

			object IEnumerator.Current {
				get { return Current; }
			}

			public void Dispose() {}

			public bool MoveNext() { 
				if (HasNext) {
					Next ();
					return true;
				} else
					return false;
			}
		}
	}
}

