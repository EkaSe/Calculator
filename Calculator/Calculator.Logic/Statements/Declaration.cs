using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Declaration: Statement
	{
		public string alias;

		public Declaration (Scope globals, string alias) {
			locals = new Scope (globals);
			this.alias = alias;
		}
	
		public Declaration (string alias) {
			locals = new Scope (Interpreter.Globals);
			this.alias = alias;
		}

		protected override string Execute () {
			locals.Reserve (alias);
			return alias + " is declared";
		}
	}


	public class DeclarationParser : StatementParser {
		static private string keyword = "var";

		override public ParsingResult Run (string input) {
			ParsingResult result = null;
			int position = FindDeclaration (input, out result);
			if (position < input.Length)
				result.isMatch = false;
			return result;
		}

		public static int FindDeclaration (string input, out ParsingResult declaration) {
			Declaration result = null;
			bool isMatch = false;
			bool isComplete = false;
			string name = "";
			int position = 0;
			if (Parser.IsIdentifierChar (input [position], true)) {
				position = Parser.FindName (input, position, out name) + 1;//var..
				if (name == keyword) {
					position++;
					if (position >= input.Length)
						isMatch = true;//var
					else if (Parser.IsIdentifierChar (input [position], true)) {
						position = Parser.FindName (input, position, out name) + 1; //var name..
						if (position >= input.Length) { //var name
							result = new Declaration (name);
							isMatch = true;
							isComplete = true;
						} else {//var name..
							result = new Declaration (name);
							isMatch = true;
						}
					}
				}
			}
			declaration = new ParsingResult (result, isMatch, isComplete);
			if (!isMatch)
				position = -1;
			return position;
		}
	}
}



