using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Multiply: BinaryOp {
		public Multiply (): base((int) Priorities.mult) {}

		protected override Operand PerformOperation (Operand operand1, Operand operand2) {
			return new Number (operand1.Value * operand2.Value);
		}

		public override int Search (string input, int startPosition) {
			return SearchBySign (input, startPosition, '*');
		}

		override public Token Clone () {
			return new Multiply ();
		}

		override public string Draw () {
			return "*";
		}
	}
}

