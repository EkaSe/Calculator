using System;
using System.Reflection;
using System.Linq;
using System.Text;
using Calculator.Logic;

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
							try{	
								try {
									mInfo.Invoke (null, testCase.Args);
									return (new StringBuilder (mInfo.Name + "[pass]"));
								} catch (TargetInvocationException e) {
									throw e.InnerException;
								}
							} catch (TestFailedException e) {
								int argCount = 0;
								StringBuilder args = mInfo.GetParameters ().Take (mInfo.GetParameters().Count() - 1)
									.Aggregate (new StringBuilder(), (sentence, pInfo) => 
										sentence.AppendFormat (", {0} = {1}", pInfo.Name, testCase.Args [argCount]));
								StringBuilder testResult = new StringBuilder ();
								return (testResult.AppendFormat ("{0} [fail, given{1} returns {2} instead of {3}]\n",
									mInfo.Name, args.ToString (), e.Message, testCase.Args [argCount+1]));
							}
						});
					return (testCaseResults);
					}
				)
				.Concat (throwsList.SelectMany <MethodInfo, StringBuilder> ((mInfo) => {
					var throwsResults = mInfo.GetCustomAttributes (typeof (ThrowsAttribute))
					.Select ((attribute) => {
						try {
							try {
								mInfo.Invoke (null, null);
								return (new StringBuilder (mInfo.Name + "[pass]\n"));
							} catch (TargetInvocationException e) {
								throw e.InnerException;
							}
						} catch (TestFailedException e) {
							return (new StringBuilder (mInfo.Name + "[fail]\n"));
						}
						});
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
				throw new TestFailedException (testResult.ToString ());
		}

		public static void ShouldThrow (this MethodBase method, 
			Type exceptionType, params object[] args) {

			bool exceptionThrown = false;
			try {
				method.Invoke (null, args);
			} catch (Exception e) {
				exceptionThrown = true;
				if (e.InnerException.GetType () != exceptionType)
					throw new TestFailedException (e.InnerException.Message);
			}
			if (!exceptionThrown)
				throw new TestFailedException ();
		}
	}

	public class TestFailedException: Exception
	{
		public TestFailedException() {}

		public TestFailedException(string message) : base(message) {}

		public TestFailedException(string message, Exception inner) : base(message, inner) {}
	}

	[TestFixture (typeof (Interpreter))]
	public class TestRunnerTest {
		[Test]
		[Covers (nameof (Interpreter.Run), 
			new Type[] { typeof (Func<string>), typeof (Func<string, bool>), typeof (bool)})]
		[TestCase ("1+1", "Test test cases: failed test")]
		public static void TestFailedTestCase (string input, string expectedOutput) {
			string [] expressionSet = new string [] {input};
			string result = InterpreterTest.RunInterpreter (expressionSet);
			result.ShouldBeEqual (expectedOutput);
		}

		[Test]
		[Covers (nameof (TestThrow))]
		[Throws (typeof (CalculatorException))]
		public static void ShouldThrowProperException () {
			var testThrow = typeof (TestRunnerTest).GetMethod (nameof (TestThrow));
			testThrow.ShouldThrow (typeof (CalculatorException), true);
		}

		[Test]
		[Covers (nameof (TestThrow))]
		[Throws (typeof (CalculatorException))]
		public static void ShouldThrowFails () {
			var testThrow = typeof (TestRunnerTest).GetMethod (nameof (TestThrow));
			testThrow.ShouldThrow (typeof (CalculatorException), false);
		}

		static public void TestThrow (bool throwException) {
			if (throwException)
				throw new CalculatorException ("exception text");
		}
	}
}