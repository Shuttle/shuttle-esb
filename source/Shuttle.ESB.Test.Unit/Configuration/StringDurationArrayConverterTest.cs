using System;
using NUnit.Framework;
using Shuttle.ESB.Core;

namespace Shuttle.ESB.Test.Unit.Configuration
{
    public class StringDurationArrayConverterTest : UnitFixture
    {
        [Test]
        public void Should_be_able_to_convert_all_supported_types()
        {
            var converter = new StringDurationArrayConverter();

            var result = (TimeSpan[])converter.ConvertFrom("120s,30m,4h,5d");

            Assert.AreEqual(4, result.Length);
            
            Assert.AreEqual(TimeSpan.FromSeconds(120), result[0]);
            Assert.AreEqual(TimeSpan.FromMinutes(30), result[1]);
            Assert.AreEqual(TimeSpan.FromHours(4), result[2]);
            Assert.AreEqual(TimeSpan.FromDays(5), result[3]);
        }

        [Test]
        public void Should_be_able_to_convert_repeated_types()
        {
            var converter = new StringDurationArrayConverter();

            var result = (TimeSpan[])converter.ConvertFrom("5ms*6,2s*5,1m*4,1h*3,1d*2");

            Assert.AreEqual(20, result.Length);
            
            Assert.AreEqual(TimeSpan.FromMilliseconds(5), result[0]);
            Assert.AreEqual(TimeSpan.FromMilliseconds(5), result[5]);
            Assert.AreEqual(TimeSpan.FromSeconds(2), result[6]);
            Assert.AreEqual(TimeSpan.FromSeconds(2), result[10]);
            Assert.AreEqual(TimeSpan.FromMinutes(1), result[11]);
            Assert.AreEqual(TimeSpan.FromMinutes(1), result[14]);
            Assert.AreEqual(TimeSpan.FromHours(1), result[15]);
            Assert.AreEqual(TimeSpan.FromHours(1), result[17]);
            Assert.AreEqual(TimeSpan.FromDays(1), result[18]);
            Assert.AreEqual(TimeSpan.FromDays(1), result[19]);
        }

        [Test]
        public void Should_be_able_to_convert_type_with_and_without_repeat_count()
        {
            var converter = new StringDurationArrayConverter();

            var result = (TimeSpan[])converter.ConvertFrom("5ms*5,2s,1m*5,1h");

            Assert.AreEqual(12, result.Length);
            
            Assert.AreEqual(TimeSpan.FromMilliseconds(5), result[0]);
            Assert.AreEqual(TimeSpan.FromMilliseconds(5), result[4]);
            Assert.AreEqual(TimeSpan.FromSeconds(2), result[5]);
            Assert.AreEqual(TimeSpan.FromMinutes(1), result[6]);
            Assert.AreEqual(TimeSpan.FromMinutes(1), result[10]);
            Assert.AreEqual(TimeSpan.FromHours(1), result[11]);
        }

        [Test]
        public void Should_be_able_to_parse_defaultDurationToSleepWhenIdle()
        {
            var result = (TimeSpan[])new StringDurationArrayConverter()
                    .ConvertFrom("5ms*100,10ms*100,50ms*50,100ms*50,250ms*20,500ms*10,1s*30,2s*60,5s");
        }
    }
}