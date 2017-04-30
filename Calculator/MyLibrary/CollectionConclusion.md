### Collections
 
* **Array** is an ordered set of objects of one predefined type. Quantity of elements is constant since array initialization. Elements can be accessed by index.
* **List** is an array of variable length.
* **Stack** is variable size array. Elements can be accessed in reverse order to adding to collection.
* **Dictionary** is a collection that stores pairs of strictly typed elements, one element of pair called key, another called value. Key element is used for accessing the value. Hash table. unique keys

### Interfaces IEnumerable and IEnumerator
Interfaces `IEnumerator<T>` and `IEnumerable<T>` of System.Collections.Generic namespace allow simple iteration over collection.

##### Collection Initialization
Collection initialization can initialize object of any class, which implements IEnumerable and contains method Add(item)
```c#
var myList = new MyList<double> () { 1, 3, 5 };
```
##### foreach
foreach statement can enumerate object of any class, which implements IEnumerable
```c#
foreach (var element in myList) {
    Console.Writeline (element);			 
}
```
##### yield return
yield return statement is used to return each element of a collection one at each call of a method. The return type must be `IEnumerable`, `IEnumerable<T>`, `IEnumerator`, or `IEnumerator<T>`.
```c#
public static IEnumerable<T2> Select<T1, T2> (this IEnumerable<T1> collection, Func<T1, T2> selector) {
			var enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
			    yield return selector(enumerator.Current);
			}
}
```
#### IEnumerator
| Method | Description |
| --- | --- |
| T Current {get;} | returns current element of enumerated collection |
| bool MoveNext () | returns True, if there are remain some other elements in enumerated collection and advances enumerator to this next element |
| void Reset() | sets the enumerator to its initial position which is **before** the first element of the collection |
| void Dispose() | performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources |
| object Current {get;} | is required for implementing the nongeneric IEnumerator interface |

#### IEnumerable
The only method GetEnumerator() returns the enumerator for this collection.

##### IEnumerable Extension Methods 

###### Where

```c#
IEnumerable<TSource> Where<TSource>(
	   this IEnumerable<TSource> source,
	   Func<TSource, bool> predicate
)
```

###### Select
Returns new IEnumerable collection, containing all new elements created by applying selector to each element in the source collection
```c#
IEnumerable<TResult> Select<TSource, TResult>(
    this IEnumerable<TSource> source,
	   Func<TSource, TResult> selector
)
```
###### ToArray
Returns new array of elements of type TSourse, copied from the collection
```c#
TSource[] ToArray<TSource>(
	this IEnumerable<TSource> source
)
```

###### ToList
Returns elements of the collection as `List<TSourse>`
```c#
List<TSource> ToList<TSource>(
	this IEnumerable<TSource> source
)
```
###### ToDictionary
Creates from given TSourse item a pair of key and value, getting them by applying corresponding selectors to the given item, and then adding them to a new `Dictionary<TKey, TElement>` instance.
```c#
Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(
	this IEnumerable<TSource> source,
	Func<TSource, TKey> keySelector,
	Func<TSource, TElement> elementSelector
)
```
###### FirstOrDefault
Returns the very first element of source collection, if it contains any, otherwise returns default value of type TSource.
```c#
TSource FirstOrDefault<TSource>(
	this IEnumerable<TSource> source
)
```

Use this modifier in all public methods of MyEnumerableExtension class.
 fluent-style call
 
 Enumerating-on-demand instead of storing results

##### References:
[IEnumerable](https://msdn.microsoft.com/ru-ru/library/9eekhta0(v=vs.110).aspx)

[IEnumerator](https://msdn.microsoft.com/ru-ru/library/78dfe2yb(v=vs.110).aspx)
