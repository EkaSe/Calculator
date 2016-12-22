using System;

namespace Calculator.Logic
{
	public class Prerequisites
	{
		static public void RegisterOperators () {
			OperatorSearch.RegisterOperator (new Plus ());
			OperatorSearch.RegisterOperator (new Minus ());
			OperatorSearch.RegisterOperator (new Multiply ());
			OperatorSearch.RegisterOperator (new Divide ());
		}
	}
}

