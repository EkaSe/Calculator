using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Operand
	{
		protected double value;

		public Operand () {}
		public Operand (double number) {
			value = number;
		}

		/*public Operand (string alias) {
			variable = alias;
			//check if exists in locals? add?
			type = operandType.variable;
		}*/

		public double Value {
			get { 
				return this.value;
			}
			protected set { 
				this.value = value;
			}
		}
	}
}

