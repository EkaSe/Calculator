using System;
using System.Collections.Generic;
/* Посимвольно считываю строку, введенную пользователем, смотрю, что за символ попался:
цифра, пустой символ (пробел), знаки математических действий, скобки. По мере чтения строки формирую числовой массив, где чередуются числа и коды математических операций (как быть с унарными операциями? - пока не поняла). Вернее, даже двумерный массив, потому что для каждому элементу первого массива сопоставляю число - приоритет (1 - число, 2 - сложение и вычитание, 3 - умножение и деление, 4 - возведение в степень). 
Потом собственно к вычислениям. Сначала выполняю операции с наибольшим приоритетом, в качестве аргументов используя числа слева и справа от позиции операции. По выполнении действия было бы хорошо заменить кусок массива из трех элементов (число + операция + число) на один элемент - новое число, но чтобы не переписывать весь массив, меняя его длину, можно записать в массив на одну из трех освободившихся позиций результат операции, а остальные два элемента пропускать при дальнейших действиях (например можно ввести приоритет 0 - игнорируемый элемент массива). 
И соответственно дальше выполнять действия по мере убывания приоритета, пока не останется одно число
 
 динамичные коллекции, такие как List и другие. 
Насчёт приоритетов: можно не делать приоритетов для операндов, сделать только для операторов, причём статические, т.е. определенному оператору всегда соответствует определённый приоритет. В таком случае можно просто ограничиться методов, который возвращает приоритет для указанного оператора, а соответствия приоритетов оператором внутри этого метода можно жёстко прописать в коде.
А чтобы разбираться со скобками можно использовать рекурсию, т.е. выделять целиком все подскобочное выражение и вызывать метод вычисления уже для него.

 вызывающий метод когда находит открывающую скобка проматывает строчку до тех пор, пока на найдёт соответствующую закрывающую скобку (именно соответствующую - скобки могут быть сколько угодно раз вложены), а затем выделяет всю подстрока и вызывает снова себя, но уже с этой подстрокой. А если не находит соответствующей закрывающей скобки - сообщает об ошибке.*/

namespace Calculator
{
	public class Calculator
	{
		public enum OperatorCode {
			plus
		};

		static double Calculate (List<double> expression) {
			double result = expression[0];
			for (int i = 1; i < expression.Count; i += 2) {
				switch ((int) expression [i]) {
				case (int) OperatorCode.plus:
					result = expression [i - 1] + expression [i + 1];
					break;
				}
			}
			return result;
		}

		static public string ProcessExpression (string input)
		{
			List<double> expression = new List<double> ();
			double currentNumber = 0;
			double currentDigit = 0;
			double mantissaLength = 1;
			string result;
			for (int i = 0; i < input.Length; i++) {
				string currentSymbol = Convert.ToString (input [i]);
				switch (currentSymbol) {
				case "0":
					currentDigit = 0;
					break;
				case "1":
					currentDigit = 1;
					break;
				case "2":
					currentDigit = 2;
					break;
				case "3":
					currentDigit = 3;
					break;
				case "4":
					currentDigit = 4;
					break;
				case "5":
					currentDigit = 5;
					break;
				case "6":
					currentDigit = 6;
					break;
				case "7":
					currentDigit = 7;
					break;
				case "8":
					currentDigit = 8;
					break;
				case "9":
					currentDigit = 9;
					break;
				case ".": 
					mantissaLength = 0.1;
					break;
				case " ": 
					break;
				case "+": 
					expression.Add (currentNumber);
					currentNumber = 0;
					currentDigit = 0;
					mantissaLength = 1;
					expression.Add ((float)OperatorCode.plus);
					break;
				default:
					result = "Invalid symbol: " + currentSymbol;
					return result;
				}
				if (mantissaLength == 1)
					currentNumber = currentNumber * 10 + currentDigit;
				else {
					currentNumber = currentNumber + currentDigit * mantissaLength;
					mantissaLength *= 0.1;
				}
			}
			expression.Add (currentNumber);
			result = Convert.ToString (Calculate (expression));
			return result;
		}
	}
}