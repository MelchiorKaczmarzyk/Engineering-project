using MyBackend.Entities;

namespace MyBackend.Services
{
    public interface IBackendRepository
    {
        Task<IEnumerable<Session>>? GetSessionsAsync();
        Task<Session> GetSessionAsyncTitle(string title);
        Task<IEnumerable<Gm>>? GetGmsAsync();
        Task<IEnumerable<Player>>? GetPlayersAsync();
        Task<IEnumerable<GameSystem>>? GetSystemsAsync();

        Task<Player> GetPlayerAsync(string id);
        Task<Session> GetSessionAsync(string id);
        Task<Gm> GetGmAsync(string id);
        Task<GameSystem> GetSystemAsync(string id);

        Task<User> GetUserByGmOrPlayerId(string id);
        public void AddSession(Session session);
        public void AddSystem(GameSystem system);
        public void AddGm(Gm gm);
        public void AddPlayer(Player player);

        Task<User> GetUserByUserName(string userName);
        Task<Player> GetPlayerAsyncName(string playerName);
        void AddPlayerToSession(string sessionTitle, string playerName);

        Task<IEnumerable<User>>? GetUsersAsync();
        void DeleteUser(string userName);

        void DeleteSession(string sessionTitle);

        void SaveContextChanges();
        
    }
}
