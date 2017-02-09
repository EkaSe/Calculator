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
			OperatorSearch.RegisterOperator (new Factorial ());
			//OperatorSearch.RegisterOperator (new Assign ());
		}
		static public void RegisterBIFs () {
			BIFSearch.RegisterBIF (new MaxBIF ());
			BIFSearch.RegisterBIF (new MinBIF ());
			BIFSearch.RegisterBIF (new SqrtBIF ());
		}
	}
}

