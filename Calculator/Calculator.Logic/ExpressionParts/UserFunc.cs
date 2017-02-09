using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class UserFunc: Token {
		public string Name;
		public string Content;

		public UserFunc (string name, string сontent): base (0) {
			Priority = (int) Priorities.operand;
			Name = name;
			Content = сontent;
		}

		override public Token Clone () {
			return new UserFunc (Name, Content);
		}

		override public double Evaluate () {
			double result = 0;
			int contentPosition = 0;

			Func<string> getExpression = () => {
				if (contentPosition < Content.Length) {
					int start = contentPosition;
					contentPosition = Content.IndexOf ('\n', contentPosition);
					if (contentPosition < 0)
						contentPosition = Content.Length;
					return Content.Substring (start, contentPosition - start);
				} else return "q";
			};

			Func<string, bool> outputAction = (output) => {
				if (output != "q") {
					result = Parser.StringToDouble (output);
					return false;
				} else {
					return true;
				}
			};

			Interpreter.Run (getExpression, outputAction, false);

			return result;
		}

		override public string Draw (){
			return Name + "{...}";
		}
	}

	static public class UFSearcher {
		static MyDictionary <string, UserFunc> UFList = new MyDictionary<string, UserFunc> ();

		static public void RegisterUF (UserFunc newUF) {
			UFList.Add (newUF.Name, newUF);
		}

		static public int Run (string input, int startPosition, out Token operand) {
			operand = null;
			string alias = null;
			int endPosition = Parser.FindName (input, startPosition, out alias);
			if (endPosition != input.Length - 1 && input [endPosition + 1] == '{') {
				if (UFList.Contains (alias))
					throw new Exception ("User function " + alias + " is already defined");
				else {
					string content; 
					endPosition = Parser.FindClosing (input, endPosition + 1, out content, '{');
					UserFunc UF = new UserFunc (alias, content);
					RegisterUF (UF);
					operand = null;
					return endPosition;
				}
			} else if (UFList.Contains (alias)) {
				operand = UFList [alias];
				return endPosition;
			} else
				return -1;
		}
	}
}

