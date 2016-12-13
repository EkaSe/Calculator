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

	public class BuiltInFunc
	{
		BIFName ID = BIFName.undefinedFunc;
		int operandCount;
		MyList <Operand> operands;

		public BuiltInFunc () {
		}

		public BuiltInFunc (string name, string arguments) {
			ID = CheckFunctionName (name);
			double arg;
			//int endPosition = Parser.FindOperand (arguments, 0, out arg);
			//endPosition++;
			//endPostion == arguments.Length || arguments [endPosition] == ","
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

