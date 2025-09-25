using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webapp.Data;
using webapp.Interfaces;
using webapp.Models;

namespace webapp.Repository
{
  public class CommentRepository : ICommentRepository
  {
    private readonly ApplicationDbContext _context;
    public CommentRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
      await _context.Comments.AddAsync(comment);
      await _context.SaveChangesAsync();
      return comment;
    }


    public async Task<List<Comment>> GetAllAsync()
    {
      return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
      return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Comment?> UpdateAsync(int id, Comment comentModel)
    {
      var comment = await _context.Comments.FindAsync(id);
      if (comment == null) return null;

      comment.Title = comentModel.Title;
      comment.Content = comentModel.Content;
      await _context.SaveChangesAsync();
      return comment;
    }
    public async Task<Comment?> DeleteAsync(int id)
    {
      var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
      if (comment == null) return null;
      _context.Comments.Remove(comment);
      await _context.SaveChangesAsync();
      return comment;
    }
  }
}