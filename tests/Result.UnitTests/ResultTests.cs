using Zord.Result;

namespace Result.UnitTests
{
    public class ResultTests
    {
        [Fact]
        public void Should_True_When_Success()
        {
            var success = Zord.Result.Result.Success();
            var error = Zord.Result.Result.Error();

            success.Succeeded.Should().Be(true);
            error.Succeeded.Should().Be(false);
        }

        [Fact]
        public void Should_Return_Correct_Code()
        {
            var success = Zord.Result.Result.Success();
            var error = Zord.Result.Result.Error();
            var badRequest = Zord.Result.Result.BadRequest();
            var unauthorized = Zord.Result.Result.Unauthorized();
            var notFound = Zord.Result.Result.NotFound();

            success.Code.Should().Be(ResultCode.Ok);
            error.Code.Should().Be(ResultCode.Error);
            badRequest.Code.Should().Be(ResultCode.BadRequest);
            unauthorized.Code.Should().Be(ResultCode.Unauthorized);
            notFound.Code.Should().Be(ResultCode.NotFound);
        }

        [Theory]
        [InlineData(ResultCode.Ok, "Success message")]
        [InlineData(ResultCode.Error, "Error message")]
        [InlineData(ResultCode.BadRequest, "BadRequest message")]
        [InlineData(ResultCode.Unauthorized, "Unauthorized message")]
        [InlineData(ResultCode.NotFound, "NotFound message")]
        public void Should_Return_Correct_Result(ResultCode code, string message)
        {
            var result = new Zord.Result.Result
            {
                Code = code,
                Message = message,
            };

            result.Code.Should().Be(code);
            result.Succeeded.Should().Be(code == ResultCode.Ok);
            result.Message.Should().Be(message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Correct_Data(int id)
        {
            var intId = Result<int>.Success(id);
            var stringId = Result<string>.Success($"ID-{id}");

            intId.Data.Should().Be(id);
            stringId.Data.Should().Be($"ID-{id}");
        }
    }
}