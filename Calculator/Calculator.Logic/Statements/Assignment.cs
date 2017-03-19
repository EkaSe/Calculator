using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Assignment: Statement
	{
		string alias; //(un)assigned Variable?
		bool isDeclaration;
		Expression content;

		public Assignment (Scope globals, string alias, string assigned, bool isDeclaration) {
			locals = new Scope (globals);
			this.alias = alias;
			this.isDeclaration = isDeclaration;
			content = new ExpressionBuilder (assigned).ToExpression ();
		}

		public Assignment (Scope globals, string alias) {
			locals = new Scope (globals);
			this.alias = alias;
			isDeclaration = true;
			content = null;
		}

		public Assignment (string alias, string assigned, bool isDeclaration) {
			locals = new Scope (Interpreter.Globals);
			this.alias = alias;
			this.isDeclaration = isDeclaration;
			content = new ExpressionBuilder (assigned).ToExpression ();
		}

		public Assignment (string alias) {
			locals = new Scope (Interpreter.Globals);
			this.alias = alias;
			isDeclaration = true;
			content = null;
		}

		protected override string Execute () {
			if (isDeclaration)
				locals.Reserve (alias);
			if (content != null) {
				string value = content.Process ();
				locals.Assign (alias, Parser.StringToDouble (value));
				return alias + " = " + value;
			} else 
				return alias + " is declared";
		}
	}

	public class AssignmentParser : StatementParser {
		readonly private string keyword = "var";

		override public ParsingResult Run (string input) {
			Assignment result = null;
			bool isMatch = false;
			bool isComplete = false;
			string name = "";
			int position = 0;
			if (Parser.IsIdentifierChar (input [position], true)) {
				position = Parser.FindName (input, position, out name) + 1;//var..
				if (name == keyword) {
					if (position >= input.Length) 
						isMatch = true;//var
					else if (Parser.IsIdentifierChar (input [position], true)) {
						position = Parser.FindName (input, position, out name) + 1; //var name..
						if (position >= input.Length) { //var name
							result = new Assignment (name);
							isMatch = true;
							isComplete = true;
						} else if (input [position] == '=') { //var name = ..
							position++;
							if (position >= input.Length)
								isMatch = true; //var name =
							else {
								try { //var name = ..
									result = new Assignment (name, input.Substring (position), true);
									isMatch = true;
									isComplete = true;
								} catch {
									isMatch = true;
								}
							}
						} 
					}
				} else {//name..
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
			}
			return new ParsingResult (result, isMatch, isComplete);
		}
	}
}

