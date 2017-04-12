using System;

namespace MyLibrary
{
	public interface IMyEnumerable<T>
	{
		IMyEnumerator<T> Enumerator {get;}
	}
}

