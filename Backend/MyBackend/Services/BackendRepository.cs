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

        public async Task<Gm> GetGmAsyncName(string name)
        {
            return await _context.Gms.FirstOrDefaultAsync(
                lol => lol.Name.Equals(name));
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
            var session = _context.Sessions.Include(s => s.Players).FirstOrDefault(s =>  s.Title.Equals(sessionTitle));
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
                    var gm = _context.Gms.Include(g => g.Sessions).FirstOrDefault(g => g.Id == user.GmOrPlayerId);
                    if(gm != null)
                    {
                        var sessions = gm.Sessions;
                        if(sessions.Count() > 0)
                        {
                            foreach (var session in sessions)
                            {
                                //nwm czy zadziała, porównuje referencje
                                var removedSession = _context.Remove(session);
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
                    var sessionsFromRemove = new List<Session>();
                    var player = _context.Players.Include(p => p.Sessions).FirstOrDefault(p => p.Id == user.GmOrPlayerId);
                    if (player != null)
                    {
                        var sessions = player.Sessions;
                        if (sessions.Count() > 0)
                        {
                            foreach (var session in sessions)
                            {
                                foreach (var p in session.Players)
                                {
                                    if (p.Id.Equals(player.Id))
                                    {
                                        sessionsFromRemove.Add(session);
                                        break;
                                        //var playerRemovedFromSession = session.Players.Remove(p);
                                    }
                                }
                                //var deletedSession = _context.Remove(session);
                            }

                            foreach (var session in sessionsFromRemove)
                            {
                                var playerRemovedFromSession = session.Players.Remove(player);
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
                else
                {
                    continue;
                }
            }
            return false;
        }

        public async void DeleteSession(string sessionTitle)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Title == sessionTitle);
            _context.Sessions.Remove(session);
            _context.SaveChanges();
        }

        public async void DeleteTag(string tagName)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
            _context.Tags.Remove(tag);
            _context.SaveChanges();
        }
        public async void DeleteTrigger(string triggerName)
        {
            var trigger = await _context.Triggers.FirstOrDefaultAsync(t => t.Name == triggerName);
            _context.Triggers.Remove(trigger);
            _context.SaveChanges();
        }
        public async void DeleteVtt(string vttName)
        {
            var vtt = await _context.Vtts.FirstOrDefaultAsync(v => v.Name == vttName);
            _context.Vtts.Remove(vtt);
            _context.SaveChanges();
        }
        public async void DeleteSystem(string systemName)
        {
            var system = await _context.Systems.FirstOrDefaultAsync(s => s.Name == systemName);
            _context.Systems.Remove(system);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Tag>>? GetTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public void AddTag(Tag tag)
        {
             _context.Tags.Add(tag);
        }
        public void AddTrigger(Trigger trigger)
        {
            _context.Triggers.Add(trigger);
        }
        public void AddVtt(Vtt Vtt)
        {
            _context.Vtts.Add(Vtt);
        }

        public async Task<IEnumerable<Trigger>>? GetTriggersAsync()
        {
            return await _context.Triggers.ToListAsync();
        }

        public async Task<IEnumerable<Vtt>>? GetVttsAsync()
        {
            return await _context.Vtts.ToListAsync();
        }

    }
}
