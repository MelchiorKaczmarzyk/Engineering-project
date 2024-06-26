using Microsoft.EntityFrameworkCore;
using MyBackend.DbContext;
using MyBackend.Entities;
using MyBackend.Models;
using System.Runtime.CompilerServices;

namespace MyBackend.Services
{
    public class BackendRepository : IBackendRepository
    {
        private readonly MyDbContext _context;

        public BackendRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<Gm> GetGmAsync(string id)
        {
            return await _context.Gms.FirstOrDefaultAsync(
                lol => lol.Id.Equals(id));
        }

        public async Task<IEnumerable<Gm>>? GetGmsAsync()
        {
            return await _context.Gms
                .Include(s => s.Sessions)
                .ToArrayAsync();
        }

        public async Task<Player> GetPlayerAsync(string id)
        {
            return await _context.Players.FirstOrDefaultAsync(
                lol => lol.Id.Equals(id));
        }

        public async Task<IEnumerable<Player>>? GetPlayersAsync()
        {
            return await _context.Players
                .Include(s => s.Sessions)
                .ToArrayAsync();
        }

        public async Task<Session> GetSessionAsync(string id)
        {
            return await _context.Sessions.FirstOrDefaultAsync(
                lol => lol.Id.Equals(id));
        }

        public void AddSession (Session session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Session>>? GetSessionsAsync()
        {
            return await _context.Sessions
                .Include(p => p.Players)
                .Include(g => g.Gm)
                .ToListAsync();
        }
    }
}
