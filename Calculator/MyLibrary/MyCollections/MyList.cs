using System;

namespace MyLibrary
{
	public class MyList<T>
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

		public MyList<T> Where(Func<T, bool> predicate) {
		//returns new MyList collection, containing only those elements, for which predicate gives True
			MyList <T> result = new MyList<T> ();
			for (int i = 0; i < Length; i++) {
				if (predicate (Elements [i]))
					result.Add (Elements [i]);
			}
			return result;
		}

		public MyList<T2> Select <T2> (Func<T, T2> selector) {
			//returns new MyList collection, containing all new elements of type T2, 
			//created by applying selector for each element in the source collection
			MyList <T2> result = new MyList<T2> ();
			for (int i = 0; i < Length; i++) {
				result.Add (selector (this [i]));
			}
			return result;
		}

		public T[] ToArray() {
			//returns new array of elements of type T, copied from the collection
			T[] result = new T[Length];
			for (int i = 0; i < Length; i++)
				result [i] = Elements [i];
			return result;
		}

		public T FirstOrDefault() {
			//returns the very first element of source collection, if it contains any, 
			//otherwise returns default value of type T
			if (Length > 0)
				return Elements [0];
			else
				return default (T);
		}

		public T FirstOrDefault(Func<T, bool> predicate) {
			//returns the very first element of the source collection, for which predicate returns True, 
			//if any, otherwise returns default value of type T
			return Where (predicate).FirstOrDefault ();
		}
	}
}

