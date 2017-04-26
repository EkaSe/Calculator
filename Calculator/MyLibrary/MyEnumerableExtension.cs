using System;
using System.Collections.Generic;

namespace MyLibrary
{
	public static class MyEnumerableExtension
	{
		public static IEnumerable<T> Where<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
		{
			var enumerator = collection.GetEnumerator ();
			while (enumerator.MoveNext ())
			{
				if (predicate(enumerator.Current))
					yield return enumerator.Current;
			}
		}

		public static IEnumerable <char> AsEnumerable (this string line) {
			MyList <char> result = new MyList<char> ();
			for (int i = 0; i < line.Length; i++) {
				result.Add (line [i]);
			}
			return (IEnumerable<char>) result;
		}

		/// <summary>
		/// returns new MyList collection, containing all new elements of type T2, 
		/// created by applying selector for each element in the source collection
		/// </summary>
		public static IEnumerable<T2> Select<T1, T2> (this IEnumerable<T1> collection, Func<T1, T2> selector) {
			var enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				yield return selector(enumerator.Current);
			}
		}



		/// <summary>
		/// returns new array of elements of type T, copied from the collection
		/// </summary>
		public static T[] ToArray<T> (this IEnumerable<T> collection) {
			var list = collection.ToList ();
			T[] result = new T[list.Length];
			for (int i = 0; i < list.Length; i++) {
				result [i] = list [i];
			}
			return result;
		}

		/// <summary>
		/// returns elements of the collection as MyList
		/// </summary>
		public static MyList<T> ToList<T> (this IEnumerable<T> collection) {
			MyList<T> result = new MyList<T>();
			var enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext()) {
				result.Add (enumerator.Current);
			}
			return result;
		}

		/// <summary>
		/// creates from given T item a pair of key and value, getting them by applying 
		/// corresponding selectors to the given item, and then populating adding them to a new MyDictionary instance.
		/// </summary>
		public static MyDictionary <K, V> ToDictionary<T, K, V>(this IEnumerable<T> collection, 
			Func<T, K> keySelector, Func<T, V> valueSelector) {

			MyDictionary <K, V> result = new MyDictionary<K, V> ();
			var enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				result.Add (keySelector (enumerator.Current), valueSelector (enumerator.Current));
			}
			return result;
		}

		/// <summary>
		/// returns the very first element of source collection, if it contains any, 
		/// otherwise returns default value of type T
		/// </summary>
		public static T FirstOrDefault <T> (this IEnumerable< T> collection) {
			var enumerator = collection.GetEnumerator();
			if (enumerator.MoveNext()) {
				return enumerator.Current;
			} else
				return default (T);
		}

		/// <summary>
		/// returns the very first element of the source collection, for which predicate returns True, 
		/// if any, otherwise returns default value of type T
		/// </summary>
		public static T FirstOrDefault<T> (this IEnumerable <T> collection, Func<T, bool> predicate) {
			var enumerator = collection.GetEnumerator();
			bool isFound = false;
			while (enumerator.MoveNext() && !isFound) {
				if (predicate (enumerator.Current))
					isFound = true;
			}
			if (isFound)
				return enumerator.Current;
			else 
				return default (T);
		}

		public static TAccumulate Aggregate <T, TAccumulate>( this IEnumerable<T> collection,
			TAccumulate startValue, Func<TAccumulate, T, TAccumulate> accumulate) {
			var accumulator = startValue;
			var enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				accumulator = accumulate (accumulator, enumerator.Current);
			}
			return accumulator;
		}

		public static int Count<T>(this IEnumerable<T> collection, Func <T, bool> predicate) {
			int count = 0;
			var enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (predicate (enumerator.Current))
					count++;
			}
			return count;
		}
	}
}