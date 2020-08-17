using System;
using Toolkit.Syncable;
using Xunit;

namespace Tests.Syncable
{
    public class ResetActionTests
    {
        private class TestClass
        {
            public string PropertyA { get; set; }
            public int PropertyB { get; }
            public string PropertyC { get; private set; }
            private double PropertyD { get; set; }
        }

        [Fact]
        void TestContructor()
        {
            // 1. Test handling of null inputs
            Assert.Throws<ArgumentNullException>(() => new DefaultResetAction<string>(null, string.Empty, null));
            Assert.Throws<ArgumentNullException>(() => new DefaultResetAction<string>(new TestClass(), null, null));

            // 2. Test handling of missing property
            Assert.Throws<ArgumentException>(() => new DefaultResetAction<string>(new TestClass(), "PropertyE", null));

            // 3. Test handling of non accessible property
            Assert.Throws<ArgumentException>(() => new DefaultResetAction<string>(new TestClass(), "PropertyD", null));

            // 4. Test handling of properties with private setter
            Assert.Throws<ArgumentException>(() => new DefaultResetAction<string>(new TestClass(), "PropertyC", null));

            // 5. Test handling of properties with no setter
            Assert.Throws<ArgumentException>(() => new DefaultResetAction<string>(new TestClass(), "PropertyB", null));

            // 6. Test handling of properties with public setters
            new DefaultResetAction<string>(new TestClass(), "PropertyA", null);
        }

        [Fact]
        void TestUndo()
        {
            var testClass = new TestClass
            {
                PropertyA = "MyValue"
            };

            var resetAction = new DefaultResetAction<string>(testClass, nameof(testClass.PropertyA), "MyValue");

            testClass.PropertyA = "testing";
            Assert.Equal("testing", testClass.PropertyA);

            resetAction.ResetProperty();
            Assert.Equal("MyValue", testClass.PropertyA);
        }
    }
}
