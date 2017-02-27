using System;

namespace Calculator.Logic
{
	abstract public class Statement
	{
		protected VarSet locals = new VarSet (Interpreter.Globals);

		public string Process () {
			locals = new VarSet (Interpreter.Globals);
			string result = Execute ();
			Interpreter.Globals.Merge (locals);
			return result;
		}

		abstract public string Execute ();
	}
}

