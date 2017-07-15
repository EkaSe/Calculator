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

			var testCaseList = testList.Where ((mInfo) => (mInfo.GetCustomAttributes (typeof(TestCaseAttribute)).Count () > 0));
			var throwsList = testList.Where ((mInfo) => mInfo.GetCustomAttributes (typeof(ThrowsAttribute)).Count () > 0);
			var simpleTestList = testList.Except (testCaseList).Except (throwsList);

			var testResults = 
				testCaseList.SelectMany <MethodInfo, StringBuilder> ((mInfo) => {
					var testCaseResults = mInfo.GetCustomAttributes (typeof (TestCaseAttribute))
						.Select ((attribute) => (TestCaseAttribute) attribute)
						.Select <TestCaseAttribute, StringBuilder> ((testCase) => {
							try {
								mInfo.Invoke (null, testCase.Args);
								Console.WriteLine ((new StringBuilder (mInfo.Name + "[pass]")).ToString ());
								return (new StringBuilder (mInfo.Name + testCase.Args + "[pass]"));
							} catch (Exception e) {
								int argCount = 0;
								StringBuilder args = mInfo.GetParameters ().Take (mInfo.GetParameters().Count() - 1)
									.Aggregate (new StringBuilder(), (sentence, pInfo) => 
										sentence.AppendFormat (", {0} = {1}", pInfo.Name, testCase.Args [argCount]));
								StringBuilder testResult = new StringBuilder ();
								return (testResult.AppendFormat ("{0} [fail, given{1} returns {2} instead of {3}]\n",
									mInfo.Name, args.ToString (), e.Message, testCase.Args [argCount++]));
							} 
						});
					return (testCaseResults);
					}
				)
				.Concat (throwsList.SelectMany <MethodInfo, StringBuilder> ((mInfo) => {
					var throwsResults = mInfo.GetCustomAttributes (typeof (ThrowsAttribute))
					.Select ((attribute) => {
						try {
							mInfo.Invoke (null, null);
							return (new StringBuilder (mInfo.Name + "[fail]\n"));
						} catch (Exception e) {
							//to do
							return (new StringBuilder (mInfo.Name + "[pass]\n"));
							}});
					return (throwsResults);
				}))
				.Concat (simpleTestList.Select <MethodInfo, StringBuilder> ((mInfo) => {
					try {
						mInfo.Invoke (null, null);
						return (new StringBuilder (mInfo.Name + "[pass]\n"));
					} catch (Exception e) {
						return (new StringBuilder (mInfo.Name + "[fail]\n"));
					}
					return (new StringBuilder ());
				})
				);
			
			foreach (StringBuilder result in testResults) {
				Console.WriteLine (result.ToString ());
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
