using CodingCat.Cache.Impls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CodingCat.Cache.Tests
{
    [TestClass]
    public class UnitTestKeyBuilder
    {
        public static readonly Type UsingType = typeof(UnitTestKeyBuilder);

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
            var actual = new KeyBuilder(UsingType, Constants.USING_KEY_PREFIX);

            // Assert
            Assert.ThrowsException<InvalidOperationException>(actual.ToString);
        }

        [TestMethod]
        public void Test_CallToStringWithEmptyUsingKey_ExpectedInvalidOperationException()
        {
            // Arrange

            // Act
            var actual = new KeyBuilder(UsingType, Constants.USING_KEY_PREFIX)
                .UseKey("");

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
            var builder = new KeyBuilder<UnitTestKeyBuilder>(
                Constants.USING_KEY_PREFIX
            );

            // Act
            var actual = builder.UseKey(USING_KEY);

            // Assert
            Assert.AreEqual(expectedPrefix, actual.KeyPrefix);
            Assert.AreEqual(USING_KEY, actual.UsingKey);
            Assert.AreNotEqual(builder.GetHashCode(), actual.GetHashCode());
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
            var actual = new KeyBuilder<UnitTestKeyBuilder>(
                Constants.USING_KEY_PREFIX
            )
                .UseKey(USING_KEY)
                .AddSegments(segments)
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
            var actual = new KeyBuilder<UnitTestKeyBuilder>(
                Constants.USING_KEY_PREFIX
            )
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
            var actual = new KeyBuilder<UnitTestKeyBuilder>(
                Constants.USING_KEY_PREFIX
            )
                .UseKey(USING_KEY)
                .ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}