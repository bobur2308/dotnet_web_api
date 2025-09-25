using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webapp.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters.")]
        public required string Symbol { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "CompanyName cannot exceed 10 characters.")]
        public required string CompanyName { get; set; }
        [Required]
        [Range(1,1000000, ErrorMessage = "Purchase price must be between 1 and 1,000,000.")]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001,100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot exceed 10 characters.")]
        public string Industry { get; set; } = string.Empty;
        [Range(1,5000000000)]
        public long MarketCap { get; set; }
    }
}