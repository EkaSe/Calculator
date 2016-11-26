using System;

namespace MyLibrary
{
	public class KeyValuePair <K, V> {
		public K Key;
		public V Value;

		public KeyValuePair (K newKey, V newValue) {
			Key = newKey;
			Value = newValue;
		}
	}

	public class MyDictionary <K, V>
	{
		MyList <MyLinkedList <KeyValuePair <K, V>>> HashTable = new MyList<MyLinkedList<KeyValuePair<K, V>>> ();

		private int CharLeftCircularShift (int value, int bitCount) {
			// cycle size as a parameter?
			int cycleSize = 8;
			int N = bitCount % cycleSize;
			int result = value;
			if (N != 0)
				result = value << N + value >> (cycleSize - N);
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

		public void Add (KeyValuePair <K, V> pair) {
			int hash = HashCode (pair.Key);
			HashTable.ApproveCapacity (hash);
			MyLinkedList <KeyValuePair <K, V>> cell = HashTable.Elements [hash];
			if (cell == null)
				cell = new MyLinkedList<KeyValuePair<K, V>> ();
			cell.Add (pair);
			HashTable.Elements [hash] = cell;
		}

		public void Remove (K targetKey) {
			Node<KeyValuePair <K, V>> target = SearchNode (targetKey);
			MyLinkedList <KeyValuePair <K, V>> cell = HashTable.Elements [HashCode (targetKey)];
			cell.Remove (target);
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
	}
}