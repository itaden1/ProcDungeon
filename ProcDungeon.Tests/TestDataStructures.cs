using System;
using Xunit;
using ProcDungeon.Structures;
using ProcDungeon.Algorythms;
using Xunit.Abstractions;
using System.Collections.Generic;


namespace ProcDungeon.Tests
{
    public class TestDataStructures
    {
        public TestDataStructures(ITestOutputHelper output)
        {
            Converter converter = new Converter(output);
            Console.SetOut(converter);
        }
        [Fact]
        public void TestPointGT()
        {
            var p1 = new Point(1,2);
            var p2 = new Point(1,1);
            Assert.True(p1 > p2);
        }
        [Fact]
        public void TestPointLT()
        {
            var p1 = new Point(1,2);
            var p2 = new Point(2,1);
            Assert.True(p1 < p2);
        }
        [Fact]
        public void TestPointEqual()
        {
            var p1 = new Point(1,2);
            var p2 = new Point(1,2);
            Assert.True(p1 == p2);
        }
    }
}