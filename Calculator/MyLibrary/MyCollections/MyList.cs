using System;

namespace MyLibrary
{
	public class MyList
	{
		public object[] Elements;
		public int Length;
		public int Capacity;

		public MyList ()
		{
			Length = 0;
			Capacity = 4;
			Elements = new object[Capacity];
		}

		public MyList (object[] inputList)
		{
			Elements = inputList;
			Length = inputList.Length;
			Capacity = 2 * Length;
		}

		public void Add (object addedElement) {
			if (Length < Capacity) {
				Elements [Length] = addedElement;
				Length++;
			} else {
				object[] temp = Elements;
				Capacity *= 2;
				Elements = new object[Capacity];
				for (int i = 0; i < Length; i++) {
					Elements [i] = temp [i];
				}
				Elements [Length++] = addedElement;
			}
		}

		public int IndexOf (object sought) {
			int index = -1;
			for (int i = 0; i < Length; i++) {
				if (sought.Equals (Elements [i]))
					index = i;
			}
			return index;
		}
	}
}

