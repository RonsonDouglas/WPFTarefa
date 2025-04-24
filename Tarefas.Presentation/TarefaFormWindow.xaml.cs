using System.Windows;
using Tarefas.Presentation.ViewModels;

namespace Tarefas.Presentation
{
    public partial class TarefaFormWindow : Window
    {
        public TarefaFormWindow(TarefaFormViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel; // Define o DataContext como o ViewModel
        }
    }
}
