using System;
using System.Text;

namespace MyLibrary
{
	public class MyEnumerableExtensionTest
	{
		public static void Run () {
			var collection = new MyList<int> ();
			for (int i = 1; i <= 100; i++) {
				collection.Add (i);
			}

			Func<int, bool> containsDigitSix = (int arg) => arg.ToString ().Contains ("6");
			Func<int, string> toBinAsString = (int arg) => Convert.ToString (arg, toBase: 2);
			Func<KeyValuePair <int, string>, KeyValuePair <int, int>> countOnesInValues = (KeyValuePair <int, string> arg) => {
				int count = 0;
				for (int i = 0; i < arg.Value.Length; i++) {
					if (arg.Value [i] == '1')
						count++;
				}
				return new KeyValuePair<int, int> (arg.Key, count);
			};

			//var result = MyEnumerableExtension.Where (collection, containsDigitTwo);
			var result0 = MyEnumerableExtension.ToDictionary <int, int, string> (collection, arg => arg, toBinAsString);
			var result1 = MyEnumerableExtension.Select (result0, countOnesInValues);
			var result2 = MyEnumerableExtension.Where <KeyValuePair <int, int>> (result1, arg => arg.Value < 3);
			var result3 = MyEnumerableExtension.Select <KeyValuePair <int, int>, int> (result2, arg => arg.Key);
			var result4 = MyEnumerableExtension.Where (result3, containsDigitSix);

			var resultArray = MyEnumerableExtension.ToArray<int> (result4);

			if (resultArray.Length == 8)
				Console.WriteLine ("My Enumerable Extension test passed");
			else
				Console.WriteLine ("My Enumerable Extension test failed: " + resultArray);
		}

		public static void FluentRun () {
			var collection = new MyList<int> ();
			for (int i = 1; i <= 100; i++) {
				collection.Add (i);
			}

			Func<int, bool> containsDigitSix = (int arg) => arg.ToString ().Contains ("6");
			Func<int, string> toBinAsString = (int arg) => Convert.ToString (arg, toBase: 2);
			Func<KeyValuePair <int, string>, KeyValuePair <int, int>> countOnesInValues = (KeyValuePair <int, string> arg) => {
				int count = 0;
				for (int i = 0; i < arg.Value.Length; i++) {
					if (arg.Value [i] == '1')
						count++;
				}
				return new KeyValuePair<int, int> (arg.Key, count);
			};

			var result = collection.ToDictionary <int, int, string> (arg => arg, toBinAsString)
				.Select (countOnesInValues)
				.Where <KeyValuePair <int, int>> (arg => arg.Value < 3)
				.Select <KeyValuePair <int, int>, int> (arg => arg.Key)
				.Where (containsDigitSix)
				.ToArray<int> ();

			if (result.Length == 8)
				Console.WriteLine ("My Enumerable Extension test passed");
			else
				Console.WriteLine ("My Enumerable Extension test failed: " + result);
		}
	}
}

