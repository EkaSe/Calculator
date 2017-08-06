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
				else if (endPosition < input.Length - 1 && input [endPosition + 1] == '=') {
					// ? operand.value - system.NullReferenceException
					operand = new Variable (alias, 0);
					Variables.CreateUnassigned (alias);
				} else
					throw new CalculatorException ("Invalid expression: " + alias + " doesn't exist yet");
			}
			return endPosition;
		}
	}

	public class Variables
	{
		static MyStack <MyDictionary <string, double>> varStack = new MyStack<MyDictionary<string, double>> ();
		static MyDictionary <string, double> locals = new MyDictionary<string, double> ();
		static MyDictionary <string, bool> unassigned = new MyDictionary<string, bool> ();

		static public void CreateLocals () {
			if (locals != null) {
				varStack.Push (locals);
				locals = locals.Clone ();
			}
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
			unassigned = new MyDictionary<string, bool> ();
		}

		static public void AssignLocal (string name, double value) {
			locals [name] = value;
			if (unassigned [name])
				unassigned.Remove (name);
		}

		static public void AssignLocal (Variable var, double value) {
			locals [var.Name] = value;
			if (unassigned [var.Name])
				unassigned.Remove (var.Name);
		}

		static public void CreateUnassigned (string name) {
			unassigned [name] = true;
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

		static public bool IsVar (string name) {
			if (locals.Contains (name) || unassigned.Contains (name))
				return true;
			else
				return false;
		}
	}
}