namespace Tarefas.Presentation.Dtos
{
    public class TarefaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string Status { get; set; } = "Pendente"; // Pendente, EmProgresso, Concluida
    }
}
