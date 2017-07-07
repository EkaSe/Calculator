using System;
using System.Reflection;
using System.Linq;
using System.Text;

namespace Calculator.Tests
{
	public static class TestRunner
	{
		public static void RunTests (Assembly testsAssembly) {
			var testList = testsAssembly.GetTypes ()
				.Where ((type) => type.GetCustomAttribute (typeof (TestFixtureAttribute)) != null)
				.SelectMany ((testClass) => testClass.GetMethods ())
				.Where ((mInfo) => mInfo.GetCustomAttribute (typeof (TestAttribute)) != null);

			var testResults = testList.SelectMany<MethodInfo, StringBuilder> ((mInfo) => {
				mInfo.GetCustomAttributes (typeof (TestCaseAttribute))
					.Select ((attribute) => (TestCaseAttribute) attribute)
					.Select <TestCaseAttribute, StringBuilder> ((testCase) => {
						try {
							mInfo.Invoke (null, testCase.Args);
							return (new StringBuilder (mInfo.Name + "[pass]"));
						} catch (Exception e) {
							int argCount = 0;
							StringBuilder args = mInfo.GetParameters ().Take (mInfo.GetParameters().Count() - 1)
								.Aggregate (new StringBuilder(), (sentence, pInfo) => 
									sentence.AppendFormat (", {1} = {2}", pInfo.Name, testCase.Args [argCount++]));
							StringBuilder testResult = new StringBuilder ();
							return (testResult.AppendFormat ("{0} [fail, given {1} returns {2} instead of {3}]\n",
								mInfo.Name, args.ToString (), e.Message, testCase.Args [argCount]));
						}
					})
					.Concat (mInfo.GetCustomAttributes (typeof (ThrowsAttribute))
						.Select ((attribute) => {
							try {
								mInfo.Invoke (null, null);
								return (new StringBuilder (mInfo.Name + "[fail]\n"));
							} catch (Exception e) {
								//to do
								return (new StringBuilder (mInfo.Name + "[pass]\n"));
							}
						})
					);
				try {
					mInfo.Invoke (null, null);
					StringBuilder[] result = {new StringBuilder (mInfo.Name + "[pass]\n")};
					return (result);
				} catch (Exception e) {
					StringBuilder[] result = {new StringBuilder (mInfo.Name + "[fail]\n")};
					return (result);
				};
			});

			foreach (StringBuilder result in testResults) {
				OnOutputMessage (result.ToString ());
			}
		}

		static void OnOutputMessage(string message)
		{
			EventHandler<string> handler = OutputMessage;
			if (handler != null)
			{
				handler(null, message);
			}
		}

		static public event EventHandler<string> OutputMessage;

		public static void ShouldBeEqual (this object testResult, object expected) {
			if (!testResult.Equals (expected))
				throw new TestFailedException ();
		}
	}

	public class TestFailedException: Exception
	{
		public TestFailedException() {}

		public TestFailedException(string message) : base(message) {}

		public TestFailedException(string message, Exception inner) : base(message, inner) {}
	}
}
