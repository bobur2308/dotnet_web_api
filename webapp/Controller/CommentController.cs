using Microsoft.AspNetCore.Mvc;
using webapp.Dtos.Comment;
using webapp.Interfaces;
using webapp.Mappers;

namespace webapp.Controller
{
  [ApiController]
  [Route("api/comment")]
  public class CommentController : ControllerBase
  {
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;

    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
      _commentRepo = commentRepo;
      _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var comments = await _commentRepo.GetAllAsync();

      var comment_dtos = comments.Select(c => c.ToCommentDto());
      return Ok(comment_dtos);
    }
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      var comment = await _commentRepo.GetByIdAsync(id);
      if (comment == null) return NotFound();
      return Ok(comment);
    }

    [HttpPost]
    [Route("{stockId}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
    {
      if (!await _stockRepo.StockExists(stockId)) return BadRequest("Stock doesnt exist");

      var comment = commentDto.ToCommentFromCreate(stockId);

      await _commentRepo.CreateAsync(comment);
      return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, UpdateCommentRequestDto updateDto)
    {
      var comment = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());
      if (comment == null) return NotFound("Comment not found");
      return Ok(comment.ToCommentDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      var comment = await _commentRepo.DeleteAsync(id);
      if (comment == null) return NotFound("Comment not found");
      return NoContent();
    }
  }
}