using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using Tarefas.Presentation.Dtos;
using Tarefas.Presentation.Services.Interfaces;
using System.Net;

namespace Tarefas.Presentation.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly HttpClient _httpClient;

        public TarefaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5001/api/"); // ajuste se necessário
        }

        public async Task<List<TarefaDto>> ObterTodasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("tarefa");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<TarefaDto>>(json) ?? new List<TarefaDto>();
                }
                else
                {
                 
                    return new List<TarefaDto>(); 
                }
            }
            catch (HttpRequestException ex)
            {
                
                throw new Exception("Erro ao obter tarefas.", ex);
            }
            catch (Exception ex)
            {
                
                throw new Exception("Erro inesperado ao obter tarefas.", ex);
            }
        }

        public async Task<TarefaDto?> ObterPorIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"tarefa/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TarefaDto>(json);
                }
                else
                {
                    return null; // Ou tratar com uma exceção específica
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro ao obter tarefa com ID {id}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao obter tarefa.", ex);
            }
        }

        public async Task CriarAsync(TarefaDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("tarefa", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Erro ao criar tarefa.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao criar tarefa.", ex);
            }
        }

        public async Task AtualizarAsync(TarefaDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"tarefa/{dto.Id}", content);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro ao atualizar tarefa com ID {dto.Id}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao atualizar tarefa.", ex);
            }
        }

        public async Task ExcluirAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"tarefa/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Erro ao excluir tarefa com ID {id}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao excluir tarefa.", ex);
            }
        }
    }
}
