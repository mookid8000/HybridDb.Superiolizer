using System.Text;
using NUnit.Framework;

namespace HybridDb.Superiolizer.Tests
{
    [TestFixture]
    public class TestSuperiolizer
    {
        [Test]
        public void CanRoundtripSomething()
        {
            var superiolizer = new Superiolizer(Encoding.UTF8);
            var something = new Something("hej med dig");

            var roundtrippedSomething = superiolizer.Roundtrip(something);

            Assert.That(roundtrippedSomething.Text, Is.EqualTo("hej med dig"));
        }

        class Something
        {
            public Something(string text)
            {
                Text = text;
            }

            public string Text { get; }
        }
    }
}
