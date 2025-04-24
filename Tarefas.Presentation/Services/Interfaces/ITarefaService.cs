
using Tarefas.Presentation.Dtos;


namespace Tarefas.Presentation.Services.Interfaces
{
    public interface ITarefaService
    {
        Task<List<TarefaDto>> ObterTodasAsync();
        Task<TarefaDto?> ObterPorIdAsync(int id);
        Task CriarAsync(TarefaDto dto);
        Task AtualizarAsync(TarefaDto dto);
        Task ExcluirAsync(int id);
    }
}
