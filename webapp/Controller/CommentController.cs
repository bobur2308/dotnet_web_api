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
      if(!ModelState.IsValid)
        return BadRequest(ModelState);
      var comments = await _commentRepo.GetAllAsync();

      var comment_dtos = comments.Select(c => c.ToCommentDto());
      return Ok(comment_dtos);
    }
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
      if(!ModelState.IsValid)
        return BadRequest(ModelState);

      var comment = await _commentRepo.GetByIdAsync(id);
      if (comment == null) return NotFound();
      return Ok(comment);
    }

    [HttpPost]
    [Route("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
    {
      if(!ModelState.IsValid)
        return BadRequest(ModelState);

      if (!await _stockRepo.StockExists(stockId)) return BadRequest("Stock doesnt exist");

      var comment = commentDto.ToCommentFromCreate(stockId);

      await _commentRepo.CreateAsync(comment);
      return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment.ToCommentDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, UpdateCommentRequestDto updateDto)
    {
      if(!ModelState.IsValid)
        return BadRequest(ModelState);

      var comment = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdate());
      if (comment == null) return NotFound("Comment not found");
      return Ok(comment.ToCommentDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      if(!ModelState.IsValid)
        return BadRequest(ModelState);
      
      var comment = await _commentRepo.DeleteAsync(id);
      if (comment == null) return NotFound("Comment not found");
      return NoContent();
    }
  }
}