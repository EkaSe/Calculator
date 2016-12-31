using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Minus: BinaryOp {
		public Minus (): base(1) {}

		protected override Operand PerformOperation (Operand operand1, Operand operand2) {
			return new Number (operand1.Value - operand2.Value);
		}

		public override int Search (string input, int startPosition) {
			return SearchBySign (input, startPosition, '-');
		}
	}
}

