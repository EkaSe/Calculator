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
				string output;
				if (Parser.ToLowerCase (input) != "q") {
					try {
						Statement result = StatementSearcher.Run (input);
						output = result.Process ();
					} catch (Exception e) {
						output = e.Message;
					}
				} else output = "q";
				finish = outputAction (output);
			}
		}
	}
}