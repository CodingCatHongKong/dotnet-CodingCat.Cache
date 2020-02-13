using CodingCat.Cache.Memory;
using CodingCat.Cache.Tests.Abstracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CodingCat.Cache.Tests
{
    [TestClass]
    public class UnitTestMemoryCache : BaseCachingTest<UnitTestMemoryCache>
    {
        [TestMethod]
        public void Test_Add_GetOk()
        {
            this.Test_Add_GetOk(
                this.KeyBuilder.UseKey(nameof(Test_Add_GetOk)),
                new Storage(TimeSpan.FromDays(1)),
                expected: nameof(UnitTestMemoryCache)
            );
        }

        [TestMethod]
        public void Test_ItemExpired_ReturnNull()
        {
            var expiry = TimeSpan.FromMilliseconds(100);

            this.Test_ItemExpired_ReturnNull(
                this.KeyBuilder.UseKey(nameof(Test_ItemExpired_ReturnNull)),
                new Storage(expiry),
                expiry
            );
        }

        [TestMethod]
        public void Test_ItemDelete_CannotGet()
        {
            this.Test_ItemDelete_CannotGet(
                this.KeyBuilder.UseKey(nameof(Test_ItemDelete_CannotGet)),
                new Storage(TimeSpan.FromDays(1))
            );
        }

        [TestMethod]
        public void Test_GetOrAdd_Success()
        {
            this.Test_GetOrAdd_Success(
                this.KeyBuilder.UseKey(nameof(Test_GetOrAdd_Success)),
                new Storage(TimeSpan.FromDays(1)),
                Guid.NewGuid().ToString()
            );
        }

        [TestMethod]
        public void Test_KeyWithSpace_GetOk()
        {
            this.Test_KeyWithSpace_GetOk(
                new Storage(TimeSpan.FromDays(1))
            );
        }
    }
}