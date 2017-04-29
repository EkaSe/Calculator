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

		public KeyValuePair <K, V> Clone () {
			return new KeyValuePair<K, V> (Key, Value);
		}
	}

	public class MyHashTable <K, V>
	{
		public MyLinkedList <KeyValuePair <K, V>> [] Table;
		public int Length { get; private set; }
		public int Capacity { get; private set; }

		public MyHashTable ()
		{
			Length = 0;
			Capacity = 64;
			Table = new MyLinkedList <KeyValuePair <K, V>>[Capacity];
		}

		public V this [K key] {
			get { 
				Node<KeyValuePair <K, V>> target = SearchNode (key);
				return target.Element.Value; 
			}
			set { 
				KeyValuePair <K, V> pair = new KeyValuePair<K, V> (key, value);
				Node<KeyValuePair <K, V>> editedNode = SearchNode (key);
				if (editedNode != null)
					editedNode.Element.Value = value;
				else {
					int hash = HashCode (pair.Key);
					ApproveCapacity (hash);
					if (Table [hash] == null)
						Table [hash] = new MyLinkedList<KeyValuePair<K, V>> ();
					Table [hash].Add (pair);
					Length++;
				}

			}
		}

		public void ApproveCapacity (int requiredSize) {
			if (Capacity < requiredSize) {
				MyLinkedList <KeyValuePair <K, V>> [] temp = Table;
				while (Capacity < requiredSize) {
					Capacity *= 2;
				}
				Table = new MyLinkedList <KeyValuePair <K, V>> [Capacity];
				for (int i = 0; i < temp.Length; i++) {
					Table [i] = temp [i];
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

		public void Remove (K targetKey) {
			Node<KeyValuePair <K, V>> target = SearchNode (targetKey);
			Table [HashCode (targetKey)].Remove (target);
			Length--;
		}

		private Node<KeyValuePair <K, V>> SearchNode (K targetKey) {
			int hash = HashCode (targetKey);
			KeyValuePair <K, V> result = new KeyValuePair <K, V> (targetKey, default (V));
			ApproveCapacity (hash);
			if (Table [hash] == null)
				return null;
			for (Node<KeyValuePair <K, V>> currentPair = Table [hash].FirstNode; currentPair != null; currentPair = currentPair.Next) {
				if (targetKey.Equals (currentPair.Element.Key)) {
					result = currentPair.Element;
					return new Node<KeyValuePair<K, V>> (result);
				}
			}
			return null;
		}

		public bool Contains (K targetKey) {
			if (SearchNode (targetKey) != null)
				return true;
			else 
				return false;
		}

		public MyHashTable <K, V> Clone () {
			MyHashTable <K, V> clone = new MyHashTable <K, V> ();
			for (int i = 0; i < Capacity; i++) {
				if (Table [i] != null)
					for (Node <KeyValuePair <K, V>> node = Table [i].FirstNode; node != null; node = node.Next) {
						clone [node.Element.Key] = node.Element.Value;
					}
			}
			return clone;
		}
	}
}

