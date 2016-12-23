using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class BuiltInFunc: Operand {
		protected int operandCount;
		protected MyList <Operand> operands; 
		protected string name;

		public BuiltInFunc (string arguments) : base() {
			SetArguments (arguments);
		}

		public void SetArguments (string arguments) {
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
			value = Evaluate ();
		}

		public bool CheckName (string alias) {
			if (name == Parser.ToLowerCase (alias))
				return true;
			else return false;
		}

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

		static public bool Run (string alias, out BuiltInFunc BIF) {
			BIF = null;
			for (int i = 0; i < BIFList.Length; i++) {
				BuiltInFunc currentBIF = BIFList [i];
				if (currentBIF.CheckName (alias)) {
					BIF = currentBIF; 
					return true;
				}
			}
			return false;
		}
	}

	public class MinBIF : BuiltInFunc {
		public MinBIF (string arguments) : base(arguments) {
			name = "min";
			SetArguments (arguments);
		}

		protected double Evaluate () {
			if (operands.Length > 0) {
				value = operands [0].Value;
				for (int i = 1; i < operands.Length; i++) {
					if (value > operands [i].Value)
						value = operands [i].Value;
				}
			} else 
				throw new Exception ("min function cannot have empty list of arguments");
			return value;
		}
	}
}