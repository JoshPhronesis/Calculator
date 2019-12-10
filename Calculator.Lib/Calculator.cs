using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Lib
{
    public class Calculator
    {
        private readonly string expression;
        private readonly char[] expressionInArray;

        public Calculator(string expression)
        {
            this.expression = expression.RemoveWhitespace() ?? throw new ArgumentNullException(nameof(expression));
            this.expressionInArray = this.expression.ToCharArray();

            if (!IsExpressionValid())
                ThrowInvalidException();
        }

        private bool IsExpressionValid()
        {
            if (!IsFirstItemNumber())
                return false;
            if (!IsLastItemNumber())
                return false;
            if (!IsOperatorPositionValid())
                return false;

            return true;
        }

        private bool IsOperatorPositionValid()
        {
            var allowedOperators = new char[] { '+', '-', '*', '/' };

            return !this.expressionInArray.Where((item, i) => allowedOperators.Contains(item) &&
                                                 (!CheckLeftPosition(item, i) || !CheckRightOperator(item, i))).Any();
        }

        private bool CheckRightOperator(char @operator, int index)
        {
            return index == expressionInArray.Length - 1 || int.TryParse(expressionInArray[index + 1].ToString(), out _);
        }

        private bool CheckLeftPosition(char @operator, int index)
        {
            return index == 0 || int.TryParse(expressionInArray[index - 1].ToString(), out _);
        }

        private bool IsLastItemNumber()
        {
            var lastCharacter = expression.ToCharArray()[expression.Length - 1].ToString();
            return int.TryParse(lastCharacter, out _);
        }

        private bool IsFirstItemNumber()
        {
            var firstCharacter = expression.ToCharArray()[0].ToString();
            return int.TryParse(firstCharacter, out _);
        }

        private void ThrowInvalidException()
        {
            throw new InvalidExpression("The expression passed is in a wrong format");
        }

        public double Calculate()
        {
            var expSeparatedByOperators = expression.SplitAndKeep(new char[] { '+', '-', '*', '/' }).ToArray();

            if (ExpContainsDivideOrMultiply())
                expSeparatedByOperators = ProcessDivideAndMultiply(expSeparatedByOperators);

            return ProcessExpression(expSeparatedByOperators);
        }

        private double ProcessExpression(string[] expSeparatedByOperators)
        {
            double result = Convert.ToDouble(expSeparatedByOperators.First());
            for (var index = 0; index < expSeparatedByOperators.Length; index++)
            {
                string item = expSeparatedByOperators[index];
                switch (item)
                {
                    case "+":
                        result += Convert.ToDouble(expSeparatedByOperators[index + 1]);
                        break;
                    case "-":
                        result -= Convert.ToDouble(expSeparatedByOperators[index + 1]);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        private bool ExpContainsDivideOrMultiply()
        {
            return expressionInArray.Any(item => item == '*' || item == '/');
        }

        private string[] ProcessDivideAndMultiply(string[] expSeparatedByOperators)
        {
            for (int i = 0; i < expSeparatedByOperators.Length; i++)
            {
                var item = expSeparatedByOperators[i];
                switch (item)
                {
                    case "/":
                        double divisionResult = EvaluateDivide(item, i, ref expSeparatedByOperators);
                        ModifyOriginalExpression(i, divisionResult, ref expSeparatedByOperators);
                        break;
                    case "*":
                        double multiplicationResult = EvaluateMultiply(item, i, ref expSeparatedByOperators);
                        ModifyOriginalExpression(i, multiplicationResult, ref expSeparatedByOperators);
                        break;
                    default:
                        break;
                }
            }

            return expSeparatedByOperators.Where(item => item != null).ToArray();
        }

        private void ModifyOriginalExpression(int index, double itemToInsert, ref string[] expSeparatedByOperators)
        {
            expSeparatedByOperators[index - 1] = itemToInsert.ToString(CultureInfo.InvariantCulture);
            expSeparatedByOperators[index] = null;
            expSeparatedByOperators[index + 1] = null;
            expSeparatedByOperators = expSeparatedByOperators.Where(item => item != null).ToArray();
        }

        private double EvaluateMultiply(string item, int index, ref string[] expSeparatedByOperators)
        {
            double.TryParse(expSeparatedByOperators[index - 1], out var leftOperand);
            double.TryParse(expSeparatedByOperators[index + 1], out var rightOperand);

            return leftOperand * rightOperand;
        }

        private double EvaluateDivide(string item, int index, ref string[] expSeparatedByOperators)
        {
            double.TryParse(expSeparatedByOperators[index - 1], out var leftOperand);
            double.TryParse(expSeparatedByOperators[index + 1], out var rightOperand);
            if (rightOperand == 0)
            {
                ThrowInvalidException();
            }
            return leftOperand / rightOperand;
        }
    }
}
