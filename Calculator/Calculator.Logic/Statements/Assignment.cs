using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Assignment: Statement
	{
		string alias; //(un)assigned Variable?
		bool isDeclaration;
		Statement content;//Expression or lambda

		public Assignment (Scope globals, string alias, string assigned, bool isDeclaration) {
			locals = new Scope (globals);
			this.alias = alias;
			this.isDeclaration = isDeclaration;
			content = new ExpressionBuilder (assigned).ToExpression ();
		}

		public Assignment (string alias, string assigned, bool isDeclaration) {
			locals = new Scope (Interpreter.Globals);
			this.alias = alias;
			this.isDeclaration = isDeclaration;
			content = new ExpressionBuilder (assigned).ToExpression ();
		}

		protected override string Execute () {
			if (isDeclaration)
				locals.Reserve (alias);
			string value = content.Process ();
			locals.Assign (alias, Parser.StringToDouble (value));
			return alias + " = " + value;

		}
	}

	public class AssignmentParser : StatementParser {

		override public ParsingResult Run (string input) {
			ParsingResult declaration = null;
			Assignment result = null;
			bool isMatch = false;
			bool isComplete = false;
			int position = DeclarationParser.FindDeclaration (input, out declaration);
			if (position < 0) {
				position = 0;
				string name = "";
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

				return new ParsingResult (null, false, false);
			}
			if (position >= input.Length)
				return new ParsingResult (null, true, false);
			else if (input [position] == '=') { //var name = ..
				position++;
				if (position >= input.Length)
					isMatch = true; //var name =
				else {
					try { //var name = ..
						result = new Assignment (((Declaration) declaration.result).alias, input.Substring (position), true);
						isMatch = true;
						isComplete = true;
					} catch {
						isMatch = true;
					}
				}
			} 
			return new ParsingResult (result, isMatch, isComplete);
		}
	}
}

