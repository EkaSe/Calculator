using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLibrary
{
	public class MyDictionary <K, V> : IEnumerable <KeyValuePair <K, V>>
	{
		MyHashTable <K,V> HashTable = new MyHashTable <K,V> ();

		public V this [K key] {
			get { return HashTable [key]; }
			set { HashTable [key] = value; }
		}

		public bool Add (K newKey, V newValue) {
			if (HashTable.Contains (newKey))
				return false;
			else {
				HashTable [newKey] = newValue;
				return true;
			}
		}

		public void Add (KeyValuePair <K, V> pair) {
			Add (pair.Key, pair.Value);
		}

		public void Remove (K targetKey) {
			HashTable.Remove (targetKey);
		}

		public bool Contains (K targetKey) {
			return HashTable.Contains (targetKey);
		}

		public MyDictionary <K, V> Clone () {
			MyDictionary <K, V> clone = new MyDictionary<K, V> ();
			clone.HashTable = this.HashTable.Clone ();
			return clone;
		}

		public IEnumerator<KeyValuePair <K, V>> GetEnumerator() {
			return (IEnumerator<KeyValuePair <K, V>>) new MyDictionaryEnumerator <K, V> (this);
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return GetEnumerator ();
		}

		public class MyDictionaryEnumerator<K, V> : IEnumerator<KeyValuePair <K, V>> {
			MyHashTable <K, V> hashTable;

			Node <KeyValuePair <K, V>> currentNode;
			int length => hashTable.Length;
			int position;
			int count;

			public MyDictionaryEnumerator (MyDictionary <K, V> parent) {
				hashTable = parent.HashTable;
				Reset ();
			}

			public KeyValuePair <K, V> Current => currentNode.Element; 

			public bool HasNext => (count < length); 

			public void Next() {
				if (currentNode == null || currentNode.Next == null) {
					if (currentNode != null)
						position++;
					while (position < 0 || hashTable.Table [position] == null) {
						position++;
					}
					currentNode = hashTable.Table [position].FirstNode;
				} else
					currentNode = currentNode.Next;
				count++;
			}

			public void Reset() {
				position = -1;
				count = 0;
				currentNode = null;
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