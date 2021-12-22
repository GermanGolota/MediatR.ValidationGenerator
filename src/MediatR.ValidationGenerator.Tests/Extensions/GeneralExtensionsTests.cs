using System;
using System.Linq;
using MediatR.ValidationGenerator.Extensions;
using Xunit;

namespace MediatR.ValidationGenerator.Tests.Extensions
{

    public class GeneralExtensionsTests
    {
        private static Func<string, int, string> DefaultFormatter = (name, number) => $"{name}_{number}";
        [Fact]
        public void PreventDuplicateNames_ShouldDoNothing_WhenNoDuplicateNames()
        {
            //Arrange
            var expected = new[] { "a", "b", "c" };
            //Act
            var actual = expected.PreventDuplicateNames(DefaultFormatter);
            //Assert
            Assert.True(expected.SequenceEqual(actual));
        }

        [Fact]
        public void PreventDuplicateNames_ShouldPreventDuplicated_WhenDuplicateNames()
        {
            //Arrange
            var names = new[] { "James", "John", "John", "Boris" };
            var expected = new[] { "James", "John", "John_1", "Boris" };
            //Act
            var actual = names.PreventDuplicateNames(DefaultFormatter);
            //Assert
            Assert.True(expected.SequenceEqual(actual));
        }
    }
}
