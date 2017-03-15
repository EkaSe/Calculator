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
			throw new Exception ("Not implemented");
			string result = content.Process ();
			return result;
		}
	}

	public class LambdaParser : StatementParser {
		override public ParsingResult Run (string input) {
			Lambda result;
			return new ParsingResult (null, false, false);
		}
	}
}

