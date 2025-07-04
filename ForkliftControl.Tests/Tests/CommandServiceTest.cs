using FluentAssertions;
using ForkliftControlSystem.Application.DTOs;
using ForkliftControlSystem.Application.Interfaces;
using ForkliftControlSystem.Infrastructure.Services;

namespace ForkliftControl.Tests.Tests
{
    public class CommandServiceTest
    {
        private readonly CommandService _service = new();

        [Fact]
        public void ParseCommand_ShouldReturnCorrectStepCount()
        {
            // Arrange
            var command = "F10R90B10L90";

            // Act
            var result = _service.ParseCommand(command);

            // Assert
            result.Should().HaveCount(4);
            result[0].Should().Contain("Forward");
            result[1].Should().Contain("Right");
            result[2].Should().Contain("Backward");
            result[3].Should().Contain("Left");
        }

        [Theory]
        [InlineData("F10", 1, new[] { "Forward" })]
        [InlineData("F10R90", 2, new[] { "Forward", "Right" })]
        [InlineData("F10R90L90", 3, new[] { "Forward", "Right", "Left" })]
        [InlineData("F10R90B5L90", 4, new[] { "Forward", "Right", "Backward", "Left" })]
        public void ParseCommand_ShouldReturnExpectedSteps(string command, int expectedCount, string[] expectedKeywords)
        {
            // Act
            List<string> result = _service.ParseCommand(command);

            // Assert
            result.Should().HaveCount(expectedCount);
            for (int i = 0; i < expectedKeywords.Length; i++)
            {
                result[i].Contains(expectedKeywords[i]).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData("F2", 0, 2, "North")]
        [InlineData("F1R90F2", 2, 1, "East")]
        [InlineData("F1L90F1", -1, 1, "West")]
        [InlineData("F1R180F1", 0, 0, "South")]
        public void ExecuteCommand_ShouldReturnCorrectFinalPosition(string command, int expectedX, int expectedY, string expectedDirection)
        {
            // Act
            CommandResult result = _service.ExecuteCommand(command);

            // Assert
            result.FinalX.Should().Be(expectedX);
            result.FinalY.Should().Be(expectedY);
            result.FinalOrientation.Should().Be(expectedDirection);
        }

        [Theory]
        [InlineData("F")]     // No number
        [InlineData("L91")]   // Invalid degree (not multiple of 90)
        [InlineData("R380")]   // Invalid degree (over 360)
        public void ParseCommand_ShouldThrowException_ForInvalidInput(string invalidCommand)
        {
            // Act
            Action act = () => _service.ParseCommand(invalidCommand);

            // Assert
            act.Should().Throw<Exception>().WithMessage("*invalid*", "*unknown*", "*degree*");
        }

        [Fact]
        public void ExecuteCommandWithObstacles_ShouldStopAtObstacle()
        {
            // Arrange
            string command = "F3";
            var obstacles = new List<ObstacleDto> { new ObstacleDto { X = 0, Y = 2 } };

            // Act
            CommandResult result = _service.ExecuteCommandWithObstacles(command, obstacles);

            // Assert
            result.FinalX.Should().Be(0);
            result.FinalY.Should().Be(1);
            result.FinalOrientation.Should().Be("North");
            result.Actions.Should().Contain(a => a.Contains("Obstacle"));
        }

        [Fact]
        public void ExecuteCommandWithObstacles_ShouldPassWithoutHittingAnyObstacle()
        {
            // Arrange
            string command = "F2R90F2"; // (0,0) → (0,2) → turn east → (2,2)
            var obstacles = new List<ObstacleDto>
            {
                new ObstacleDto { X = 1, Y = 0 },
                new ObstacleDto { X = 3, Y = 2 },
                new ObstacleDto { X = 0, Y = 5 }
            };

            // Act
            CommandResult result = _service.ExecuteCommandWithObstacles(command, obstacles);

            // Assert
            result.FinalX.Should().Be(2);
            result.FinalY.Should().Be(2);
            result.FinalOrientation.Should().Be("East");
            result.Actions.Should().NotContain(a => a.Contains("Obstacle"));
        }
    }
}
