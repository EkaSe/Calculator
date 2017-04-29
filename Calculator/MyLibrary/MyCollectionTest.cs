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

			if (testPassed)
				Console.WriteLine ("My list test passed");
			else
				Console.WriteLine ("My list test failed: " + resultValues);
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

			if (resultValues[0] == 1 && resultValues[1] == 3 && resultValues[2] == 5)
				Console.WriteLine ("My linked list test passed");
			else
				Console.WriteLine ("My linked list test failed: " + resultValues);
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

			if (resultValues[0] == 5 && resultValues[1] == 3 && resultValues[2] == 1)
				Console.WriteLine ("My stack test passed");
			else
				Console.WriteLine ("My stack test failed: " + resultValues);
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

			if (resultValues[0] == 1 && resultValues[1] == 5 && resultValues[2] == 3)
				Console.WriteLine ("My dictionary test passed");
			else
				Console.WriteLine ("My dictionary test failed: " + resultValues);
		}
	}
}

