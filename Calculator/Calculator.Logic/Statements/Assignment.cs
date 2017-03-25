using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Assignment: Statement
	{
		string alias;
		Statement content;

		public Assignment (Scope globals, string alias, string assigned, bool isDeclaration) {
			locals = new Scope (globals);
			this.alias = alias;
			content = new ExpressionBuilder (assigned).ToExpression ();
		}

		public Assignment (string alias, string assigned, bool isDeclaration) {
			locals = new Scope (Interpreter.Globals);
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
			int position = 0;
			if (Parser.IsIdentifierChar (input [position], true)) {
				position = Parser.FindName (input, position, out name) + 1;
				if (position >= input.Length) //name
						isMatch = true;
				else if (input [position] == '=') {
					position++; //name = ..
					if (position >= input.Length)
						isMatch = true; //name =
						else {
						try { //name = ..
							result = new Assignment (name, input.Substring (position), false);
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

