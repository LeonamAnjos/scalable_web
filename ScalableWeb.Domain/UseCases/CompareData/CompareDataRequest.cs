using MediatR;

namespace ScalableWeb.Domain.UseCases.CompareData
{
    public class CompareDataRequest : IRequest<CompareDataResponse>
    {
        public int DiffId { get; set; }
    }
}