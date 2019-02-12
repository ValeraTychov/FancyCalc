using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyCalc
{
    public class FancyCalcEnguine
    {
        public double Add(int a, int b)
        {
            return a + b;
        }

        public double Subtract(int a, int b)
        {
            return a - b;
        }

        public double Multiply(int a, int b)
        {
            return a * b;
        }

        //generic calc method. usage: "10 + 20"  => result 30
        public double Culculate(string expression)
        {
            if (expression == null)
                throw new ArgumentNullException();

            Parser parser = new Parser();
            Expression exp = parser.Parse(expression);
            return exp.Calculate();

        }
    }

    public interface IToken
    {
        string Sign { get; set; }
        double Function(List<Expression> arguments);
    }

    public interface IBinaryOperator
    {
        int Priority { get; }
    }

    public class NumberToken : IToken
    {
        double value;

        public NumberToken(double value)
        {
            this.value = value;
            Sign = "number";
        }

        public string Sign { get; set; }

        public double Function(List<Expression> arguments)
        {
            return value;
        }
    }

    public class AbsToken : IToken
    {
        public string Sign { get; set; } = "Abs";

        public double Function(List<Expression> arguments)
        {
            if (arguments.Count == 1)
                return Math.Abs(arguments[0].Calculate());

            throw new ArgumentException();
        }
    }

    public class MinusToken : IToken, IBinaryOperator
    {
        public string Sign { get; set; } = "-";

        public int Priority => 1;

        public double Function(List<Expression> arguments)
        {
            if (arguments.Count == 1)
                return -arguments[0].Calculate();
            if (arguments.Count == 2)
                return arguments[0].Calculate() - arguments[1].Calculate();
            throw new ArgumentException();
        }
    }

    public class PlusToken : IToken, IBinaryOperator
    {
        public string Sign { get; set; } = "+";

        public int Priority => 1;

        public double Function(List<Expression> arguments)
        {
            if (arguments.Count < 2)
                throw new ArgumentException();

            return arguments[0].Calculate() + arguments[1].Calculate();
        }
    }

    public class MultiplyToken : IToken, IBinaryOperator
    {
        public string Sign { get; set; } = "*";

        public int Priority => 2;

        public double Function(List<Expression> arguments)
        {
            if (arguments.Count < 2)
                throw new ArgumentException();

            return arguments[0].Calculate() * arguments[1].Calculate();
        }
    }

    public class OpenParenthesisToken: IToken
    {
        public string Sign { get; set; } = "(";

        public double Function(List<Expression> arguments)
        {
            throw new NotImplementedException();
        }
    }

    public class CloseParenthesisToken: IToken
    {
        public string Sign { get; set; } = ")";

        public double Function(List<Expression> arguments)
        {
            throw new NotImplementedException();
        }
    }

    public class SemicolonToken : IToken
    {
        public string Sign { get; set; } = ";";

        public double Function(List<Expression> arguments)
        {
            throw new NotImplementedException();
        }
    }

    public class Expression
    {
        IToken token;
        List<Expression> arguments;

        public Expression (IToken token)
        {
            this.token = token;
        }

        public Expression (IToken token, Expression argument) : this(token)
        {
            arguments = new List<Expression>() { argument };
        }

        public Expression (IToken token, Expression leftArgument, Expression rightArgument) : this(token, leftArgument)
        {
            arguments.Add(rightArgument);
        }

        public double Calculate()
        {
            return token.Function(arguments);
        }

        public IToken GetToken()
        {
            return token;
        }
    }

    public class Parser
    {
        List<IToken> availableOperators;

        string expression;

        int index;

        public Parser()
        {
            availableOperators = new List<IToken>()
            {
                new AbsToken(),
                new MinusToken(),
                new PlusToken(),
                new MultiplyToken(),
                new OpenParenthesisToken(),
                new CloseParenthesisToken(),
                new SemicolonToken()
            };
        }

        public Expression Parse(string expression)
        {
            this.expression = expression;

            return ParseBinaryExpression(0);
        }

        private Expression ParseBinaryExpression(int minPriority)
        {
            int operatorPriority;

            Expression leftExpression = ParseUnaryExpression();

            IToken operatorToken = ParseToken();

            if (operatorToken is SemicolonToken)
                return leftExpression;

            if (!(operatorToken is IBinaryOperator))
            {
                index -= operatorToken.Sign.Length;
                return leftExpression;
            }
            else
            {
                operatorPriority = ((IBinaryOperator)operatorToken).Priority;
            }

            if (operatorPriority < minPriority)
            {
                index -= operatorToken.Sign.Length;
                return leftExpression;
            }

            Expression rightExpression = ParseBinaryExpression(operatorPriority);

            return new Expression(operatorToken, leftExpression, rightExpression);
        }

        private Expression ParseUnaryExpression()
        {
            IToken token = ParseToken();
            if (token is NumberToken)
                return new Expression(token);

            if (token is OpenParenthesisToken)
            {
                Expression inParenthesisExpression = ParseBinaryExpression(0);

                if (!(ParseToken() is CloseParenthesisToken)) throw new ArgumentException(") expected");
                return inParenthesisExpression;
            }

            return new Expression(token, ParseUnaryExpression());
        }

        private IToken ParseToken()
        {
            GetSubstring(() => Char.IsWhiteSpace(expression[index]));
            
            string number = GetSubstring(() => Char.IsDigit(expression[index]));

            if (number.Length > 0)
            {
                return new NumberToken(Double.Parse(number));
            }

            for (int i = 0; i < availableOperators.Count; i++)
            {
                int indexStartPosition = index;
                Func<bool> condition = () =>
                {
                    bool firstCondition = (index - indexStartPosition) < availableOperators[i].Sign.Length;
                    bool secondCondition = Contains(availableOperators[i].Sign, expression[index]);
                    return firstCondition && secondCondition;
                };
                string operatorToken = GetSubstring(condition);
                if (operatorToken == availableOperators[i].Sign)
                    return availableOperators[i];
                else index = indexStartPosition;
            }

            if (index < expression.Length)
                throw new ArgumentException("Wrong expression format");

            return new SemicolonToken();
        }

        bool Contains(string text, char symbol)
        {
            for (int i = 0; i < text.Length; i++)
                if (text[i] == symbol) return true;
            return false;
        }

        string GetSubstring(Func<bool> condition)
        {
            StringBuilder stringBuilder = new StringBuilder();

            while (index < expression.Length && condition())
            {
                stringBuilder.Append(expression[index++]);
            }

            return stringBuilder.ToString();
        }

    }
}
