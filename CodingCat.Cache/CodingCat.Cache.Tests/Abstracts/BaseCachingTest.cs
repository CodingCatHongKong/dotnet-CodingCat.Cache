using CodingCat.Cache.Impls;
using CodingCat.Cache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        #endregion

        protected void Test_Add_GetOk(
            IKeyBuilder usingKey,
            IStorage storage,
            string expected
        )
        {
            // Act
            storage.Delete(usingKey).Add(usingKey, expected);
            Thread.Sleep(500);
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
    }
}
