using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Assign: BinaryOp {
		public Assign (): base(0) {}

		protected override Operand PerformOperation (Operand operand1, Operand operand2) {
			if (operand1 is Variable)
				Variables.AssignLocal ((Variable) operand1, operand2.Evaluate ());
			else
				throw new Exception ("Invalid expression: Cannot assign value to " + operand1.Draw ());

			/*int assignPosition = statement.IndexOf ('=');
			if (assignPosition > 0) {
				string expression = statement.Substring (assignPosition + 1);
				if (expression.IndexOf ('=') >= 0)
					throw new Exception ("Invalid expression: Assignment under assignment");
				string assignee = statement.Substring (0, assignPosition);
				if (Variables.CheckVariable (assignee) && (expression.IndexOf (assignee) < 0 || Variables.IsLocal (assignee))) {
					Expression tree = new ExpressionBuilder (expression).ToExpression ();
					string value = Parser.DoubleToString (tree.Calculate ());
					result = assignee + " = " + value;
					Variables.AssignLocal (assignee, Parser.StringToDouble (value));
				} else
					throw new Exception ("Invalid expression: Cannot assign value to " + assignee);
			}*/

			return new Number (operand1.Evaluate ());
		}

		public override int Search (string input, int startPosition) {
			return SearchBySign (input, startPosition, '=');
		}

		override public Token Clone () {
			return new Assign ();
		}

		override public string Draw () {
			return "=";
		}
	}
}


