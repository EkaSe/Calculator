using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Operand
	{
		enum operandType {
			number,
			variable,
			function
		}
		operandType type;

		double number;
		string variable;
		BuiltInFunc function;

		public Operand (double newNumber) {
			number = newNumber;
			type = operandType.number;
		}

		public Operand (string alias) {
			variable = alias;
			//check if exists in locals? add?
			type = operandType.variable;
		}

		public Operand (BuiltInFunc BIF) {
			function = BIF;
			type = operandType.function;
		}

		public double Value {
			get { 
				double result = 0;
				switch (this.type) {
				case operandType.number:
					result = number;
					break;
				case operandType.variable:
					result = Variables.GetLocal (variable); //local?
					break;
				case operandType.function:
					result = function.Calculate ();
					break;
				default:
					throw new Exception ("Undefined operand type");
				}
				return result;
			}
			set { 
				switch (this.type) {
				case operandType.number:
					number = value;
					break;
				case operandType.variable:
					Variables.AssignLocal (variable, value); //local?
					break;
				case operandType.function:
					throw new Exception ("Invalid assignment: Attempt to assign value to built-in function");
				}
			}
		}

	}
}

