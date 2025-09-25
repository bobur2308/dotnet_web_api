using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapp.Dtos.Stock;
using webapp.Helpers;
using webapp.Models;

namespace webapp.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto dto);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExists(int id);
    }
}