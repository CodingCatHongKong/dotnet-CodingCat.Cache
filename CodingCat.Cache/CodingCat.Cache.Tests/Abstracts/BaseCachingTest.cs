using CodingCat.Cache.Impls;
using CodingCat.Cache.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace CodingCat.Cache.Tests.Abstracts
{
    [TestClass]
    public abstract class BaseCachingTest<T>
    {
        public KeyBuilder KeyBuilder { get; }

        #region Constructor(s)

        public BaseCachingTest()
        {
            this.KeyBuilder = new KeyBuilder<T>(Constants.USING_KEY_PREFIX);
        }

        #endregion Constructor(s)

        protected void Test_Add_GetOk(
            IKeyBuilder usingKey,
            IStorage storage,
            string expected
        )
        {
            // Act
            storage.Delete(usingKey).Add(usingKey, expected);
            Thread.Sleep(100);
            var actual = storage.Get(usingKey);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        protected void Test_ItemExpired_ReturnNull(
            IKeyBuilder usingKey,
            IStorage storage,
            TimeSpan expiry
        )
        {
            // Act
            storage
                .Delete(usingKey)
                .Add(usingKey, Guid.NewGuid().ToString());
            Thread.Sleep((int)expiry.TotalMilliseconds + 1);
            var actual = storage.Get(usingKey);

            // Assert
            Assert.IsNull(actual);
        }

        protected void Test_ItemDelete_CannotGet(
            IKeyBuilder usingKey,
            IStorage storage
        )
        {
            // Act
            var actual = storage
                .Delete(usingKey)
                .Add(usingKey, Guid.NewGuid().ToString())
                .Delete(usingKey)
                .Get(usingKey);

            // Assert
            Assert.IsNull(actual);
        }

        protected void Test_GetOrAdd_Success(
            IKeyBuilder usingKey,
            IStorage storage,
            string expected
        )
        {
            // Act
            var actual = storage
                .Delete(usingKey)
                .Get(usingKey, () => expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        protected void Test_KeyWithSpace_GetOk(
            IStorage storage
        )
        {
            // Arrange
            var usingKey = this.KeyBuilder
                .UseKey(nameof(Test_KeyWithSpace_GetOk))
                .AddSegment("Key with space");
            var expected = Guid.NewGuid().ToString();

            // Act
            storage.Delete(usingKey).Add(usingKey, expected);
            Thread.Sleep(100);
            var actual = storage.Get(usingKey);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        protected void Test_NullValue_IsByPassed(
            IStorage storage
        )
        {
            // Arrange
            var notExpected = true;
            var usingKey = this.KeyBuilder
                .UseKey(nameof(Test_NullValue_IsByPassed));

            storage.Delete(usingKey)
                .Get(usingKey, () => null);

            // Act
            var actual = notExpected;
            storage.Get(
                usingKey,
                () =>
                {
                    actual = !notExpected;
                    return null;
                }
            );

            // Assert
            Assert.AreNotEqual(notExpected, actual);
        }
    }
}