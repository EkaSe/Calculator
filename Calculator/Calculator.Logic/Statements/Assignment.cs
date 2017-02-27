using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Assignment: Statement
	{
		string alias; //(un)assigned Variable?
		Variable assignee;
		Expression content;

		public Assignment (VarSet globals, string alias, string assigned) {
			locals = new VarSet (globals);
			this.alias = alias;
			content = new ExpressionBuilder (assigned).ToExpression ();
		}

		public override string Execute () {
			throw new Exception ("Not implemented");
			return alias + "=" + Parser.DoubleToString (content.Calculate ());
		}
	}

	public class AssignmentParser : StatementParser {
		override public ParsingResult Run (string input) {
			throw new Exception ("Not implemented");
			Assignment result;
			return new ParsingResult (result);
		}
	}
}

