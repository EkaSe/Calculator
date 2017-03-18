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
			numbers = globals.numbers;
		}

		public void Merge (Scope locals) {
			numbers = locals.numbers;
		}

		public void Assign (string name, double value) {
			numbers [name] = value;
		}

		public void Assign (Variable var, double value) {
			numbers [var.Name] = value;
		}

		public bool IsVar (string name) {
			if (numbers.Contains (name))
				return true;
			else
				return false;
		}

	}
}

