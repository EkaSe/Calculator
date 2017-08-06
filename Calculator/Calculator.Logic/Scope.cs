using System;
using MyLibrary;
using System.Text;

namespace Calculator.Logic
{
	public class Scope
	{
		private Scope parent;
		MyDictionary <string, double> numbers;
		MyDictionary <string, Lambda> lambdas;
		MyDictionary <string, bool> reserved; //name, mayBeChanged: keywords, unassigned variables 

		public double this [string key] {
			get { return numbers [key]; }
			set { numbers [key] = value; }
		}
		/*
		private string Find (string dictionaryName) {
			
		}*/


		public Scope () {
			numbers = new MyDictionary<string, double> ();
		}

		public Scope (Scope globals) {
			numbers = globals.numbers.Clone();
		}

		public void Merge (Scope locals) {
			numbers = locals.numbers;
		}

		public void Assign (string name, double value) {
			if (reserved.Contains (name) && !reserved [name])
				numbers [name] = value;
			else
				throw new CalculatorException (name + " is not declared");
		}

		public bool IsVar (string name) {
			if (numbers.Contains (name))
				return true;
			else
				return false;
		}

		public void Reserve (string name, bool isKeyword) {
			if (reserved.Contains (name))
				throw new CalculatorException (name + " is already reserved");
			else
				reserved.Add (name,isKeyword);
		}

		public void Reserve (string name) {
			Reserve (name, false);
		}
	}
}

