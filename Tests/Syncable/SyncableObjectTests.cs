using Toolkit.Syncable;
using Xunit;

namespace Tests.Syncable
{
    public class SyncableObjectTests
    {
        private class TestObject : SyncableObject
        {
            private string _myProperty;
            private int _myOtherProperty;

            public string MyProperty
            {
                get => _myProperty;
                set => SetAndNotify(ref _myProperty, value);
            }

            public int MyOtherProperty
            {
                get => _myOtherProperty;
                set => SetAndNotify(ref _myOtherProperty, value);
            }
        }

        private TestObject _testObject;

        public SyncableObjectTests()
        {
            _testObject = new TestObject();
        }

        [Fact]
        void TestConstructor()
        {
            // 1. IsSynced should be true on construction
            Assert.True(_testObject.IsSynced);

            // 2. Commands should be not null
            Assert.NotNull(_testObject.SyncCommand);
            Assert.NotNull(_testObject.RevertAllCommand);
        }

        [Fact]
        void TestIsModified()
        {
            // 1. Null or empty returns False
            Assert.False(_testObject.IsModified(string.Empty));
            Assert.False(_testObject.IsModified(null));

            // 2. 'MyProperty' is not modified
            Assert.False(_testObject.IsModified(nameof(_testObject.MyProperty)));

            // 3. 'MyProperty' is marked as modified, after being modified
            _testObject.MyProperty = "Sample text";
            Assert.True(_testObject.IsModified(nameof(_testObject.MyProperty)));
        }

        [Fact]
        void TestRevert()
        {
            // 1. Revert with no changes throws no exception
            _testObject.Revert(null);

            // 2. Revert with a non existing property throws no exception
            _testObject.Revert("SomeProperty");

            // 3. Revert with no previous value will assume
            // the first value set as fallback value.
            _testObject.MyProperty = default;
            _testObject.MyProperty = "This is the new start";

            _testObject.Revert(nameof(_testObject.MyProperty));
            Assert.Equal("This is the new start", _testObject.MyProperty);

            // 4. Revert with a previous value will revert to it
            _testObject.MyProperty = "Something else";
            _testObject.Revert(nameof(_testObject.MyProperty));
            Assert.Equal("This is the new start", _testObject.MyProperty);
        }

        [Fact]
        void TestRevertAll()
        {
            // 1. RevertAll with no changed throws no exception
            _testObject.RevertAll();

            // 2. Changing multiple properties and then reverting works
            _testObject.MyProperty = "Test123";
            Assert.Equal("Test123", _testObject.MyProperty);
            _testObject.MyOtherProperty = 42;
            Assert.Equal(42, _testObject.MyOtherProperty);

            _testObject.MyProperty = "321String";
            Assert.Equal("321String", _testObject.MyProperty);
            _testObject.MyOtherProperty = 12;
            Assert.Equal(12, _testObject.MyOtherProperty);

            _testObject.RevertAll();

            _testObject.MyProperty = "Test123";
            Assert.Equal("Test123", _testObject.MyProperty);
            _testObject.MyOtherProperty = 42;
            Assert.Equal(42, _testObject.MyOtherProperty);
        }
    }
}
