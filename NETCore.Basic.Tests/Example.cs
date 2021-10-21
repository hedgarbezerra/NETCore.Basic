using AutoFixture;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NETCore.Basic.Tests
{

    public class TestExampleTests
    {
        private MockRepository mockRepository;


        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }


        public void TestMethod1()
        {
            // Arrange
            var test = mockRepository.Of<ITest>()
                            .Where(s => s.Say() == "Hi!")
                            .Where(s => s.DoSomething(5, 4) == 20)
                            .Where(s => s.DoSomething(4, 5) == 9)
                            .Where(s => s.DoSomething(It.IsAny<int>(), 7) == 99)
                            .Where(s => s.Prop == 1)
                            .First();

            var fixture = new Fixture();
            fixture.Inject<Stream>(Stream.Null);


            var streams = fixture.Create<Stream>();

            var test2 = fixture.Build<ITest>().Do(x => x.Props.Add(1))
                .With(x => x.Props, new List<int>())
                .Without(x => x.Prop)
                .Create();

            var strBase2 = mockRepository.Create<ITest>().SetupSequence(x => x.DoSomething(1, 2)).Returns(2);

            // Act


            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
    public interface ITest
    {
        public int Prop { get; set; }
        public List<int> Props { get; set; }
        string Say();
        int DoSomething(int a, int b);
    }

    internal class TestExample
    {
    }
}
