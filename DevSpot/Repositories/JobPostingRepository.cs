namespace DevSpot.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;
using DevSpot.Data;
using DevSpot.Models;
using Microsoft.EntityFrameworkCore;

public class JobPostingRepository : IRepsoitory<JobPosting>
{

    private readonly ApplicationDbContext _context; // database connection // read only means that we can assign value in the constructor or in definiton but not later on 
    // for example In any functions below we cannot assign readonly value
    public JobPostingRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(JobPosting entity)
    {
        await _context.JobPostings.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var jobPosting = await _context.JobPostings.FindAsync(id);

        if(jobPosting == null)
        {
            throw new KeyNotFoundException();
        }

        _context.JobPostings.Remove(jobPosting);
        await _context.SaveChangesAsync();
        
    }

    public async Task<IEnumerable<JobPosting>> GetAllAsync()
    {
        return await _context.JobPostings.ToListAsync();
    }

    public async Task<JobPosting> GetByIdAsync(int id)
    {
        var jobPosting = await _context.JobPostings.FindAsync(id);

        if (jobPosting == null)
        {
            throw new KeyNotFoundException();
        }

        return jobPosting;  
    }

    public async Task UpdateAsync(JobPosting entity)
    {
       _context.JobPostings.Update(entity);
        await _context.SaveChangesAsync();
    }
}
