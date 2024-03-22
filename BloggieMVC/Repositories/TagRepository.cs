using BloggieMVC.Data;
using BloggieMVC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BloggieMVC.Repositories;

public class TagRepository : ITagInterface
{
    private readonly BloggieDbContext _dbContext;

    public TagRepository(BloggieDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Tag>> GetAllAsync()
    {
        return await _dbContext.Tags.ToListAsync();
    }

    public Task<Tag?> GetAsync(Guid id)
    {
        return _dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Tag> AddAsync(Tag tag)
    {
        await _dbContext.Tags.AddAsync(tag);
        await _dbContext.SaveChangesAsync();

        return tag;
    }

    public async Task<Tag?> UpdateAsync(Tag tag)
    {
        var existingTag = await _dbContext.Tags.FindAsync(tag.Id);
        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;

            await _dbContext.SaveChangesAsync();

            return existingTag;
        }

        return null;
    }

    public async Task<Tag?> DeleteAsync(Guid id)
    {
        var existingTag = await _dbContext.Tags.FindAsync(id);
        if (existingTag != null)
        {
            _dbContext.Tags.Remove(existingTag);
            await _dbContext.SaveChangesAsync();
            return existingTag;
        }

        return null;
    }
}