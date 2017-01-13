using System;
using MyLibrary;
using System.Text;

namespace Calculator.Logic
{
	public class VarSearch {
		static public int Run (string input, int startPosition, out Token operand) {
			operand = null;
			string alias;
			int endPosition = Parser.FindName (input, startPosition, out alias);
			if (endPosition >= 0) {
				if (Variables.IsLocal (alias))
					operand = new Variable (alias, Variables.GetLocal (alias));
				else
					throw new Exception ("Invalid expression: " + alias + " doesn't exist yet");
			}
			return endPosition;
		}
	}

	public class Variables
	{
		static MyDictionary <string, double> locals;
		static MyDictionary <string, double> variables = new MyDictionary<string, double> ();

		static public void CreateLocals () {
			locals = variables;
		}

		static public void MergeLocals() {
			variables = locals;
		}

		static public void ClearDictionaries () {
			variables = new MyDictionary<string, double> ();
		}

		static public bool CheckVariable (string input) {
			if (input.Length == 0)
				return false;
			if (variables.Contains (input))
				return true;
			if (!Parser.IsIdentifierChar (input [0], true))
				return false;
			for (int i = 0; i < input.Length; i++) {
				if (!Parser.IsIdentifierChar (input [i], false))
					return false;
			}
			return true;
		}

		static public void AssignVariable (string name, double value) {
			variables [name] = value;
		}

		static public void AssignLocal (string name, double value) {
			locals [name] = value;
		}

		static public double GetLocal (string name) {
			return locals [name];
		}

		static public bool IsVariable (string name) {
			if (variables.Contains (name))
				return true;
			else
				return false;
		}

		static public bool IsLocal (string name) {
			if (locals.Contains (name))
				return true;
			else
				return false;
		}
	}
}