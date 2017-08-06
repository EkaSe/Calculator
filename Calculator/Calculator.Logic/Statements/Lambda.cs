using System;
using MyLibrary;

namespace Calculator.Logic
{
	public class Lambda: Statement
	{
		public string Name;
		Statement content;

		public Lambda (string name, string body) {
			this.Name = name;
			content = StatementSearcher.Run (body);
		}

		protected override string Execute () {
			throw new CalculatorException ("Not implemented");
			string result = content.Process ();
			return result;
		}
	}

	public class LambdaParser : StatementParser {
		readonly private string keywordFunction = "function";
		readonly private string keywordVar = "var";
		readonly private string keywordReturn = "return";

		override public ParsingResult Run (string input) {
			Lambda result = null;
			bool isMatch = false;
			bool isComplete = false;
			/*string name = "";
			int position = 0;
			if (Parser.IsIdentifierChar (input [position], true)) {
				position = Parser.FindName (input, position, out name) + 1;//var..
				if (name == keywordVar) {
					position++;
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
			}*/
			return new ParsingResult (result, isMatch, isComplete);
		}
	}
}

