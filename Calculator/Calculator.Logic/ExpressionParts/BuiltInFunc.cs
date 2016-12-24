using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	abstract public class BuiltInFunc: Operand {
		protected int operandCount;
		protected MyList <Operand> operands; 
		public string name;

		public BuiltInFunc () : base() {
		}

		public BuiltInFunc (string newName, string arguments) : base() {
			name = newName;
			SetArguments (arguments);
		}

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
					Operand newOperand = null;
					OperandSearch.Run (newArg.ToString (), 0, out newOperand);
					operands.Add (newOperand); 
					operandCount++;
					newArg = new StringBuilder ();
				}
			}
			value = Evaluate ();
		}
	}

	public class BIFSearch {
		static MyDictionary <string, BuiltInFunc> BIFList = new MyDictionary<string, BuiltInFunc> ();

		static public void RegisterBIF (BuiltInFunc newBIF) {
			BIFList.Add (newBIF.name, newBIF);
		}

		static public int Run (string input, int startPosition, out Operand operand) {
			operand = null;
			string alias = null;
			int endPosition = Parser.FindName (input, startPosition, out alias);
			if (!BIFList.Contains (alias))
				return -1;
			else if (endPosition != input.Length - 1 && input [endPosition + 1] == '(') {
				string arguments; 
				endPosition = Parser.FindClosingParenthesis (input, endPosition + 1, out arguments);
				BuiltInFunc BIF = BIFList [alias];
				BIF.SetArguments (arguments);
				operand = BIF;
				return endPosition;
			} else
				return -1;
		}
	}

	public class MinBIF : BuiltInFunc {
		public MinBIF() : base() { name = "min"; }
		public MinBIF (string arguments) : base("min", arguments) {}

		new protected double Evaluate () {
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

	public class MaxBIF : BuiltInFunc {
		public MaxBIF() : base() { name = "max"; }
		public MaxBIF (string arguments) : base("max", arguments) {
		}

		new protected double Evaluate () {
			if (operands.Length > 0) {
				value = operands [0].Value;
				for (int i = 1; i < operands.Length; i++) {
					if (value < operands [i].Value)
						value = operands [i].Value;
				}
			} else 
				throw new Exception ("max function cannot have empty list of arguments");
			return value;
		}
	}

	public class SqrtBIF : BuiltInFunc {
		public SqrtBIF() : base() { name = "sqrt"; }

		public SqrtBIF (string arguments) : base(arguments) {
			name = "sqrt";
			SetArguments (arguments);
		}

		new protected double Evaluate () {
			if (operandCount == 1) {
				value = System.Math.Sqrt (operands [0].Value);
			} else 
				throw new Exception ("sqrt function can have only 1 argument");
			return value;
		}
	}
}