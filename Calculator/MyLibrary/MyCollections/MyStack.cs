using System;

namespace MyLibrary
{
	public class MyStack <T> : IMyEnumerable <T>
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

		public T Pop () {
			if (Length == 0) 
				throw new Exception ("Attempt to pop empty stack");
			Length--;
			T result = top;
			if (Length != 0)
				top = list.Elements [Length - 1];
			return result;
		}

		IMyEnumerator<T> IMyEnumerable<T>.Enumerator {
			get { return (IMyEnumerator<T>) Enumerator; }
		}

		private MyListEnumerator<T> Enumerator {
			get { return new MyListEnumerator<T> (list); }
		}
	}
}

