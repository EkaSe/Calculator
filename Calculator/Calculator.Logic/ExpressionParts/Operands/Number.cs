using System;

namespace Calculator.Logic
{
	public class Number: Operand
	{
		public Number (double newDouble): base () { value = newDouble; }

		override public double Value {
			get { return this.value; }
			set { this.value = value; }
		}

		override public ExpressionPart Clone () { return new Number (value); }
	}
}

