using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLibrary
{
	public class MyList<T>: IMyEnumerable <T>
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
			set { Elements [key] = value; }
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

		/// <summary>
		/// returns new MyList collection, containing only those elements, 
		/// for which predicate gives True
		/// </summary>
		public MyList<T> Where(Func<T, bool> predicate) {
			MyList <T> result = new MyList<T> (Length);
			for (int i = 0; i < Length; i++) {
				if (predicate (Elements [i]))
					result.Add (Elements [i]);
			}
			return result;
		}

		/// <summary>
		/// returns new MyList collection, containing all new elements of type T2, 
		/// created by applying selector for each element in the source collection
		/// </summary>
		public MyList<T2> Select <T2> (Func<T, T2> selector) {
			MyList <T2> result = new MyList<T2> ();
			for (int i = 0; i < Length; i++) {
				result.Add (selector (this [i]));
			}
			return result;
		}

		/// <summary>
		/// returns new array of elements of type T, copied from the collection
		/// </summary>
		public T[] ToArray() {
			T[] result = new T[Length];
			for (int i = 0; i < Length; i++)
				result [i] = Elements [i];
			return result;
		}

		/// <summary>
		/// returns the very first element of source collection, if it contains any, 
		/// otherwise returns default value of type T
		/// </summary>
		public T FirstOrDefault() {
			if (Length > 0)
				return Elements [0];
			else
				return default (T);
		}

		/// <summary>
		/// returns the very first element of the source collection, for which predicate returns True, 
		/// if any, otherwise returns default value of type T
		/// </summary>
		public T FirstOrDefault(Func<T, bool> predicate) {
			return Where (predicate).FirstOrDefault ();
		}

		public IMyEnumerator<T> Enumerator => (IMyEnumerator<T>) new MyListEnumerator<T> (this);

		public IEnumerator<T> GetEnumerator() {
			return (IEnumerator <T>) Enumerator;
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return GetEnumerator ();
		}

		public class MyListEnumerator<T> : IMyEnumerator<T> {
			MyList <T> collection;
			int position;

			public MyListEnumerator (MyList <T> list) {
				collection = list;
				position = -1;
			}

			public T Current {
				get { return collection [position]; }
			}

			object IEnumerator.Current {
				get { return Current; }
			}

			public bool HasNext {
				get { 
					if (collection.Length > 0 && position < collection.Length)
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

