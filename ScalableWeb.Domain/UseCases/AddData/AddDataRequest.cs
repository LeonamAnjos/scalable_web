using MediatR;
using ScalableWeb.Domain.Models;

namespace ScalableWeb.Domain.UseCases.AddData
{
    public class AddDataRequest : IRequest<AddDataResponse>
    {
        public int DiffId { get; set; }
        public DataSide Side { get; set; }
        public byte[] Data { get; set; }
        
    }
}