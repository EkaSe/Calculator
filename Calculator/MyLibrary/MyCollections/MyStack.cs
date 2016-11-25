using System;

namespace MyLibrary
{
	public class MyStack <T>
	{
		private Node<T> Top;
		public T top {
			get { return Top.Element; }
			set { Top.Element = value; }
		}

		private int count;
		public int Count {
			get { return count; }
			private set { count = value; }
		}

		public MyStack () {
			Count = 0;
			Top = null;
		}

		public void Push (T element) {
			Count++;
			Node<T> newTop = new Node<T> (element);
			if (Count == 1) {
				Top = newTop;
				Top.Previous = null;
			} else {
				Top.Next = newTop;
				newTop.Previous = Top;
				Top = newTop;
			}
			Top.Next = null;
		}

		public T Pop () {
			if (count == 0) 
				throw new Exception ("Attempt to pop empty stack");
			Count--;
			T result = top;
			Top = Top.Previous;
			Top.Next = null;			
			return result;
		}

	}
}

