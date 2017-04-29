using System;
using System.Collections;
using System.Collections.Generic;

namespace MyLibrary
{
	public interface IMyEnumerator<T>: IEnumerator <T>
	{
		T Current {get;} //returns the current element of enumerating collection
		bool HasNext {get;}//returns True, if there are remain some other elements in enumerating collection, that can be enumerated
		void Next();//change ** Current** value to the next not enumerated yet element of collection
		void Reset(); //reset **Current
	}
}

