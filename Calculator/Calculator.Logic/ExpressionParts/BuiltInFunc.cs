using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public enum BIFName {
		undefinedFunc,
		min,
		max,
		sqrt
	};

	public class BuiltInFunc: Operand {
		int operandCount;
		MyList <Operand> operands; 

		public BuiltInFunc (string name, string arguments) : base() {
			BIFName ID = CheckFunctionName (name);
			operands = new MyList<Operand> ();
			operandCount = 0;
			StringBuilder newArg = new StringBuilder ();
			for (int i = 0; i < arguments.Length; i++) {
				if (arguments [i] == ',') {
					newArg.Append (arguments [i]);
				} else {
					double newValue;
					Parser.FindOperand (newArg.ToString (), 0, out newValue);
					operands.Add (new Operand(newValue)); 
					operandCount++;
					newArg = new StringBuilder ();
				}
			}
		}

		public double Calculate () {
			return 0;
		}

		static public BIFName CheckFunctionName (string input) {
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
		}



	}
}

