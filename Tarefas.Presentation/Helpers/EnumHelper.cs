using Tarefas.Presentation.Enums;

namespace Tarefas.Presentation.Helpers
{
    public static class EnumHelper
    {
        public static List<StatusTarefa> StatusTarefaValores =>
            Enum.GetValues(typeof(StatusTarefa)).Cast<StatusTarefa>().ToList();
    }
}
