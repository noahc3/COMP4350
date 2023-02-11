﻿using ThreaditAPI.Database;
using ThreaditAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ThreaditAPI.Repositories
{
    public class SpoolRepository : AbstractRepository
    {
        public SpoolRepository(PostgresDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Spool?> GetSpoolAsync(Spool spool)
        {
            return await GetSpoolAsync(spool.Id);
        }

        public async Task<Spool?> GetSpoolAsync(string spoolId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            return dbSpool == default ? null : dbSpool;
        }

        public async Task InsertSpoolAsync(Spool spool)
        {
            await db.Spools.AddAsync(spool);
            await db.SaveChangesAsync();
        }

        public async Task<List<string>> GetModeratorsAsync(string spoolId)
        {
            //List<string> returnList = new List<string>();
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            //return dbSpool.Moderators;
            if (dbSpool != null)
            {
                return dbSpool.Moderators.First().Split(',').ToList();
            }
            return new List<string> { };
        }

        public async Task AddModeratorAsync(string spoolId, string userId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            if (dbSpool == null)
                return;
            dbSpool.Moderators.Add(userId);
            await db.SaveChangesAsync();
        }

        public async Task RemoveModeratorAsync(string spoolId, string userId)
        {
            Spool? dbSpool = await db.Spools.FirstOrDefaultAsync(u => u.Id == spoolId);
            if (dbSpool == null)
                return;
            dbSpool.Moderators.Remove(userId);
            await db.SaveChangesAsync();
        }
    }
}
