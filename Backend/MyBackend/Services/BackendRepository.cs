using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBackend.DbContext;
using MyBackend.Entities;
using MyBackend.Models;
using System.Numerics;

namespace MyBackend.Services
{
    public class BackendRepository : IBackendRepository
    {
        private readonly MyDbContext _context;
        private readonly UserManager<User> _userManager;
        public BackendRepository(MyDbContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
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

        public void AddSystem(GameSystem system)
        {
            _context.Systems.Add(system);
            _context.SaveChanges();
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

        public async Task<Player> GetPlayerAsyncName(string playerName)
        {
            return await _context.Players.FirstOrDefaultAsync(p => p.Name.Equals(playerName));
        }

        public async Task<Session> GetSessionAsync(string id)
        {
            return await _context.Sessions.FirstOrDefaultAsync(
                lol => lol.Id.Equals(id));
        }

        public async Task<Session> GetSessionAsyncTitle(string title)
        {
            return await _context.Sessions
                .Include(p => p.Players)
                .Include(g => g.Gm)
                .FirstOrDefaultAsync(lol => lol.Title.Equals(title));
        }
        public async Task<IEnumerable<Session>>? GetSessionsAsync()
        {
           var results = await _context.Sessions
                .Include(p => p.Players)
                .Include(g => g.Gm)
                .Include(s => s.System)
                .ToListAsync();
            return results;
        }

        public void AddSession (Session session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }

        public void AddPlayerToSession(string sessionTitle, string playerName)
        {
            var session = _context.Sessions.FirstOrDefault(s =>  s.Title.Equals(sessionTitle));
            var player = _context.Players.FirstOrDefault(p =>  p.Name.Equals(playerName));

            session.Players.Add(player);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<GameSystem>>? GetSystemsAsync()
        {
            return await _context.Systems.ToListAsync();
        }

        public async Task<GameSystem> GetSystemAsync(string id)
        {
            return await _context.Systems.FirstOrDefaultAsync(s => s.Id.Equals(id));
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(userName));
        }

        public async Task<User> GetUserByGmOrPlayerId(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.GmOrPlayerId == id);
        }
        public void AddPlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        public void AddGm(Gm gm)
        {
            _context.Gms.Add(gm);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<User>>? GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public void SaveContextChanges()
        {
            _context.SaveChanges();
        }

        public async void DeleteUser(string userName)
        {
            //znajdź użytkownika
            //oceń czy to gracz czy Gm

            var user =_context.Users.FirstOrDefault(u => u.UserName.Equals(userName));
            if (user != null)
            {
                //Gm =>
                //znajdź gma
                //znajdź wszystkie sesje, gdzie on jest gmem
                //usuń wszystkie sesje
                //usuń gma
                //usuń użytkownika
                if (user.Role.Equals("Gm"))
                {
                    //Musisz zadbać o to, żeby kiedy pobierasz rekord z bazy, to żeby miał wszystkie pola!
                    //            tu => \/
                    var gm = _context.Gms.FirstOrDefault(g => g.Id == user.GmOrPlayerId);
                    if(gm != null)
                    {
                        var sessions = _context.Sessions.Where(s => s.GmId == gm.Id);
                        if(sessions.Any())
                        {
                            foreach (var session in sessions)
                            {
                                //nwm czy zadziała, porównuje referencje
                                _context.Remove(session);
                            }
                        }
                        //nwm czy zadziała, porównuje referencje
                        var lol = _context.Remove(gm);
                        await _userManager.DeleteAsync(user);
                        _context.SaveChanges();
                    }
                }
                //Gracz =>
                //znajdź gracza
                //znajdź wszystkie sesje, w których występuje ten gracz
                //usuń z każdej z tych sesji tego gracza
                //usuń gracza
                //usuń użytkownika
                if (user.Role.Equals("Player"))
                {
                    var player = _context.Players.FirstOrDefault(p => p.Id == user.GmOrPlayerId);
                    if (player != null)
                    {
                        var sessions = _context.Sessions.Where(s => 
                            findSessionsWithPlayer(s,player));
                        if(sessions.Any())
                        {
                            foreach (var session in sessions)
                            {
                                foreach (var p in session.Players)
                                {
                                    if (p.Id.Equals(player.Id))
                                    {
                                        //nwm czy zadziała, porównuje referencje
                                        var playerRemovedFromSession = session.Players.Remove(p);
                                    }
                                }
                                var deletedSession = _context.Remove(session);
                            }
                        }
                        //nwm czy zadziała, porównuje referencje
                        var deletedPlayer = _context.Remove(player);
                        _userManager.DeleteAsync(user);
                        _context.SaveChanges();
                    }
                }
            }
        }
        private bool findSessionsWithPlayer(Session s, Player player)
        {
            foreach (var p in s.Players)
            {
                if (p.Id.Equals(player.Id))
                {
                    return true;
                }
            }
            return false;
        }

        public async void DeleteSession(string sessionTitle)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Title == sessionTitle);
            //nwm czy usunie, bo porównuje referencje
            _context.Sessions.Remove(session);
            _context.SaveChanges();
        }

    }
}
