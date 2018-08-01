using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScalableWeb.Domain.Models
{
    public class DataRecord
    {
        public int Id { get; set; }

        public DataSide Side { get; set; }

        [Required]
        public int DiffId { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }
}
