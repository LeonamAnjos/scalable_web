using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ScalableWeb.Domain.Models;
using ScalableWeb.Domain.Patterns;

namespace ScalableWeb.Domain.UseCases.CompareData
{
    public class CompareDataCommand : IRequestHandler<CompareDataRequest, CompareDataResponse>
    {
        private readonly IRepository<DataRecord> _repository;

        public CompareDataCommand(IRepository<DataRecord> repository)
        {
            _repository = repository;
        }

        public Task<CompareDataResponse> Handle(CompareDataRequest request, CancellationToken cancellationToken)
        {
            var datas = _repository.Queriable().Where(d => d.DiffId == request.DiffId).ToList();
            var left = datas.Find(d => d.Side == DataSide.Left);
            var right = datas.Find(d => d.Side == DataSide.Right);

            if (left == null || right == null)
                return Task.FromResult(new CompareDataResponse
                {
                    Message = $"No data found for ID {request.DiffId}."
                });

            if (!IsSameSize(left.Data, right.Data))
                return Task.FromResult(new CompareDataResponse
                {
                    Message = $"Size difference! Left: {left.Data.Length}; Right: {right.Data.Length}"
                });

            if (IsSameDataSequence(left.Data, right.Data))
                return Task.FromResult(new CompareDataResponse
                {
                    AreEqual = true,
                    Message = "They are equal!",
                });

            var offset = Offset(left.Data, right.Data);
            return Task.FromResult(new CompareDataResponse
            {
                Message = $"They are different from the index {offset}."
            });
            
        }

        private static int Offset(byte[] left, byte[] right)
        {
            var element = left.Except(right).First();
            return Array.IndexOf(left, element);
        }

        private static bool IsSameDataSequence(byte[] left, byte[] right)
        {
            return left.SequenceEqual(right);
        }

        private static bool IsSameSize(byte[] left, byte[] right)
        {
            return left.Length == right.Length;
        }
    }
}