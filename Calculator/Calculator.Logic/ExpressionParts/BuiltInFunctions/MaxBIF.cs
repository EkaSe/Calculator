using System;

namespace Calculator.Logic
{
	public class MaxBIF : BuiltInFunc {
		public MaxBIF() : base() { name = "max"; }
		public MaxBIF (string arguments) : base("max", arguments) {}
		override public Token Clone () {
			MaxBIF result = new MaxBIF ();
			result.branchCount = this.branchCount;
			result.Arguments = new Token[branchCount];
			return result;
		}

		override public double Evaluate () {
			double result;
			if (Arguments.Length > 0) {
				result = Arguments [0].Evaluate();
				for (int i = 1; i < Arguments.Length; i++) {
					if (result < Arguments [i].Evaluate())
						result = Arguments [i].Evaluate();
				}
			} else 
				throw new CalculatorException ("max function cannot have empty list of arguments");
			return result;
		}

		override public string Draw () {
			return "max";
		}
	}

}

