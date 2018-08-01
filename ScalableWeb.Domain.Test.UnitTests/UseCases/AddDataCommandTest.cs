using System.Threading;
using FluentAssertions;
using Moq;
using ScalableWeb.Domain.Models;
using ScalableWeb.Domain.Patterns;
using ScalableWeb.Domain.UseCases;
using ScalableWeb.Domain.UseCases.AddData;
using Xunit;

namespace ScalableWeb.Domain.Test.UnitTests.UseCases
{
    public class AddDataCommandTest
    {
        private readonly Mock<IRepository<DataRecord>> _mockRepository;

        public AddDataCommandTest()
        {
            _mockRepository = new Mock<IRepository<DataRecord>>();
        }

        [Fact]
        public void AddLeftDataTest()
        {
            DataRecord record = null;
            _mockRepository.Setup(r => r.Insert(It.IsAny<DataRecord>())).Callback<DataRecord>(r => record = r);

            var data = new byte[] { 1, 2, 3 };
            var response = new AddDataCommand(_mockRepository.Object)
                .Handle(new AddDataRequest{ DiffId = 1, Data = data}, CancellationToken.None)
                .Result;

            response.Success.Should().BeTrue();
            record.Side.Should().Be(DataSide.Left);
            record.DiffId.Should().Be(1);
            record.Data.Should().BeSameAs(data);
        }

        [Fact]
        public void AddRightDataTest()
        {
            DataRecord record = null;
            _mockRepository.Setup(r => r.Insert(It.IsAny<DataRecord>())).Callback<DataRecord>(r => record = r);

            var data = new byte[] { 1, 2, 3 };
            var response = new AddDataCommand(_mockRepository.Object)
                .Handle(new AddDataRequest { DiffId = 2, Data = data, Side = DataSide.Right }, CancellationToken.None)
                .Result;

            response.Success.Should().BeTrue();
            record.Side.Should().Be(DataSide.Right);
            record.DiffId.Should().Be(2);
            record.Data.Should().BeSameAs(data);
        }

        [Fact]
        public void AddInvalidDataTest()
        {
            _mockRepository.Verify(r => r.Insert(It.IsAny<DataRecord>()), Times.Never);

            var response = new AddDataCommand(_mockRepository.Object)
                .Handle(new AddDataRequest { Data = null }, CancellationToken.None)
                .Result;

            response.Success.Should().BeFalse();
            response.ErrorMessage.Should().Be("The Data field is required.");
        }
    }
}
