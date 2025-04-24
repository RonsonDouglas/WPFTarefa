using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarefas.Presentation.Models
{
    using Tarefas.Presentation.Enums;
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataConclusao { get; set; }
        public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
    }
}
