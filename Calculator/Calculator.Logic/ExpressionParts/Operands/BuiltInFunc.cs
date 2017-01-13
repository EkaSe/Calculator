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

		public BuiltInFunc (string arguments) : 
		base() {
			SetArguments (arguments);
		}

		//abstract protected double Evaluate ();

		public void SetArguments (string arguments) {
			operands = new MyList<Operand> ();
			operandCount = 0;
			string restOfArgs = arguments;
			Func <string, int, bool> endCondition = (input, currentPosition) => {
				if (input [currentPosition] == ',')
					return true;
				else return false;
			};
			while (restOfArgs != "") {
				if (restOfArgs [0] == ',')
					restOfArgs = restOfArgs.Substring (1);
				Expression tree = new Expression (restOfArgs, endCondition, out restOfArgs);
				double newArg = tree.Calculate ();
				operands.Add (new Number (newArg)); 
				operandCount++;
			}
			value = Evaluate ();
		}
	}

	static public class BIFSearch {
		static MyDictionary <string, BuiltInFunc> BIFList = new MyDictionary<string, BuiltInFunc> ();

		static public void RegisterBIF (BuiltInFunc newBIF) {
			BIFList.Add (newBIF.name, newBIF);
		}

		static public int Run (string input, int startPosition, out Token operand) {
			operand = null;
			string alias = null;
			int endPosition = Parser.FindName (input, startPosition, out alias);
			alias = Parser.ToLowerCase (alias);
			if (!BIFList.Contains (alias))
				return -1;
			else if (endPosition != input.Length - 1 && input [endPosition + 1] == '(') {
				string arguments; 
				endPosition = Parser.FindClosingParenthesis (input, endPosition + 1, out arguments);
				BuiltInFunc BIF = (BuiltInFunc) BIFList [alias].Clone();
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
		override public Token Clone () {
			return new MinBIF ();
		}

		override public double Evaluate () {
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
		public MaxBIF (string arguments) : base("max", arguments) {}
		override public Token Clone () {
			return new MaxBIF ();
		}

		override public double Evaluate () {
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
		public SqrtBIF (string arguments) : base("sqrt", arguments) {}

		override public Token Clone () {
			return new SqrtBIF ();
		}

		override public double Evaluate () {
			if (operandCount == 1) {
				value = System.Math.Sqrt (operands [0].Value);
			} else 
				throw new Exception ("sqrt function can have only 1 argument");
			return value;
		}
	}
}