using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class BuiltInFunc: Operand {
		int operandCount;
		MyList <Operand> operands; 

		public BuiltInFunc (string name, string arguments) : base() {
			operands = new MyList<Operand> ();
			operandCount = 0;
			StringBuilder newArg = new StringBuilder ();
			for (int i = 0; i < arguments.Length; i++) {
				if (arguments [i] == ',') {
					newArg.Append (arguments [i]);
				} else {
					double newValue;
					Parser.FindOperand (newArg.ToString (), 0, out newValue); //change to operandSearcher
					operands.Add (new Operand(newValue)); 
					operandCount++;
					newArg = new StringBuilder ();
				}
			}
		}

		abstract public double Value ();
		abstract public int Search (string input, int startPosition);

		/*static public int FindAlias (string input, int startPosition, out double value) {
			value = 0;
			string aliasString;
			int endPosition = FindName (input, startPosition, out aliasString);
			if (BuiltInFunc.IsFunctionName (aliasString) && endPosition != input.Length - 1 && input [endPosition + 1] == '(') {
				//treat aliasString as variable if no arguments follow?
				string arguments; 
				endPosition = FindClosingParenthesis (input, endPosition + 1, out arguments);
				BuiltInFunc BIF = new BuiltInFunc (aliasString, arguments);
				value = BIF.Calculate ();
			} else if (Variables.IsLocal (aliasString)) {
				value = Variables.GetLocal (aliasString);
			} else {
				throw new Exception ("Invalid expression: " + aliasString + " doesn't exist yet");
			}
			return endPosition;
		}*/

		/*static public BIFName CheckFunctionName (string input) {
			foreach (BIFName name in Enum.GetValues(typeof(BIFName))) {
				if (name.ToString () == Parser.ToLowerCase (input))
					return name;
			}
			return BIFName.undefinedFunc;
		}

		static public bool IsFunctionName (string input) {
			if (CheckFunctionName (input) != BIFName.undefinedFunc)
				return true;
			else 
				return false;
		}*/
	}
}

