using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Tarefas.Presentation.Enums;

namespace Tarefas.Presentation.Helpers
{
    public static class EnumHelper
    {
        public static List<StatusTarefa> StatusTarefaValores =>
            Enum.GetValues(typeof(StatusTarefa)).Cast<StatusTarefa>().ToList();

        public static string GetDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                  .FirstOrDefault() as DescriptionAttribute;

            return attribute?.Description ?? value.ToString();
        }
    }
}
