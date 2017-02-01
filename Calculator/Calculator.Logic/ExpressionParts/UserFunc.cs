using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{

	public class UserFunc: Token {
		public string Name;
		public string Content;

		public UserFunc (string newName, string newContent): base (0) {
			Priority = 100;
			Name = newName;
			Content = newContent;
		}

		override public Token Clone () {
			return new UserFunc (Name, Content);
		}

		override public double Evaluate (){
			return 0;
		}

		override public string Draw (){
			return Name + "{...}";
		}
	}

	static public class UFSearch {
		static MyDictionary <string, UserFunc> UFList = new MyDictionary<string, UserFunc> ();

		static public void RegisterUF (UserFunc newUF) {
			UFList.Add (newUF.Name, newUF);
		}

		static public int Run (string input, int startPosition, out Token operand) {
			operand = null;
			string alias = null;
			int endPosition = Parser.FindName (input, startPosition, out alias);
			alias = Parser.ToLowerCase (alias);
			if (alias == "call") {
				string name;
				endPosition = Parser.FindName (input, endPosition + 1, out name);
				if (!UFList.Contains (name)) {
					operand = UFList [name];
					return endPosition;
				} else
					throw new Exception ("User function " + name + " is not defined");
			} else	if (!UFList.Contains (alias)) {
				if (endPosition != input.Length - 1 && input [endPosition + 1] == '{') {
					string content; 
					//change FindClosingParenthesis to meet other parenthesis type
					endPosition = Parser.FindClosing (input, endPosition + 1, out content);
					UserFunc UF = new UserFunc (alias, content);
					RegisterUF (UF);
					operand = null;
					return endPosition;
				} else
					return -1;
			} else {
				throw new Exception ("User function " + alias + " is already defined");
			}
		}
	}
}

