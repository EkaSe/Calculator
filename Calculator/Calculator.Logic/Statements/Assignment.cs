using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Assignment: Statement
	{
		string alias; //(un)assigned Variable?
		//Variable assignee;
		Expression content;

		public Assignment (VarSet globals, string alias, string assigned) {
			locals = new VarSet (globals);
			this.alias = alias;
			content = new ExpressionBuilder (assigned).ToExpression ();
		}

		public Assignment (string alias, string assigned) {
			locals = new VarSet (Interpreter.Globals);
			this.alias = alias;
			content = new ExpressionBuilder (assigned).ToExpression ();
		}

		protected override string Execute () {
			string value = content.Process ();
			locals.Assign (alias, Parser.StringToDouble (value));
			return alias + " = " + value;
		}
	}

	public class AssignmentParser : StatementParser {
		override public ParsingResult Run (string input) {
			Assignment result = null;
			bool isMatch = false;
			bool isComplete = false;
			string name = "";
			if (Parser.IsIdentifierChar (input [0], true)) {
				int position = Parser.FindName (input, 0, out name) + 1;
				if (position >= input.Length)
					isMatch = true;
				else if (input [position] == '=') {
					position++;
					if (position >= input.Length)
						isMatch = true;
					else {
						try {
							result = new Assignment (name, input.Substring (position));
							isMatch = true;
							isComplete = true;
						} catch {
							isMatch = true;
						}
					}
				}
			}
			return new ParsingResult (result, isMatch, isComplete);
		}
	}
}

