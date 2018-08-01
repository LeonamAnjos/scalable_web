using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ScalableWeb.Domain.Models;
using ScalableWeb.Domain.Patterns;

namespace ScalableWeb.Domain.UseCases.AddData
{
    public class AddDataCommand : IRequestHandler<AddDataRequest, AddDataResponse>
    {
        private readonly IRepository<DataRecord> _repository;
        private readonly ICollection<ValidationResult> _validationResults = new List<ValidationResult>();

        public AddDataCommand(IRepository<DataRecord> repository)
        {
            _repository = repository;
        }

        public Task<AddDataResponse> Handle(AddDataRequest request, CancellationToken cancellationToken)
        {
            var record = new DataRecord
            {
                DiffId = request.DiffId,
                Side = request.Side,
                Data = request.Data
            };

            if (!Validator.TryValidateObject(record, new ValidationContext(record), _validationResults))
            {
                return Task.FromResult(new AddDataResponse
                {
                    ErrorMessage = string.Join("; ", _validationResults.Select(r => r.ErrorMessage))
                });
            }

            _repository.Insert(record);

            return Task.FromResult(new AddDataResponse { Success = true });
        }
        
    }
}