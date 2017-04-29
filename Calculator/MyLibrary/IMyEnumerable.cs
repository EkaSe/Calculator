using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLibrary
{
	public interface IMyEnumerable<T>: IEnumerable <T>
	{
		IMyEnumerator<T> Enumerator {get;} 
	}
}
