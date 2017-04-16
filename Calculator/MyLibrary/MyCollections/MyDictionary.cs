using System;

namespace MyLibrary
{
	public class MyDictionary <K, V> : IMyEnumerable <KeyValuePair <K, V>>
	{
		MyHashTable <K,V> HashTable = new MyHashTable <K,V> ();
		//private int length = 0;

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

		public IMyEnumerator<KeyValuePair <K, V>> Enumerator => 
		(IMyEnumerator<KeyValuePair <K, V>>) new MyDictionaryEnumerator <K, V> (this);

		public class MyDictionaryEnumerator<K, V> : IMyEnumerator<KeyValuePair <K, V>> {
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
		}
	}
}