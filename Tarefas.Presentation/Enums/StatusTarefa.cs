using System.ComponentModel;

namespace Tarefas.Presentation.Enums
{
    public enum StatusTarefa
    {
        [Description("Todos")]
        Todos,

        [Description("Pendente")]
        Pendente,

        [Description("Em Progresso")]
        EmProgresso,

        [Description("Concluída")]
        Concluida
    }
}
