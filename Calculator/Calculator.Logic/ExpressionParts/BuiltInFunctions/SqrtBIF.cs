using System;

namespace Calculator.Logic
{
	public class SqrtBIF : BuiltInFunc {
		public SqrtBIF() : base() { name = "sqrt"; }
		public SqrtBIF (string arguments) : base("sqrt", arguments) {}

		override public Token Clone () {
			SqrtBIF result = new SqrtBIF ();
			result.branchCount = this.branchCount;
			result.Arguments = new Token[branchCount];
			return result;
		}

		override public double Evaluate () {
			double result;
			if (branchCount == 1) {
				result = System.Math.Sqrt (Arguments [0].Evaluate());
			} else 
				throw new Exception ("sqrt function can have only 1 argument");
			return result;
		}

		override public string Draw () {
			return "sqrt";
		}
	}
}

