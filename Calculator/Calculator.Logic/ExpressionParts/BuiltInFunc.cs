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

		static protected int SearchAlias (string input, int startPosition, string alias) {
			return startPosition;
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
	}

	public class BIFSearch {
		static MyList<BuiltInFunc> BIFList = new MyList<BuiltInFunc> ();

		static public void RegisterBIF (BuiltInFunc newBIF) {
			BIFList.Add (newBIF);
		}

		static public int Run (string input, int startPosition, out BuiltInFunc BIF) {
			int position = -1;
			BIF = null;
			for (int i = 0; i < BIFList.Length; i++) {
				BuiltInFunc currentBIF = BIFList [i];
				int currentPosition = currentBIF.Search (input, startPosition);
				if (currentPosition > 0 && (currentPosition < position || position < 0)) {
					position = currentPosition;
					BIF = currentBIF;
				}
			}
			return position;
		}
	}

	public class MinBIF : BuiltInFunc {
		static string alias = "min";

		public MinBIF (string name, string arguments) : base(name, arguments) {}

		public double Value () {
		}

		public int Search (string input, int startPosition) {
		}
	}
}

