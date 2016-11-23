using System;
using System.Text;
using NUnit.Framework;

namespace HybridDb.Superiolizer.Tests
{
    [TestFixture]
    public class TestCustomSerialization
    {
        [Test]
        public void CanRoundtripCustomSerializedDate()
        {
            var configuration = new Configuration(Encoding.UTF8);

            Func<Date, string> customDateSerializerFunction = date => $"{date.Year:0000}/{date.Month:00}/{date.Day:00}";

            configuration.AddCustomSerialization(customDateSerializerFunction,
                str =>
                {
                    var parts = str.Split('/');
                    return new Date(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                });

            var superiolizer = new Superiolizer(configuration);

            var properDateValueType = new Date(2016, 11, 22);
            var classWithDates = new ClassWithDates(properDateValueType);

            string jsonText;

            var roundtrippedClassWithDates = superiolizer.Roundtrip(classWithDates, out jsonText);

            Console.WriteLine(jsonText);

            var expectedDateLayout = customDateSerializerFunction(properDateValueType);
            Assert.That(jsonText, Contains.Substring(expectedDateLayout));
            Assert.That(roundtrippedClassWithDates.Date, Is.EqualTo(properDateValueType));
        }

        class ClassWithDates
        {
            public ClassWithDates(Date date, Date nullDate = null)
            {
                Date = date;
                NullDate = nullDate;
            }

            public Date Date { get; }
            public Date NullDate { get; }
        }

        class Date : IEquatable<Date>
        {
            public int Year { get; }
            public int Month { get; }
            public int Day { get; }

            public Date(int year, int month, int day)
            {
                Year = year;
                Month = month;
                Day = day;
            }

            public bool Equals(Date other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Year == other.Year && Month == other.Month && Day == other.Day;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Date)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Year;
                    hashCode = (hashCode * 397) ^ Month;
                    hashCode = (hashCode * 397) ^ Day;
                    return hashCode;
                }
            }

            public static bool operator ==(Date left, Date right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Date left, Date right)
            {
                return !Equals(left, right);
            }
        }
    }
}