using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ScalableWeb.Domain.Models;
using ScalableWeb.Domain.Patterns;
using ScalableWeb.Domain.UseCases.CompareData;
using Xunit;

namespace ScalableWeb.Domain.Test.UnitTests.UseCases
{
    public class CompareDataCommandTest
    {
        private readonly Mock<IRepository<DataRecord>> _mockRepository;

        public CompareDataCommandTest()
        {
            _mockRepository = new Mock<IRepository<DataRecord>>();
        }

        [Fact]
        public async Task CompareDataThatNotExist()
        {
            _mockRepository.Setup(r => r.Queriable()).Returns(EmptyData());

            var response = await new CompareDataCommand(_mockRepository.Object)
                .Handle(new CompareDataRequest {DiffId = 1}, CancellationToken.None);

            response.AreEqual.Should().BeFalse();
            response.Message.Should().Be("No data found for ID 1.");
        }

        [Fact]
        public async Task CompareTwoDataWithDifferentSize()
        {
            _mockRepository.Setup(r => r.Queriable()).Returns(DataWithDifferentSize());

            var response = await new CompareDataCommand(_mockRepository.Object)
                .Handle(new CompareDataRequest {DiffId = 1}, CancellationToken.None);

            response.AreEqual.Should().BeFalse();
            response.Message.Should().Be("Size difference! Left: 3; Right: 2");
        }

        [Fact]
        public async Task CompareEqualData()
        {
            _mockRepository.Setup(r => r.Queriable()).Returns(EqualData());

            var response = await new CompareDataCommand(_mockRepository.Object)
                .Handle(new CompareDataRequest {DiffId = 2}, CancellationToken.None);

            response.AreEqual.Should().BeTrue();
            response.Message.Should().Be("They are equal!");
        }

        [Fact]
        public async Task CompareEqualSizeDifferentData()
        {
            _mockRepository.Setup(r => r.Queriable()).Returns(DataWithDifferentContent());

            var response = await new CompareDataCommand(_mockRepository.Object)
                .Handle(new CompareDataRequest {DiffId = 3}, CancellationToken.None);

            response.AreEqual.Should().BeFalse();
            response.Message.Should().Be("They are different from the index 2.");
        }

        private static IQueryable<DataRecord> DataWithDifferentContent()
        {
            return new List<DataRecord>
            {
                new DataRecord{ Side = DataSide.Left, DiffId = 3,  Data = new byte[] { 1, 2, 3, 4 }},
                new DataRecord{ Side = DataSide.Right, DiffId = 3, Data = new byte[] { 1, 2, 4, 5 }},
            }.AsQueryable();
        }

        private static IQueryable<DataRecord> EqualData()
        {
            return new List<DataRecord>
            {
                new DataRecord{ Side = DataSide.Left, DiffId = 2, Data = new byte[] { 1, 2 }},
                new DataRecord{ Side = DataSide.Right, DiffId = 2, Data = new byte[] { 1, 2 }},
            }.AsQueryable();
        }

        private static IQueryable<DataRecord> DataWithDifferentSize()
        {
            return new List<DataRecord>
            {
                new DataRecord{ Side = DataSide.Left, DiffId = 1, Data = new byte[] { 1, 2, 3 }},
                new DataRecord{ Side = DataSide.Right, DiffId = 1, Data = new byte[] { 1, 2 }},
            }.AsQueryable();
        }

        private static IQueryable<DataRecord> EmptyData()
        {
            return new List<DataRecord>().AsQueryable();
        }

    }
}
