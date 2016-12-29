using System;

namespace Calculator.Logic
{
	public class Variable: Operand
	{
		public string name;

		public Variable (): base () {}

		public double Value {
			get { return Variables.GetLocal (name); }
			set { Variables.AssignLocal (name, value); }
		}

	}
}

