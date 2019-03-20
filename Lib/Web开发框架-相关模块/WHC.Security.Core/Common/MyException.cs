using System;

namespace WHC.Security.Common
{
	public class MyException : Exception
	{
		public MyException() : base("")
		{
		}

		public MyException(string message) : base(message)
		{
		}
	}
}
