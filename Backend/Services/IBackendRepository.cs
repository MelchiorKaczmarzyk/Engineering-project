using MyBackend.Entities;

namespace MyBackend.Services
{
    public interface IBackendRepository
    {
        Task<IEnumerable<Session>>? GetSessionsAsync();
        Task<IEnumerable<Gm>>? GetGmsAsync();
        Task<IEnumerable<Player>>? GetPlayersAsync();

        Task<Player> GetPlayerAsync(string id);
        Task<Session> GetSessionAsync(string id);
        Task<Gm> GetGmAsync(string id);
        public void AddSession(Session session);
    }
}
