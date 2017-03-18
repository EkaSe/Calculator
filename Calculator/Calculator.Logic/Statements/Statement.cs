using System;

namespace Calculator.Logic
{
	abstract public class Statement
	{
		protected Scope locals = new Scope (Interpreter.Globals);

		public string Process () {
			locals = new Scope (Interpreter.Globals);
			string result = Execute ();
			Interpreter.Globals.Merge (locals);
			return result;
		}

		abstract protected string Execute ();
	}
}

