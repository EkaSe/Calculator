﻿using System;

namespace Calculator.Logic
{
	public class Number: Operand
	{
		public Number (double newDouble): base () { value = newDouble; }

		override public double Value {
			get { return this.value; }
			set { this.value = value; }
		}

		override public Token Clone () { return new Number (value); }

		override public string Draw () {
			return Parser.DoubleToString (value);
		}
	}
}

