using System;

namespace Lib
{
    public class InvalidExpression : Exception
    {
        public InvalidExpression(string message) : base(message)
        {
        }

        public InvalidExpression(string message, Exception exception):base(message, exception)
        {
        }
    }
}