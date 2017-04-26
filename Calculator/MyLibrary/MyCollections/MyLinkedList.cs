using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLibrary
{
	public class Node <T> {
		public Node<T> Previous;
		public Node<T> Next;
		public T Element;

		public Node (T input = default (T), Node<T> previousNode = null, Node<T> nextNode = null) {
			Element = input;
			Previous = previousNode;
			Next = nextNode;
		}

		public Node (T input) {
			Element = input;
			Previous = null;
			Next = null;
		}

		public Node () {
			Previous = null;
			Next = null;
		}
	}

	public class MyLinkedList <T> : IEnumerable <T>
	{
		public Node<T> FirstNode;
		public Node<T> LastNode;

		private int length;
		public int Length {
			get { return length; }
			private set { length = value; }
		}

		public MyLinkedList ()
		{
			Length = 0;
			FirstNode = null;
			LastNode = null;
		}

		public MyLinkedList (T[] inputList)
		{
			Length = 0;
			for (int i = 0; i < inputList.Length; i++) {
				Add (inputList [i]);
			}
		}

		public void Add (T addedElement) {
			Length++;
			Node<T> currentElement = new Node<T> (addedElement);
			if (Length == 1) {
				FirstNode = currentElement;
				LastNode = currentElement;
			} else {
				currentElement.Previous = LastNode;
				LastNode.Next = currentElement;
				LastNode = currentElement;
			}
			FirstNode.Previous = null;
			LastNode.Next = null;
		}

		public void InsertAfter (T insertion, Node<T> preceedingNode) {
			Node<T> followingNode = preceedingNode.Next;
			Node<T> insertedElement = new Node<T> (insertion, preceedingNode, followingNode);
			preceedingNode.Next = insertedElement;
			if (followingNode != null)
				followingNode.Previous = insertedElement;
			else
				LastNode = insertedElement;
			Length++;
		}

		public void Insert (T insertion, int position) {
			if (position > length) 
				throw new Exception ("Insertion outside linked list bounds");
			Node<T> preceedingElement = FirstNode;
			for (int i = 1; i < position; i++) {
				preceedingElement = preceedingElement.Next;
			}
			InsertAfter (insertion, preceedingElement);
		}

		public void Insert (T[] insertion, int startPosition) {
			if (startPosition > Length) 
				throw new Exception ("Insertion outside linked list bounds");
			if (insertion.Length != 0) {
				Node<T> preceedingElement = FirstNode;
				for (int i = 1; i < startPosition; i++) {
					preceedingElement = preceedingElement.Next;
				}
				Node<T> followingElement = preceedingElement.Next;
				Node<T> insertedElement = new Node<T> (insertion [0], preceedingElement, followingElement);
				preceedingElement.Next = insertedElement;
				Node<T> nextInsertedElement;
				for (int i = 0; i < insertion.Length - 1; i++) {
					nextInsertedElement = new Node<T> (insertion [i + 1], insertedElement, followingElement);
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

		public void Remove (Node<T> element) {
			if (element.Previous != null) {
				element.Previous.Next = element.Next;
				if (element.Next != null) {
					element.Next.Previous = element.Previous;
				} else {
					LastNode = element.Previous;
					LastNode.Next = null;
				}
			} else {
				if (element.Next != null) {
					FirstNode = element.Next;
					FirstNode.Previous = null;

				} else {
					FirstNode = null;
					LastNode = null;
				}
			}
			Length--;
		}

		public void RemoveBefore (Node<T> followingNode) {
			Remove (followingNode.Previous);
		}

		public void RemoveAfter (Node<T> preceedingNode) {
			Remove (preceedingNode.Next);
		}

		public T[] ToArray (){
			T[] array = new T[length];
			return array;
		}

		public Node<T> Navigate (Node<T> currentNode, int offset) {
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

		public IEnumerator<T> GetEnumerator() {
			return (IEnumerator <T>) new MyLinkedListEnumerator<T> (this);
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return GetEnumerator ();
		}

		public class MyLinkedListEnumerator<T> : IEnumerator<T> {
			MyLinkedList <T> collection;
			Node <T> currentNode;

			public MyLinkedListEnumerator (MyLinkedList <T> list) {
				collection = list;
				currentNode = new Node<T> (nextNode: list.FirstNode);
			}

			public T Current {
				get { return currentNode.Element; }
			}

			public bool HasNext {
				get { return collection.Length > 0 && currentNode.Next != null; }
			}

			public void Next() {
				currentNode = currentNode.Next;
			}

			public void Reset() {
				currentNode = new Node<T> (nextNode: collection.FirstNode);
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