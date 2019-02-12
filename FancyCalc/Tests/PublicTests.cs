using System;
using NUnit.Framework;

namespace FancyCalc
{
    [TestFixture]
    public class FancyCalculatorTests
    {

        [Test]
        public void AddTest()
        {
            FancyCalcEnguine calc = new FancyCalcEnguine();
            double expected = 4;
            double actual = calc.Add(2, 2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SubtractTest()
        {
            var calc = new FancyCalcEnguine();
            double expected = 0;
            double actual = calc.Subtract(1, 1);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(3, 3, ExpectedResult = 9)]
        [TestCase(1, 0, ExpectedResult = 0)]
        public double MultiplyTest(int a, int b)
        {
            var calc = new FancyCalcEnguine();
            return calc.Multiply(a, b);
        }

        [TestCase("3+3", ExpectedResult = 6)]
        [TestCase("1 *  0", ExpectedResult = 0)]
        [TestCase("10 -  9", ExpectedResult = 1)]
        [TestCase("2 + 2    *2", ExpectedResult = 6)]
        [TestCase("(2+ 2)*2", ExpectedResult = 8)]
        [TestCase("(2+ 2)*2    ", ExpectedResult = 8)]
        [TestCase("(2+ 2)*2;", ExpectedResult = 8)]
        [TestCase("-4 + 5", ExpectedResult = 1)]
        [TestCase("-4 - -5", ExpectedResult = 1)]
        [TestCase("-4 * -5", ExpectedResult = 20)]
        [TestCase("-4 * 5", ExpectedResult = -20)]
        [TestCase("Abs(-5)", ExpectedResult = 5)]
        [TestCase("-Abs(-5)", ExpectedResult = -5)]
        [TestCase("Abs((((2+2)*5)+Abs(3*-5))*-1)", ExpectedResult = 35)]
        [TestCase("(((2+2)*5)+Abs(3*-5))*-1", ExpectedResult = -35)]
        public double CalculateTestHidden(string expression)
        {
            FancyCalcEnguine calc = new FancyCalcEnguine();
            return calc.Culculate(expression);
        }

        [Test]
        [Property("Mark", 5)]
        public void CalculateTest_IfArgument_IsNull_Throws_ArgumentNullException()
             => Assert.Throws<ArgumentNullException>(() => new FancyCalcEnguine().Culculate(null)
            , "Expression for Calcilate method cannot be null");


        #region Easter Egg was found 👍

        //[TestCase("3+3", ExpectedResult = 6)]
        //[TestCase("1 *  0", ExpectedResult = 0)]
        //[TestCase("10 -  9", ExpectedResult = 1)]
        //[Property("Mark", 5)]
        //public double CalculateTestHidden(string expression)
        //{
        //    FancyCalcEnguine calc = new FancyCalcEnguine();
        //    return calc.Culculate(expression);
        //}

        //[Test]
        //[Property("Mark", 5)]
        //public void CalculateTest_IfArgument_IsNull_Throws_ArgumentNullException()
        //     => Assert.Throws<ArgumentNullException>(() => new FancyCalcEnguine().Culculate(null)
        //    , "Expression for Calcilate method cannot be null");


        //[Test]
        //public void AddTestHidden()
        //{
        //    FancyCalcEnguine calc = new FancyCalcEnguine();
        //    double expected = 4;
        //    double actual = calc.Add(2, 2);
        //    Assert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void SubtractTestHidden()
        //{
        //    FancyCalcEnguine calc = new FancyCalcEnguine();
        //    double expected = 0;
        //    double actual = calc.Subtract(1, 1);
        //    Assert.AreEqual(expected, actual);
        //}


        //[TestCase(3, 3, ExpectedResult = 9)]
        //[TestCase(1, 0, ExpectedResult = 0)]
        //public double MultiplyTestHidden(int a, int b)
        //{
        //    FancyCalcEnguine calc = new FancyCalcEnguine();
        //    return calc.Multiply(a, b);
        //}

        #endregion
    }
}
