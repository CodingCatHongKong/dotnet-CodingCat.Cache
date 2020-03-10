using CodingCat.Cache.Impls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CodingCat.Cache.Tests
{
    [TestClass]
    public class UnitTestKeyBuilder
    {
        public static readonly Type UsingType = typeof(UnitTestKeyBuilder);

        public KeyBuilder KeyBuilder { get; }

        #region Constructor(s)

        public UnitTestKeyBuilder()
        {
            this.KeyBuilder = new KeyBuilder<UnitTestKeyBuilder>(
                Constants.USING_KEY_PREFIX
            );
        }

        #endregion Constructor(s)

        [TestMethod]
        public void Test_InitKeyBuilder_Ok()
        {
            // Arrange
            var expectedPrefix = string.Join(
                "-",
                UsingType.FullName,
                Constants.USING_KEY_PREFIX
            );

            // Act
            var actual = new KeyBuilder(
                UsingType,
                Constants.USING_KEY_PREFIX
            );

            // Assert
            Assert.AreEqual(expectedPrefix, actual.KeyPrefix);
        }

        [TestMethod]
        public void Test_InitKeyBuilder_WithConfig_Ok()
        {
            // Arrange
            var expectedPrefix = string.Join(
                "-",
                UsingType.FullName,
                Constants.USING_KEY_PREFIX
            );

            // Act
            var actual = new KeyBuilder(
                new KeyBuilderConfiguration()
                {
                    UsingType = UsingType,
                    KeyPrefix = Constants.USING_KEY_PREFIX
                }
            );

            // Assert
            Assert.AreEqual(expectedPrefix, actual.KeyPrefix);
        }

        [TestMethod]
        public void Test_InitKeyBuilderWithGenericType_Ok()
        {
            // Arrange
            var expectedPrefix = string.Join(
                "-",
                UsingType.FullName,
                Constants.USING_KEY_PREFIX
            );

            // Act
            var actual = new KeyBuilder<UnitTestKeyBuilder>(
                Constants.USING_KEY_PREFIX
            );

            // Assert
            Assert.AreEqual(expectedPrefix, actual.KeyPrefix);
        }

        [TestMethod]
        public void Test_CallToStringWithoutUsingKey_ExpectedInvalidOperationException()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsException<InvalidOperationException>(
                this.KeyBuilder.ToString
            );
        }

        [TestMethod]
        public void Test_CallToStringWithEmptyUsingKey_ExpectedInvalidOperationException()
        {
            // Arrange

            // Act
            var actual = this.KeyBuilder.UseKey("");

            // Assert
            Assert.ThrowsException<InvalidOperationException>(actual.ToString);
        }

        [TestMethod]
        public void Test_SetUsingKeyGetNewInstance_Ok()
        {
            const string USING_KEY = nameof(Test_SetUsingKeyGetNewInstance_Ok);

            // Arrange
            var expectedPrefix = string.Join(
                "-",
                UsingType.FullName,
                Constants.USING_KEY_PREFIX
            );

            // Act
            var actual = this.KeyBuilder.UseKey(USING_KEY);

            // Assert
            Assert.AreEqual(expectedPrefix, actual.KeyPrefix);
            Assert.AreEqual(USING_KEY, actual.UsingKey);
            Assert.AreNotEqual(this.KeyBuilder.GetHashCode(), actual.GetHashCode());
        }

        [TestMethod]
        public void Test_AddSegments_Expected()
        {
            const string USING_KEY = nameof(Test_AddSegments_Expected);

            // Arrange
            var segments = new object[] { Guid.NewGuid(), Guid.NewGuid() };
            var expected = string.Join(
                "-",
                UsingType.FullName,
                Constants.USING_KEY_PREFIX,
                USING_KEY,
                segments[0].ToString(),
                segments[1].ToString()
            );

            // Act
            var actual = this.KeyBuilder
                .UseKey(USING_KEY)
                .AddSegments(segments)
                .ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_AddSegmentsByParams_Expected()
        {
            const string USING_KEY = nameof(Test_AddSegmentsByParams_Expected);

            // Arrange
            var segments = new object[] { Guid.NewGuid(), Guid.NewGuid() };
            var expected = string.Join(
                "-",
                UsingType.FullName,
                Constants.USING_KEY_PREFIX,
                USING_KEY,
                segments[0].ToString(),
                segments[1].ToString()
            );

            // Act
            var actual = this.KeyBuilder
                .UseKey(USING_KEY)
                .AddSegments(segments[0].ToString(), segments[1].ToString())
                .ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_AddSegment_Expected()
        {
            const string USING_KEY = nameof(Test_AddSegment_Expected);

            // Arrange
            var segment = Guid.NewGuid();
            var expected = string.Join(
                "-",
                UsingType.FullName,
                Constants.USING_KEY_PREFIX,
                USING_KEY,
                segment.ToString()
            );

            // Act
            var actual = this.KeyBuilder
                .UseKey(USING_KEY)
                .AddSegment(segment)
                .ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_NoSegment_Expected()
        {
            const string USING_KEY = nameof(Test_NoSegment_Expected);

            // Arrange
            var expected = string.Join(
                "-",
                UsingType.FullName,
                Constants.USING_KEY_PREFIX,
                USING_KEY
            );

            // Act
            var actual = this.KeyBuilder
                .UseKey(USING_KEY)
                .ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}