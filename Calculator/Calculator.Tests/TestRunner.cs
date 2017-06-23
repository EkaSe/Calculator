﻿using System;
using System.Reflection;
using System.Linq;

namespace Calculator.Tests
{
	public static class TestRunner
	{
		public static void RunTests (Assembly assembly) {
			var TestList = assembly.GetTypes ()
				.Where ((type) => type.GetCustomAttribute (typeof (TestFixtureAttribute)) != null)
				.SelectMany ((testClass) => testClass.GetMethods ())
				.Where ((mInfo) => mInfo.GetCustomAttribute (typeof (TestAttribute)) != null);

			var TestResults = TestList.SelectMany ((mInfo) => {
				if (mInfo.GetCustomAttributes (typeof (TestCaseAttribute)).Count () != 0) {
					mInfo.GetCustomAttributes (typeof (TestCaseAttribute))
						.Select ((testCase) => try {
							mInfo.Invoke (testCase.Args);
							return (mInfo.Name + "[pass]");
						} catch (Exception e) {
							int argCount = 0;
							string args = mInfo.GetParameters ().Take (mInfo.GetParameters().Count() - 1)
								.Aggregate ((sentence, pInfo) => sentence + ", " + pInfo.Name + " = " + testCase.Args [argCount++]);
							return (mInfo.Name + "[fail, given " + args + "returns " + e.Message + " instead of " + testCase.Args [argsCount] + "]");
						})
				} else if (mInfo.GetCustomAttributes (typeof (ThrowsAttribute)).Count () != 0) {
					try {
						mInfo.Invoke (testCase.Args);
						return (mInfo.Name + "[fail]");
					} catch (Exception e) {
						//to do
						return (mInfo.Name + "[]");
					}
				} else {
					try {
						mInfo.Invoke (testCase.Args);
						return (mInfo.Name + "[pass]");
					} catch (Exception e) {
						int argCount = 0;
						string args = mInfo.GetParameters ().Take (mInfo.GetParameters().Count() - 1)
							.Aggregate ((sentence, pInfo) => sentence + ", " + pInfo.Name + " = " + testCase.Args [argCount++]);
						return (mInfo.Name + "[fail]");
					})
				}
			});
		}
	}
}
