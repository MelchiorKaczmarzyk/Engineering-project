using MyBackend.Entities;
using MyBackend.Models;

namespace MyBackend.Services
{
    public interface IBackendRepository
    {
        Task<IEnumerable<Tag>>? GetTagsAsync();
        Task<IEnumerable<Trigger>>? GetTriggersAsync();
        Task<IEnumerable<Vtt>>? GetVttsAsync();
        Task<IEnumerable<Session>>? GetSessionsAsync();
        Task<Session> GetSessionAsyncTitle(string title);
        Task<IEnumerable<Gm>>? GetGmsAsync();
        Task<Gm> GetGmAsyncName(string name);
        Task<IEnumerable<Player>>? GetPlayersAsync();
        Task<IEnumerable<GameSystem>>? GetSystemsAsync();

        Task<Player> GetPlayerAsync(string id);
        Task<Session> GetSessionAsync(string id);
        Task<Gm> GetGmAsync(string id);
        Task<GameSystem> GetSystemAsync(string id);

        Task<User> GetUserByGmOrPlayerId(string id);
        public void AddSession(Session session);
        public void AddGm(Gm gm);
        public void AddPlayer(Player player);
        public void AddSystem(GameSystem system);
        public void AddTag(Tag tag);
        public void AddTrigger(Trigger trigger);
        public void AddVtt(Vtt vtt);

        Task<User> GetUserByUserName(string userName);
        Task<Player> GetPlayerAsyncName(string playerName);
        void AddPlayerToSession(string sessionTitle, string playerName);

        Task<IEnumerable<User>>? GetUsersAsync();

        void DeleteUser(string userName);
        void DeleteSystem(string systemName);
        void DeleteSession(string sessionTitle);
        void DeleteTag(string tagName);
        void DeleteTrigger(string triggerName);
        void DeleteVtt(string vttName);

        void SaveContextChanges();
        
    }
}
