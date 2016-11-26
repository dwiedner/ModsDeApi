using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModsDeApi.Services.Board
{
    public interface IBoardService
    {
        Task<IReadOnlyCollection<Category>> GetCategories();
        Task<IReadOnlyCollection<Board>> GetBoards();
        Task<Board> GetBoard(int boardId);
    }
}