using System;
using System.Reflection;
using System.Linq;

namespace Calculator.Tests
{
	public static class TestRunner
	{
		public static void RunTests (Assembly assembly) {
			var testList = assembly.GetTypes ()
				.Where ((type) => type.GetCustomAttribute (typeof (TestFixtureAttribute)) != null)
				.SelectMany ((testClass) => testClass.GetMethods ())
				.Where ((mInfo) => mInfo.GetCustomAttribute (typeof (TestAttribute)) != null);

			var TestResults = testList.SelectMany ((mInfo) => {
				mInfo.GetCustomAttributes (typeof (TestCaseAttribute))
					.Select ((attribute) => (TestCaseAttribute) attribute)
					.Select ((testCase) => {
						try {
							mInfo.Invoke (null, testCase.Args);
							return (mInfo.Name + "[pass]");
						} catch (Exception e) {
							int argCount = 0;
							string args = mInfo.GetParameters ().Take (mInfo.GetParameters().Count() - 1)
								.Aggregate ("", (sentence, pInfo) => sentence + ", " + pInfo.Name 
									+ " = " + testCase.Args [argCount++]);
							return (mInfo.Name + "[fail, given " + args + "returns " + e.Message 
								+ " instead of " + testCase.Args [argCount] + "]");
						}
					})
					.Concat (mInfo.GetCustomAttributes (typeof (ThrowsAttribute))
						.Select ((attribute) => {
							try {
								mInfo.Invoke (null, null);
								return (mInfo.Name + "[fail]");
							} catch (Exception e) {
								//to do
								return (mInfo.Name + "[]");
							}
						})
					);
				try {
					mInfo.Invoke (null, null);
					return (mInfo.Name + "[pass]");
				} catch (Exception e) {
					int argCount = 0;
					return (mInfo.Name + "[fail]");
				};
			});
		}
	}
}
