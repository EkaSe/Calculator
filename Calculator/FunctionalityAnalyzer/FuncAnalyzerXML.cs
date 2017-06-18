using System;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using Calculator.Logic;

namespace FunctionalityAnalyzer
{
	public class FuncAnalyzerXML
	{
		static public void Run (Type childClassType) {
			Assembly assembly = childClassType.Assembly;
			GetReport (assembly);
			//GetReport (report);
			//Console.WriteLine (GetReport (assembly));
		}

		public static string LogPath {
			get {
				if (logPath == null) {
					string currentPath = Directory.GetCurrentDirectory ();
					logPath = currentPath.Substring (0, currentPath.IndexOf (@"FunctionalityAnalyzer")) 
						+ @"FunctionalityAnalyzer/FuncAnLog.xml";
				}
				return logPath;
			}
		}
		static string logPath;

		static void GetReport (Assembly assembly) {
			XDocument doc = new XDocument ();
			XElement main = new XElement ("report", "FuncAnReport");
			main.Add (assembly.GetTypes ()
				.Where ((type) => !type.Name.Contains("AnonStorey"))
				.Select ((t) => {
					XElement type = new XElement ("class", new XAttribute ("name", t.Name), 
						new XAttribute ("baseClass", t.BaseType.Name));
					type.Add (t.GetFields ().Where ((field) => field.DeclaringType != typeof (object))
						.Select ((FieldInfo field) => new XElement ("field", field.ToString ())));
					type.Add (t.GetMethods ().Where ((method) => method.DeclaringType != typeof (object))
						.Select ((MethodInfo method) => new XElement ("method", method.ToString ())));
				return type;
				})
			);
			doc.Add (main);
			doc = XDocument.Parse (XmlFormat (doc.ToString ()), LoadOptions.PreserveWhitespace);
			//XmlFormat (doc.ToString ());
			Console.WriteLine (doc);
			doc.Save (LogPath);
		}

		static public string XmlFormat (string doc) {
			StringBuilder indentedDoc = new StringBuilder ();
			int indentationLevel = -1;
			int position = 0;
			bool previousTagIsClosing = false;
			while (position >= 0 && position < doc.Length - 1) {
				int nextPosition = doc.IndexOf ('<', position);
				string freeText = doc.Substring (position, nextPosition - position);
				indentedDoc.Append (' ' + freeText + ' ');
				string nextTag = FindTag (doc, ref nextPosition);
				if (IsClosingTag (nextTag)) {
					if (previousTagIsClosing)
						indentedDoc.Append ('\n' + Indents (indentationLevel));
					indentedDoc.Append(nextTag);
					previousTagIsClosing = true;
					indentationLevel--;
				} else {
					indentationLevel++;
					indentedDoc.Append ('\n' + Indents (indentationLevel) + nextTag);
					previousTagIsClosing = false;
				}
				position = nextPosition;
			}
			/*
			for (int i = 0; i < doc.Length; i++) {
				if (doc [i] == '<') {
					if (i > 0)
						indentedDoc.AppendLine();
					if (doc [i + 1] != '/')
						indentationLevel++;
					else {
						if (indentationLevel > 0)
							indentedDoc.Append ('\t');
						indentationLevel--;
					}
					for (int j = 0; j < indentationLevel; j++) {
						indentedDoc.Append ('\t');
					}
				}
				indentedDoc.Append (doc [i]);
				if (doc [i] == '>') {
					if (i < doc.Length - 1 && doc [i + 1] != '<') {
						indentedDoc.AppendLine ();
						for (int j = 0; j < indentationLevel + 1; j++) {
							indentedDoc.Append ('\t');
						}
					}
				}
			}*/
			Console.WriteLine (indentedDoc.ToString ());
			return indentedDoc.ToString ();
		}

		static string FindTag (string doc, ref int position) {
			position = doc.IndexOf ('<', position);
			string result = "";
			if (position >= 0)
				position = Parser.FindClosing (doc, position, out result, '<') + 1;
			return '<' + result + '>';
		}

		static bool IsClosingTag (string tag) {
			if (tag.Length >= 3 && tag [0] == '<' && tag [1] == '/' && tag [tag.Length - 1] == '>')
				return true;
			else
				return false;
		}

		static string Indents (int level) {
			StringBuilder result = new StringBuilder ();
			for (int i = 0; i < level; i++) {
				result.Append ('\t');
			}
			return result.ToString ();
		}
	}
}

