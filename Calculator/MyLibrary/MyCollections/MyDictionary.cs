﻿using System;

namespace MyLibrary
{
	public class KeyValuePair <K, V> {
		public K Key;
		public V Value;

		public KeyValuePair (K newKey, V newValue) {
			Key = newKey;
			Value = newValue;
		}

		public KeyValuePair <K, V> Clone () {
			return new KeyValuePair<K, V> (Key, Value);
		}
	}

	public class MyDictionary <K, V> : IMyEnumerable <KeyValuePair <K, V>>
	{
		MyList <MyLinkedList <KeyValuePair <K, V>>> HashTable = new MyList<MyLinkedList<KeyValuePair<K, V>>> ();
		private int length = 0;

		public V this [K key] {
			get { return Get (key);    }
			set {
				if (Contains (key)) {
					Node<KeyValuePair <K, V>> target = SearchNode (key);
					target.Element.Value = value;
				} else {
					Add (key, value);
				}
			}
		}

		private int CharLeftCircularShift (int value, int bitCount) {
			// cycle size as a parameter?
			int cycleSize = 8;
			int N = bitCount % cycleSize;
			int result = value;
			if (N != 0)
				result = (value << N) + (value >> (cycleSize - N));
			return result;
		}

		private int HashCode (K key) {
			string stringKey = key.ToString ();
			int result = 0;
			for (int i = 0; i < stringKey.Length; i++) {
				result += CharLeftCircularShift ( (int) stringKey [i], i);
			}
			return result;
		}

		public bool Add (K newKey, V newValue) {
			KeyValuePair <K, V> pair = new KeyValuePair<K, V> (newKey, newValue);
			if (SearchNode (newKey) != null)
				return false;
			int hash = HashCode (pair.Key);
			HashTable.ApproveCapacity (hash);
			MyLinkedList <KeyValuePair <K, V>> cell = HashTable.Elements [hash];
			if (cell == null)
				cell = new MyLinkedList<KeyValuePair<K, V>> ();
			cell.Add (pair);
			HashTable.Elements [hash] = cell;
			length++;
			return true;
		}

		public void Remove (K targetKey) {
			Node<KeyValuePair <K, V>> target = SearchNode (targetKey);
			MyLinkedList <KeyValuePair <K, V>> cell = HashTable.Elements [HashCode (targetKey)];
			cell.Remove (target);
			length--;
		}

		public Node<KeyValuePair <K, V>> SearchNode (K targetKey) {
			int hash = HashCode (targetKey);
			KeyValuePair <K, V> result = new KeyValuePair <K, V> (targetKey, default (V));
			HashTable.ApproveCapacity (hash);
			MyLinkedList <KeyValuePair <K, V>> tableCell = HashTable.Elements [hash];
			if (tableCell == null)
				return null;
			for (Node<KeyValuePair <K, V>> currentPair = tableCell.FirstNode; currentPair != null; currentPair = currentPair.Next) {
				if (targetKey.Equals (currentPair.Element.Key)) {
					result = currentPair.Element;
					return new Node<KeyValuePair<K, V>> (result);
				}
			}
			return null;
		}

		public V Get (K targetKey) {
			Node<KeyValuePair <K, V>> target = SearchNode (targetKey);
			return target.Element.Value;
		}

		public bool Contains (K targetKey) {
			if (SearchNode (targetKey) != null)
				return true;
			else 
				return false;
		}

		public MyDictionary <K, V> Clone () {
			MyDictionary <K, V> clone = new MyDictionary<K, V> ();
			for (int i = 0; i < HashTable.Capacity; i++) {
				if (HashTable [i] != null)
					for (Node <KeyValuePair <K, V>> node = HashTable [i].FirstNode; node != null; node = node.Next) {
						clone.Add (node.Element.Key, node.Element.Value);
					}
			}
			return clone;
		}

		public IMyEnumerator<KeyValuePair <K, V>> Enumerator => 
		(IMyEnumerator<KeyValuePair <K, V>>) new MyDictionaryEnumerator <K, V> (this);

		public class MyDictionaryEnumerator<K, V> : IMyEnumerator<KeyValuePair <K, V>> {
			MyList <MyLinkedList <KeyValuePair <K, V>>> hashTable;
			Node <KeyValuePair <K, V>> currentNode;
			int length;
			int position;
			int count;

			public MyDictionaryEnumerator (MyDictionary <K, V> parent) {
				hashTable = parent.HashTable;
				this.length = parent.length;
				Reset ();
			}

			public KeyValuePair <K, V> Current {
				get { return currentNode.Element; }
			}

			public bool HasNext {
				get { return (count < length); }
			}

			public void Next() {
				if (currentNode == null || currentNode.Next == null) {
					if (currentNode != null)
						position++;
					while (position < 0 || hashTable.Elements [position] == null) {
						position++;
					}
					currentNode = hashTable.Elements [position].FirstNode;
				} else
					currentNode = currentNode.Next;
				count++;
			}

			public void Reset() {
				position = -1;
				count = 0;
				currentNode = null;
			}
		}
	}
}