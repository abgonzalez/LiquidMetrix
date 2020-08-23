using Microsoft.Extensions.Logging;
using Moq;
using Xunit;


namespace LiquidMetrix
{
    public class When_Moving_Rover
    {
        #region Initialize
        public Mock<ILogger<Rover>> _loggerMock;
        public Mock<IGrid> _gridMock;
        public Rover rover;
        public When_Moving_Rover()
        {
            _loggerMock = new Mock<ILogger<Rover>>();
            _gridMock = new Mock<IGrid>();
            _gridMock.Setup(x => x._width).Returns(40);
            _gridMock.Setup(x => x._height).Returns(30);
            rover = new Rover(_loggerMock.Object, _gridMock.Object);

        }
        #endregion

        #region TestCases
        [Fact]
        public void Should_SetInitialPosition()
        {
            Rover expected = new Rover(_loggerMock.Object, _gridMock.Object);
            Assert.Equal(expected, rover);
        }
        [Fact]
        public void Should_MoveCorrect()
        {
            rover.SetPosition();
            rover.Move("L0");
            Rover expected = new Rover(_loggerMock.Object, _gridMock.Object);
            expected.SetPosition(0, 0, 'W');
            Assert.Equal(expected, rover);

            rover.SetPosition();
            rover.Move("R9L0R9L0R9L0");
            expected.SetPosition(27, 0, 'N');
            Assert.Equal(expected, rover);

            rover.SetPosition();
            rover.Move("L0R5");
            expected.SetPosition(0, 5, 'N');
            Assert.Equal(expected, rover);

            rover.SetPosition();
            rover.Move("R9L9R5R4");
            expected.SetPosition(14, 5, 'S');
            Assert.Equal(expected, rover);

            rover.SetPosition();
            rover.Move("R2R0R0R0");
            expected.SetPosition(2, 0, 'N');
            Assert.Equal(expected, rover);

            rover.SetPosition();
            rover.Move("R5L1");
            expected.SetPosition(5, 1, 'N');
            Assert.Equal(expected, rover);

            rover.SetPosition();
            rover.Move("R5L3R9R3");
            expected.SetPosition(14, 0, 'S');
            Assert.Equal(expected, rover);

            rover.SetPosition();
            rover.Move("R9L0R9L0R9L0R9L0");
            expected.SetPosition(36, 0, 'N');
            Assert.Equal(expected, rover);

            rover.SetPosition();
            rover.Move("R0L9R0L9R0L9R0");
            expected.SetPosition(0, 27, 'E');
            Assert.Equal(expected, rover);


        }
        [Fact]
        public void ShouldReturnWrongInputData()
        {


            rover.SetPosition();
            Assert.Equal(StatusCode.InvalidInput, rover.Move("12"));

            rover.SetPosition();
            Assert.Equal(StatusCode.InvalidInput, rover.Move("LL"));

            rover.SetPosition();
            Assert.Equal(StatusCode.InvalidInput, rover.Move("X3"));

            rover.SetPosition();
            Assert.Equal(StatusCode.InvalidInput, rover.Move("!£"));

            rover.SetPosition();
            Assert.Equal(StatusCode.InvalidInput, rover.Move("L-2"));

            rover.SetPosition();
            Assert.Equal(StatusCode.Error, rover.Move("L10"));


            //ex = Assert.Throws<ArgumentException>(() => rover.Move("XCCCCC"));
            //Assert.Equal("Wrong instructions.", ex.Message);
        }

        [Fact]
        public void ShouldReturnOutOfBoundException()
        {
            /// Expected position(-2, 0, 'W'); -> Out of bound(40*30)
            rover.SetPosition();
            Assert.Equal(StatusCode.OutOfBounds, rover.Move("L2R0R0R0R0"));

            /// Expected position(45, 0, 'N'); -> Out of bound(40*30)
            rover.SetPosition();
            Assert.Equal(StatusCode.OutOfBounds, rover.Move("R9L0R9L0R9L0R9L0R9L0"));

            /// Expected position(-2, 3, 'N'); -> Out of bound(40*30)
            rover.SetPosition();
            Assert.Equal(StatusCode.OutOfBounds, rover.Move("L2R3"));

            /// Expected position(1, -4, 'S'); -> Out of bound(40*30)
            rover.SetPosition();
            Assert.Equal(StatusCode.OutOfBounds, rover.Move("R1R4"));


            /// Expected position(-9, 0, 'W'); -> Out of bound(40*30)
            rover.SetPosition();
            Assert.Equal(StatusCode.OutOfBounds, rover.Move("L9L0L9L0L9L0L9L0L9L0L9L0L9L0L9L0L9L0L9L0L9"));

            /// Expected position(45, 0, 'W'); -> Out of bound(40*30)
            rover.SetPosition();
            Assert.Equal(StatusCode.OutOfBounds, rover.Move("R9L0R9L0R9L0R9L0R9L0"));

            /// Expected position(-2, 0, 'W'); -> Out of bound(40*30)
            rover.SetPosition();
            Assert.Equal(StatusCode.OutOfBounds, rover.Move("R0R9"));

            /// Expected position(-2, 0, 'W'); -> Out of bound(40*30)
            rover.SetPosition();
            Assert.Equal(StatusCode.OutOfBounds, rover.Move("R0L9R0L9R0L9R0L9R0L9R0L9R0L9"));

            /// Expected position(45, 0, 'W'); -> Out of bound(40*30)
            rover.SetPosition();
            Assert.Equal(StatusCode.OutOfBounds, rover.Move("R0L9R0L9R0L9R0L9R0L9R0"));

            /// Expected position(-2, 0, 'W'); -> Out of bound(40*30)
            rover.SetPosition();
            Assert.Equal(StatusCode.OutOfBounds, rover.Move("L2R0R0R0R0"));

        }
        #endregion
    }
}


