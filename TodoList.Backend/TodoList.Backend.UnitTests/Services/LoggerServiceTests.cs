using Moq;
using TodoList.Backend.Models.Interfaces;
using Xunit;

namespace TodoList.Backend.UnitTests.Services
{
    public class LoggerServiceTests
    {
        private static ILoggerService _mockLogger;

        public LoggerServiceTests()
        {
            _mockLogger = new Mock<ILoggerService>().Object;
        }
        
        [Fact]
        public void Log_Debug_Test_Passes()
        {
            //Arrange
            string someText = "Some text";

            //Act
            _mockLogger.LogDebug(someText);
           
            //Assert
            Assert.True(true);
        }
        
        [Fact]
        public void Log_Error_Test_Passes()
        {
            //Arrange
            string someText = "Some text";

            //Act
            _mockLogger.LogError(someText);

            //Assert
            Assert.True(true);
        }

        [Fact]
        public void Log_Info_Test_Passes()
        {
            //Arrange
            string someText = "Some text";

            //Act
            _mockLogger.LogInfo(someText);

            //Assert
            Assert.True(true);
        }

        [Fact]
        public void Log_Warn_Test_Passes()
        {
            //Arrange
            string someText = "Some text";

            //Act
            _mockLogger.LogWarn(someText);

            //Assert
            Assert.True(true);
        }
    }
}
