using System;
using System.Collections.Generic;

namespace MyLibrary
{
	public class OutTree <RootType, LeafType>
	{
		public int BranchCount;
		public OutTree <RootType, LeafType> Root;
		
		public OutTree ()
		{
		}
	}
}