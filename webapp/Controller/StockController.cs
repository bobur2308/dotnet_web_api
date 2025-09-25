
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Dtos.Stock;
using webapp.Helpers;
using webapp.Interfaces;
using webapp.Mappers;

namespace webapp.Controller
{
  [Route("api/stock")]
  [ApiController]
  public class StockController : ControllerBase
  {
    private readonly ApplicationDbContext _context;
    private readonly IStockRepository _stockRepo;
    public StockController(ApplicationDbContext context, IStockRepository stockRepo)
    {
      _stockRepo = stockRepo;
      _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
      var stocks = await _stockRepo.GetAllAsync(query);
      var stockDtos = stocks.Select(s => s.ToStockDto()).ToList();

      return Ok(stockDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
      var stock = await _stockRepo.GetByIdAsync(id);
      if (stock == null)
      {
        return NotFound();
      }
      return Ok(stock);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
      var new_stock = stockDto.ToStackFromCreateDTO();
      await _stockRepo.CreateAsync(new_stock);
      return CreatedAtAction(nameof(GetById), new { id = new_stock.Id }, new_stock.ToStockDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
      var stock = await _stockRepo.UpdateAsync(id, updateDto);

      if (stock == null)
      {
        return NotFound();
      }

      return Ok(stock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      var stock = await _stockRepo.DeleteAsync(id);
      if (stock == null)
      {
        return NotFound();
      }
      return NoContent();
    }
  }
}