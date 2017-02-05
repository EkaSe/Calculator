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
		static MyStack <MyDictionary <string, double>> varStack = new MyStack<MyDictionary<string, double>> ();
		static MyDictionary <string, double> locals = new MyDictionary<string, double> ();
		//static MyDictionary <string, double> globals = new MyDictionary<string, double> ();

		static public void CreateLocals () {
			varStack.Push (locals);
			locals = locals.Clone ();
		}

		static public void MergeLocals() {
			varStack.Pop ();
		}

		static public void ClearLocals () {
			locals = varStack.Pop ();
		}

		static public void ClearDictionaries () {
			varStack = new MyStack<MyDictionary<string, double>> ();
			locals = new MyDictionary<string, double> ();
		}

		static public bool CheckVariable (string input) {
			if (input.Length == 0)
				return false;
			if (locals.Contains (input))
				return true;
			if (!Parser.IsIdentifierChar (input [0], true))
				return false;
			for (int i = 0; i < input.Length; i++) {
				if (!Parser.IsIdentifierChar (input [i], false))
					return false;
			}
			return true;
		}

		static public void AssignLocal (string name, double value) {
			locals [name] = value;
		}

		static public double GetLocal (string name) {
			return locals [name];
		}

		static public bool IsLocal (string name) {
			if (locals.Contains (name))
				return true;
			else
				return false;
		}
	}
}