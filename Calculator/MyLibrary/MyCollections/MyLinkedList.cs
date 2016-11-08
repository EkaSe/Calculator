using System;
using System.Collections.Generic;

namespace MyLibrary
{
	unsafe class LinkedListElement {
		public LinkedListElement PreviousElement;
		public LinkedListElement NextElement;
		public object Element;

		public LinkedListElement (object input, LinkedListElement previous, LinkedListElement next) {
			Element = input;
			PreviousElement = previous;
			NextElement = next;
		}

		public LinkedListElement (object input) {
			Element = input;
			PreviousElement = null;
			NextElement = null;
		}

		public LinkedListElement () {
			PreviousElement = null;
			NextElement = null;
		}
	}

	public class MyLinkedList
	{
		private LinkedListElement FirstElement;
		private LinkedListElement LastElement;
		public int Length;

		public MyLinkedList ()
		{
			Length = 0;
			FirstElement = new LinkedListElement ();
			LastElement = new LinkedListElement ();
		}

		public MyLinkedList (object[] inputList)
		{
			Length = 0;
			FirstElement = new LinkedListElement ();
			LastElement = new LinkedListElement ();
			for (int i = 0; i < inputList.Length; i++) {
				Add (inputList [i]);
			}
		}

		public void Add (object addedElement) {
			Length++;
			if (Length == 1) {
				FirstElement.Element = addedElement;
				LastElement.Element = addedElement;
			}
			LinkedListElement currentElement = new LinkedListElement (addedElement);
			currentElement.NextElement = LastElement;
			LastElement.NextElement = currentElement;
			LastElement = currentElement;
		}

		public void Insert (object insertion, int position) {
			if (position > Length) 
				throw new Exception ("Insertion outside linked list bounds");
			LinkedListElement preceedingElement = FirstElement;
			for (int i = 1; i < position; i++) {
				preceedingElement = preceedingElement.NextElement;
			}
			LinkedListElement followingElement = preceedingElement.NextElement;
			LinkedListElement insertedElement = new LinkedListElement (insertion, preceedingElement, followingElement);
			preceedingElement.NextElement = insertedElement;
			if (position != Length)
				followingElement.PreviousElement = insertedElement;
			else
				LastElement = insertedElement;
			Length++;
		}

		public void Insert (object[] insertion, int startPosition) {
			for (int i = 0; i < insertion.Length; i++) {
				Insert (insertion [i], i + startPosition);
			}
		}


	}
}

