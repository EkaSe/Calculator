using System;
using System.Collections.Generic;

namespace MyLibrary
{
	public class Node {
		public Node Previous;
		public Node Next;
		public object Element;

		public Node (object input, Node previousNode, Node nextNode) {
			Element = input;
			Previous = previousNode;
			Next = nextNode;
		}

		public Node (object input) {
			Element = input;
			Previous = null;
			Next = null;
		}

		public Node () {
			Previous = null;
			Next = null;
		}
	}

	public class MyLinkedList
	{
		public Node FirstNode;
		public Node LastNode;

		private int length;
		public int Length {
			get { return length; }
			private set { length = value; }
		}

		public MyLinkedList ()
		{
			Length = 0;
			FirstNode = new Node ();
			LastNode = new Node ();
		}

		public MyLinkedList (object[] inputList)
		{
			Length = 0;
			FirstNode = new Node ();
			LastNode = new Node ();
			for (int i = 0; i < inputList.Length; i++) {
				Add (inputList [i]);
			}
		}

		public void Add (object addedElement) {
			Length++;
			Node currentElement = new Node (addedElement);
			if (Length == 1) {
				FirstNode = currentElement;
				LastNode = currentElement;
			} else {
				currentElement.Previous = LastNode;
				LastNode.Next = currentElement;
				LastNode = currentElement;
			}
		}

		public void InsertAfter (object insertion, Node preceedingNode) {
			Node followingNode = preceedingNode.Next;
			Node insertedElement = new Node (insertion, preceedingNode, followingNode);
			preceedingNode.Next = insertedElement;
			if (followingNode != null)
				followingNode.Previous = insertedElement;
			else
				LastNode = insertedElement;
			Length++;
		}

		public void Insert (object insertion, int position) {
			if (position > length) 
				throw new Exception ("Insertion outside linked list bounds");
			Node preceedingElement = FirstNode;
			for (int i = 1; i < position; i++) {
				preceedingElement = preceedingElement.Next;
			}
			InsertAfter (insertion, preceedingElement);
		}

		public void Insert (object[] insertion, int startPosition) {
			if (startPosition > Length) 
				throw new Exception ("Insertion outside linked list bounds");
			if (insertion.Length != 0) {
				Node preceedingElement = FirstNode;
				for (int i = 1; i < startPosition; i++) {
					preceedingElement = preceedingElement.Next;
				}
				Node followingElement = preceedingElement.Next;
				Node insertedElement = new Node (insertion [0], preceedingElement, followingElement);
				preceedingElement.Next = insertedElement;
				Node nextInsertedElement;
				for (int i = 0; i < insertion.Length - 1; i++) {
					nextInsertedElement = new Node (insertion [i + 1], insertedElement, followingElement);
					insertedElement.Next = nextInsertedElement;
					insertedElement = nextInsertedElement;
				}
				if (startPosition != length)
					followingElement.Previous = insertedElement;
				else
					LastNode = insertedElement;
				length += insertion.Length;
			}
		}

		public void Remove (Node element) {
			if (element.Next != null)
				element.Next.Previous = element.Previous;
			else {
				LastNode = element.Previous;
				LastNode.Next = null;
			}
			if (element.Previous != null)
				element.Previous.Next = element.Next;
			else {
				FirstNode = element.Next;
				FirstNode.Previous = null;
			}
			Length--;
		}

		public void RemoveBefore (Node followingNode) {
			Remove (followingNode.Previous);
		}

		public object[] ToArray (){
			object[] array = new object[length];
			return array;
		}

		public Node Navigate (Node currentNode, int offset) {
			if (offset > 0) {
				for (int i = 0; i < offset; i++) {
					if (currentNode == LastNode)
						i = offset;
					else
						currentNode = currentNode.Next;
				}
			} else {
				for (int i = 0; i < offset; i++) {
					if (currentNode == FirstNode)
						i = offset;
					else
						currentNode = currentNode.Previous;
				}
			}
			return currentNode;
		}
	}
}

