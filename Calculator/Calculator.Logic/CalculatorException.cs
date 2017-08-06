using System;

namespace Calculator.Logic
{
	public class CalculatorException : Exception
	{
		public CalculatorException () {}

		public CalculatorException (string message) : base(message) {}

		public CalculatorException (string message, Exception inner) : base(message, inner) {}
	}
}