using System;
using System.Text;
using NUnit.Framework;

namespace HybridDb.Superiolizer.Tests
{
    [TestFixture]
    public class TestCustomNaming
    {
        [Test]
        public void CanUseCustomNames()
        {
            var configuration = new Configuration(Encoding.UTF8)
                .WithShortName<InnerClass.InnerInnerClass.ThisIsTheClass>("ThisIsTheClass");

            var superiolizer = new Superiolizer(configuration);

            var instanceWithSillyName = new InnerClass.InnerInnerClass.ThisIsTheClass {Value = 23};

            string jsonText;
            var roundtrippedInstanceWithSillyName = superiolizer.Roundtrip(instanceWithSillyName, out jsonText);

            Console.WriteLine(jsonText);

            Assert.That(roundtrippedInstanceWithSillyName.Value, Is.EqualTo(23));
            Assert.That(jsonText, Does.Not.Contain("InnerClass+InnerInnerClass+ThisIsTheClass"));
            Assert.That(jsonText, Does.Not.Contain("HybridDb.Superiolizer.Tests"));
        }

        public class InnerClass
        {
            public class InnerInnerClass
            {
                public class ThisIsTheClass
                {
                    public int Value { get; set; }
                }
            }
        }
    }
}