using System;

namespace Calculator.Logic
{
	public class MinBIF : BuiltInFunc {
		public MinBIF() : base() { name = "min"; }
		public MinBIF (string arguments) : base("min", arguments) {}
		override public Token Clone () {
			MinBIF result = new MinBIF ();
			result.branchCount = this.branchCount;
			result.Arguments = new Token[branchCount];
			return result;
		}

		override public double Evaluate () {
			double result;
			if (Arguments.Length > 0) {
				result = Arguments [0].Evaluate();
				for (int i = 1; i < Arguments.Length; i++) {
					if (result > Arguments [i].Evaluate())
						result = Arguments [i].Evaluate();
				}
			} else 
				throw new CalculatorException ("min function cannot have empty list of arguments");
			return result;
		}

		override public string Draw () {
			return "min";
		}
	}
}

