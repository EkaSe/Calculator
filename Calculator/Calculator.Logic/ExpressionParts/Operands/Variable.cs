using System;

namespace Calculator.Logic
{
	public class Variable: Operand
	{
		public string name;

		public Variable (): base () {}

		public Variable (string newName, double newValue): base () {
			name = newName;
			this.Value = newValue;
		}

		override public double Value {
			get { return Variables.GetLocal (name); }
			set { Variables.AssignLocal (name, value); }
		}
	}
}

