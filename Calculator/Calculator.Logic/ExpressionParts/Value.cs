using System;

namespace Calculator.Logic
{
	public class Value
	{
		readonly public double RawValue;
		readonly public Type Type;

		public Value (Type type, double newValue) {
			RawValue = newValue;
			Type = type;
		}

		public Value (double newValue) {
			RawValue = newValue;
			Type = typeof (double);
		}
	}
}

