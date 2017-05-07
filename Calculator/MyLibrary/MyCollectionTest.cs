using System;
using System.Collections.Generic;

namespace MyLibrary
{
	public class MyCollectionTest
	{
		static public void Run () {
			MyListTest ();
			MyLinkedListTest ();
			MyStackTest ();
			MyDictionaryTest ();
			MyEnumerableExtensionTest.Run ();
			MyEnumerableExtensionTest.FluentRun ();
		}

		static public Func <string, bool> OutputFunc = (string output) => {
			Console.WriteLine (output);
			return true;
		};

		static public void MyListTest () {
			var myList = new MyList<double> () { 1, 3, 5 };

			var resultValues = new MyList<double> ();
			var enumerator = myList.GetEnumerator ();
			enumerator.Reset();
			while (enumerator.MoveNext ()) {
				var value = enumerator.Current;
				resultValues.Add(value);
			}
			enumerator.Reset ();
			bool testPassed = true;
			int i = 0;
			foreach (var element in myList) {
				if (resultValues [i++] != element) {
					testPassed = false;
					break;
				}
			}

			string testResult;
			if (testPassed)
				testResult = "My list test passed";
			else
				testResult = "My list test failed: " + resultValues;
			OutputFunc (testResult);
		}

		static public void MyLinkedListTest () {
			var myList = new MyLinkedList<double> () { 1, 3, 5 };

			var resultValues = new MyList<double> ();
			var enumerator = myList.GetEnumerator ();
			enumerator.Reset();
			while (enumerator.MoveNext ()) {
				var value = enumerator.Current;
				resultValues.Add(value);
			}

			string testResult;
			if (resultValues[0] == 1 && resultValues[1] == 3 && resultValues[2] == 5)
				testResult = "My linked list test passed";
			else
				testResult = "My linked list test failed: " + resultValues;
			OutputFunc (testResult);
		}

		static public void MyStackTest () {
			var myList = new MyStack<double> () { 1, 3, 5 };

			var resultValues = new MyList<double> ();
			var enumerator = myList.GetEnumerator ();
			enumerator.Reset();
			while (enumerator.MoveNext ()) {
				var value = enumerator.Current;
				resultValues.Add(value);
			}

			string testResult;
			if (resultValues[0] == 5 && resultValues[1] == 3 && resultValues[2] == 1)
				testResult = "My stack test passed";
			else
				testResult = "My stack test failed: " + resultValues;
			OutputFunc (testResult);
		}

		static public void MyDictionaryTest () {
			var myList = new MyDictionary<string, double> () {{"one", 1}, {"three", 3}, {"five", 5}};
			//myList.Add ("one", 1);
			//myList.Add ("three", 3);
			//myList.Add ("five", 5);

			var resultValues = new MyList<double> ();
			var enumerator = myList.GetEnumerator ();
			enumerator.Reset();
			while (enumerator.MoveNext ()) {
				var value = enumerator.Current.Value;
				resultValues.Add(value);
			}

			string testResult;
			if (resultValues[0] == 1 && resultValues[1] == 5 && resultValues[2] == 3)
				testResult = "My dictionary test passed";
			else
				testResult = "My dictionary test failed: " + resultValues;
			OutputFunc (testResult);
		}
	}
}

