using System;

namespace MyLibrary
{
	public static class MyEnumerableExtension
	{
		/// <summary>
		/// returns new MyList collection, containing only those elements, 
		/// for which predicate gives True
		/// </summary>
		public static IMyEnumerable<T> Where<T> (this IMyEnumerable<T> collection, Func<T, bool> predicate) {
			Type collectionType = collection.GetType ();
			var result = Activator.CreateInstance (collectionType);
			var enumerator = collection.Enumerator;
			while (enumerator.HasNext) {
				enumerator.Next ();
				if (predicate (enumerator.Current))
					((IMyEnumerable<T>) result).Add (enumerator.Current);
			}
			return (IMyEnumerable<T>) result;

			/*var enumerator = collection.Enumerator;
			MyList <T> result = new MyList<T> ();
			while (enumerator.HasNext) {
				enumerator.Next ();
				if (predicate (enumerator.Current))
					result.Add (enumerator.Current);
			}
			return (IMyEnumerable <T>) result;*/
		}

		public static IMyEnumerable <char> Where (this string line, Func <char, bool> predicate) {
			//There MUST be some less horrible way to operate with string
			MyList <char> result = new MyList<char> ();
			for (int i = 0; i < line.Length; i++) {
				if (predicate (line [i]))
					result.Add (line [i]);
			}
			return (IMyEnumerable<char>) result;
		}

		/// <summary>
		/// returns new MyList collection, containing all new elements of type T2, 
		/// created by applying selector for each element in the source collection
		/// </summary>
		public static IMyEnumerable<T2> Select<T1, T2> (this IMyEnumerable<T1> collection, Func<T1, T2> selector) {
			MyList <T2> result = new MyList<T2> ();
			var enumerator = collection.Enumerator;
			while (enumerator.HasNext) {
				enumerator.Next ();
				result.Add (selector (enumerator.Current));
			}
			return (IMyEnumerable <T2>) result;
		}

		/// <summary>
		/// returns new array of elements of type T, copied from the collection
		/// </summary>
		public static T[] ToArray<T> (this IMyEnumerable<T> collection) {
			var enumerator = collection.Enumerator;
			int length = 0;
			while (enumerator.HasNext) {
				enumerator.Next ();
				length++;
			}
			enumerator.Reset ();
			T[] result = new T[length];
			for (int i = 0; i < length; i++) {
				enumerator.Next ();
				result[i] = enumerator.Current;
			}
			return result;
		}

		/// <summary>
		/// returns elements of the collection as MyList
		/// </summary>
		public static MyList<T> ToList<T> (this IMyEnumerable<T> collection) {
			MyList <T> result = new MyList<T> ();
			var enumerator = collection.Enumerator;
			while (enumerator.HasNext) {
				enumerator.Next ();
				result.Add (enumerator.Current);
			}
			return result;
		}

		/// <summary>
		/// creates from given T item a pair of key and value, getting them by applying 
		/// corresponding selectors to the given item, and then populating adding them to a new MyDictionary instance.
		/// </summary>
		public static MyDictionary <K, V> ToDictionary<T, K, V>(this IMyEnumerable<T> collection, 
			Func<T, K> keySelector, Func<T, V> valueSelector) {

			MyDictionary <K, V> result = new MyDictionary<K, V> ();
			var enumerator = collection.Enumerator;
			while (enumerator.HasNext) {
				enumerator.Next ();
				result.Add (keySelector (enumerator.Current), valueSelector (enumerator.Current));
			}
			return result;
		}

		/// <summary>
		/// returns the very first element of source collection, if it contains any, 
		/// otherwise returns default value of type T
		/// </summary>
		public static T FirstOrDefault <T> (this IMyEnumerable< T> collection) {
			var enumerator = collection.Enumerator;
			if (enumerator.HasNext) {
				enumerator.Next ();
				return enumerator.Current;
			} else
				return default (T);
		}

		/// <summary>
		/// returns the very first element of the source collection, for which predicate returns True, 
		/// if any, otherwise returns default value of type T
		/// </summary>
		public static T FirstOrDefault<T> (this IMyEnumerable <T> collection, Func<T, bool> predicate) {
			var enumerator = collection.Enumerator;
			bool isFound = false;
			while (enumerator.HasNext && !isFound) {
				enumerator.Next ();
				if (predicate (enumerator.Current))
					isFound = true;
			}
			if (isFound)
				return enumerator.Current;
			else 
				return default (T);
		}

		public static TAccumulate Aggregate <T, TAccumulate>( this IMyEnumerable<T> collection,
			TAccumulate startValue, Func<TAccumulate, T, TAccumulate> accumulate) {
			var enumerator = collection.Enumerator;
			var accumulator = startValue;
			while (enumerator.HasNext) {
				enumerator.Next ();
				accumulator = accumulate (accumulator, enumerator.Current);
			}
			return accumulator;
		}

		public static int Count<T>(this IMyEnumerable<T> collection, Func <T, bool> predicate) {
			var enumerator = collection.Enumerator;
			int count = 0;
			while (enumerator.HasNext) {
				enumerator.Next ();
				if (predicate (enumerator.Current))
					count++;
			}
			return count;
		}
	}
}
