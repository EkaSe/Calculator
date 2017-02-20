using System;
using MyLibrary;
using System.Text;

namespace Calculator.Logic
{
	public class VarSet
	{
		MyDictionary <string, double> variables;
		public MyDictionary <string, double> Variables {
			get { return variables.Clone (); }
			private set { }
		}

		public double this [string key] {
			get { return variables [key]; }
			set { variables [key] = value; }
		}

		public VarSet () {
			variables = new MyDictionary<string, double> ();
		}

		public VarSet (VarSet globals) {
			variables = globals.Variables;
		}

		public void Merge (VarSet locals) {
			variables = locals.Variables;
		}

		public void Assign (string name, double value) {
			variables [name] = value;
		}

		public void Assign (Variable var, double value) {
			variables [var.Name] = value;
		}

		public bool IsVar (string name) {
			if (variables.Contains (name))
				return true;
			else
				return false;
		}

	}
}

