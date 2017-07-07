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

        [TestCase("N","S")]
        [TestCase("NE", "SE")]
        [TestCase("NW", "SW")]
        [TestCase("E", "W")]        
        public void TurnStringCreationTest(string move, string build)
        {
            var turn = new Turn(move, build);

            Assert.That(turn.MoveDirection, Is.EqualTo(Enum.Parse(typeof(Direction), move)));
            Assert.That(turn.BuildDirection, Is.EqualTo(Enum.Parse(typeof(Direction), build)));
        }

        [Test]
        public void ToStringTest()
        {
            var turn = new Turn(Direction.N, Direction.S).ToString();

            Assert.That(turn, Is.EqualTo("N S"));
        }
    }
}
