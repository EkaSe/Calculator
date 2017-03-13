using System;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class Interpreter
	{
		static public VarSet Globals = new VarSet ();

		static public void Run (Func<string> getInput, Func<string, bool> outputAction) {
			Run (getInput, outputAction, true);
		}

		static public void Run (Func<string> getInput, Func<string, bool> outputAction, bool clearRun) {
			if (clearRun) {
				//clear UF list
				Globals = new VarSet ();
			}
			bool finish = false;
			while (!finish) {
				string input = getInput ();
				bool isSuppressed = false;
				while (input == "")
					input = getInput ();
				ParsedStream stream = new ParsedStream (input);
				string output = "";
				while (!stream.IsEnd) {
					
					string inputLine = stream.GetStatement (out isSuppressed);
					//call Run() recursively
					//check if line is ended with ; (supress output)
					if (Parser.ToLowerCase (inputLine) != "q") {
						try {
							Statement result = StatementSearcher.Run (inputLine);
							output = result.Process ();
							if (isSuppressed)
								output = "";
						} catch (Exception e) {
							output = e.Message;
						}
					} else output = "q";
				}
				finish = outputAction (output);
			}
		}
	}
}