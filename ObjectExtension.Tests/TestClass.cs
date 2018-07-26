using System;
using System.Reflection;
using NUnit.Framework;
using Shouldly;

namespace ObjectExtension.Tests
{
    [TestFixture]
    public class ObjectExtensionTests
    {
        [Test]
        public void Compare_DateTime_value_20180101_and_20180101_should_be_true()
        {
            var actual = new DateTime(2018, 1, 1).Compare(new DateTime(2018, 1, 1));

            actual.ShouldBeTrue();
        }

        [Test]
        public void Compare_DateTime_value_20180101_and_20180102_should_be_False()
        {
            var actual = new DateTime(2018, 1, 1).Compare(new DateTime(2018, 1, 2));

            actual.ShouldBeFalse();
        }

        [Test]
        public void Compare_int_value_1_and_1_should_be_true()
        {
            var actual = 1.Compare(1);

            actual.ShouldBeTrue();
        }


        [Test]
        public void Compare_MyObject_Name_value_Fish_and_Fish_should_be_true()
        {
            var self = new MyObject
            {
                Name = "Fish"
            };

            var to = new MyObject
            {
                Name = "Fish"
            };

            var actual = self.Compare(to);

            actual.ShouldBeTrue();
        }

        [Test]
        public void Compare_string_value_abc_and_abc_should_be_true()
        {
            var actual = "abc".Compare("abc");

            actual.ShouldBeTrue();
        }


        [Test]
        public void Compare_string_value_abc_and_def_should_be_false()
        {
            var actual = "abc".Compare("def");

            actual.ShouldBeFalse();
        }
    }

    public class MyObject
    {
        public string Name { get; set; }
    }

    public static class ObjectExtension
    {
        public static bool Compare<T>(this T self, T to)
        {
            var type = typeof(T);

            switch (type.BaseType.FullName)
            {
                case "System.ValueType":
                    if (self.Equals(to)) return true;
                    break;
                default:
                    if (type.FullName == "System.String")
                    {
                        if (self.Equals(to)) return true;
                        return false;
                    }


                    foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        var selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                        var toValue = type.GetProperty(pi.Name).GetValue(to, null);

                        if (selfValue == toValue && selfValue.Equals(toValue)) return true;
                    }

                    break;
            }


            return false;
        }
    }
}