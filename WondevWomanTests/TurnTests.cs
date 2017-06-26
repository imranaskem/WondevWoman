using System;
using NUnit.Framework;

namespace WondevWomanTests
{
    [TestFixture]
    public class TurnTests
    {
        [Test]
        public void TurnCreationTest()
        {
            var turn = new Turn(Direction.N, Direction.S);

            Assert.That(turn.MoveDirection, Is.EqualTo(Direction.N));
            Assert.That(turn.BuildDirection, Is.EqualTo(Direction.S));
        }

        [Test]
        public void ToStringTest()
        {
            var turn = new Turn(Direction.N, Direction.S).ToString();

            Assert.That(turn, Is.EqualTo("N S"));
        }
    }
}
