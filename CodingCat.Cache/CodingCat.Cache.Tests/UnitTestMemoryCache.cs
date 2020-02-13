using System;
using CodingCat.Cache.Impls;
using CodingCat.Cache.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodingCat.Cache.Tests
{
    [TestClass]
    public class UnitTestMemoryCache
    {
        public KeyBuilder KeyBuilder { get; }

        #region Constructor(s)
        public UnitTestMemoryCache()
        {
            this.KeyBuilder = new KeyBuilder<UnitTestMemoryCache>(
                Constants.USING_KEY_PREFIX
            );
        }
        #endregion

        [TestMethod]
        public void Test_Add_GetOk()
        {
            // Arrange
            var usingKey = this.KeyBuilder
                .UseKey(nameof(Test_Add_GetOk));
            var storage = new Storage(TimeSpan.FromDays(1));
            var expected = nameof(UnitTestMemoryCache);

            // Act
            storage.Add(usingKey, expected);
            System.Threading.Thread.Sleep(500);
            var actual = storage.Get(usingKey);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_ItemExpired_ReturnNull()
        {
            const int EXPIRY_IN_MILLISECONDS = 100;

            // Arrange
            var usingKey = this.KeyBuilder
                .UseKey(nameof(Test_ItemExpired_ReturnNull));
            var storage = new Storage(
                TimeSpan.FromMilliseconds(EXPIRY_IN_MILLISECONDS)
            ).Add(usingKey, Guid.NewGuid().ToString());

            // Act
            System.Threading.Thread.Sleep(EXPIRY_IN_MILLISECONDS + 1);
            var actual = storage.Get(usingKey);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Test_ItemDelete_CannotGet()
        {
            // Arrange
            var usingKey = this.KeyBuilder
                .UseKey(nameof(Test_ItemDelete_CannotGet));
            var storage = new Storage(TimeSpan.FromDays(1))
                .Add(usingKey, Guid.NewGuid().ToString());

            // Act
            var actual = storage.Delete(usingKey).Get(usingKey);

            // Assert
            Assert.IsNull(actual);
        }
    }
}
