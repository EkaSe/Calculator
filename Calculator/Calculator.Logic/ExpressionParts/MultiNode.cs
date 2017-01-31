using System;

namespace Calculator.Logic
{
	public class MultiNode <T> {
		public MultiNode<T> Ancestor;
		public MultiNode<T>[] Descendants;
		public int DescendantCount;
		public T Element;
		public int Index;

		public MultiNode (T input) {
			Element = input;
			Ancestor = null;
			Descendants = null;
			DescendantCount = 0;
			Index = -1;
		}

		public MultiNode (T input, int nextCount) {
			Element = input;
			Ancestor = null;
			Descendants = new MultiNode<T>[nextCount];
			DescendantCount = nextCount;
			Index = -1;
		}
	}
}

