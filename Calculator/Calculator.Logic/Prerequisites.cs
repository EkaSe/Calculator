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
		}
		static public void RegisterBIFs () {
			BIFSearch.RegisterBIF (new MaxBIF ());
			BIFSearch.RegisterBIF (new MinBIF ());
			BIFSearch.RegisterBIF (new SqrtBIF ());
		}

		static public void RegisterStatements () {
			StatementSearcher.Register (new DeclarationParser ());
			StatementSearcher.Register (new AssignmentParser ());
			StatementSearcher.Register (new BlockParser ());
			StatementSearcher.Register (new LambdaParser ());
		}
	}
}

