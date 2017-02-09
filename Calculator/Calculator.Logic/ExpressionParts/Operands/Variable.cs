using System;

namespace Calculator.Logic
{
	public class Variable: Operand
	{
		public string Name;

		public Variable (): base () {}

		public Variable (string newName, double newValue): base () {
			Name = newName;
			this.Value = newValue;
		}

		override public double Value {
			get { return Variables.GetLocal (Name); }
			set { Variables.AssignLocal (Name, value); }
		}

		override public Token Clone () {
			//actual variable cloning has no sense since initializing of variables with the same name is not allowed
			return this;
		}

		override public string Draw () {
			return Name;
		}
	}
}

