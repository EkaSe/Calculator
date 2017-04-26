using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLibrary
{
	public class MyList<T>: IEnumerable <T>
	{
		public T[] Elements;
		public int Length;
		public int Capacity;

		public MyList ()
		{
			Length = 0;
			Capacity = 4;
			Elements = new T[Capacity];
		}

		public MyList (int length) {
			Length = length;
			Capacity = length;
			Elements = new T[Capacity];
		}

		public T this [int key] {
			get { return Elements [key]; }
			set { 
				Elements [key] = value;

			}
		}

		public MyList (T[] inputList)
		{
			Elements = inputList;
			Length = inputList.Length;
			Capacity = 2 * Length;
		}

		public void Add (T addedElement) {
			if (Length < Capacity) {
				Elements [Length] = addedElement;
				Length++;
			} else {
				ApproveCapacity (Length + 1);
				Elements [Length++] = addedElement;
			}
		}

		public int IndexOf (T sought) {
			int index = -1;
			for (int i = 0; i < Length; i++) {
				if (sought.Equals (Elements [i]))
					index = i;
			}
			return index;
		}

		public void ApproveCapacity (int requiredSize) {
			if (Capacity < requiredSize) {
				T[] temp = Elements;
				while (Capacity < requiredSize) {
					Capacity *= 2;
				}
				Elements = new T[Capacity];
				for (int i = 0; i < temp.Length; i++) {
					Elements [i] = temp [i];
				}
			}
		}

		public IEnumerator<T> GetEnumerator() {
			return (IEnumerator<T>) new MyListEnumerator<T> (this);
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return GetEnumerator ();
		}

		public class MyListEnumerator<T> : IEnumerator<T> {
			MyList <T> collection;
			int position;

			public MyListEnumerator (MyList <T> list) {
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

