using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Factorial: UnaryOp {
		public Factorial (): base (4) {}

		protected override Operand PerformOperation (Operand operand) {
			int arg = (int) operand.Value;
			if (operand.Value != arg || arg < 0) 
				throw new Exception ("Invalid expression: Factorial is defined only for non-negative integers");
			long result = 1;
			for (int i = 1; i <= arg; i++) {
				result = result * i;
			}
			return new Operand ((double) result);
		}

		//to be removed
		protected override Operand PerformOperation (MyList<Operand> operands) {
			Operand operand = operands [0];
			return PerformOperation (operand);
		}

		public override int Search (string input, int startPosition) {
			return SearchBySign (input, startPosition, '!');
		}
	}
}