using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModsDeApi.Services.Thread
{
    public interface IThreadService
    {
        Task<IReadOnlyCollection<Thread>> GetThreads(int boardId, int page = 1);
        Task<Thread> GetThread(int threadId);
        Task<IReadOnlyCollection<Thread>> GetAllThreads(int boardId);
    }
}