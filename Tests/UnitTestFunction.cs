using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CefPreter;
using CefPreter.Types;
using CefPreter.Function;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class UnitTestFuntion
    {
        [TestMethod]
        public void TestToNumber()
        {
            string num = "f 12 ";
            ToNumber toNumber = new ToNumber();
            toNumber.Parameters = new List<Token>() { num.toToken(CefType.StringLiteral) };
            var res =  toNumber.Exec(null);
            
            res.Wait();
            Assert.IsNotNull(res.Result is Number);
            Assert.AreEqual((int)res.Result.Value, 12);
        }

        [TestMethod]
        public void TestGreater()
        {
            
            IsGreater toNumber = new IsGreater();
            toNumber.Parameters = new List<Token>() { "12".toToken(CefType.NumberLiteral), "-2".toToken(CefType.NumberLiteral) };
            var res = toNumber.Exec(null);

            res.Wait();

            Assert.AreEqual((int)res.Result.Value, 1);
        }

        [TestMethod]
        public void TestLess()
        {

            IsLess toNumber = new IsLess();
            toNumber.Parameters = new List<Token>() { "-12".toToken(CefType.NumberLiteral), "2".toToken(CefType.NumberLiteral) };
            var res = toNumber.Exec(null);

            res.Wait();

            Assert.AreEqual((int)res.Result.Value, 1);
        }

        [TestMethod]
        public void TestEqual()
        {
            AreEqual toNumber = new AreEqual();
            toNumber.Parameters = new List<Token>() { "-12".toToken(CefType.NumberLiteral), "-12".toToken(CefType.NumberLiteral) };
            var res = toNumber.Exec(null);

            res.Wait();

            Assert.AreEqual((int)res.Result.Value, 1);
        }

      

        [TestMethod]
        public void TestToStr()
        {


            ToStr toStr = new ToStr();
            toStr.Parameters = new List<Token>() { "val".toToken(CefType.StringLiteral) };

            var res = toStr.Exec(null);
            res.Wait();




            Assert.AreEqual((string)res.Result.Value, "val");
        }


    }
}
