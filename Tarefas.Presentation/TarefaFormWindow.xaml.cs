using System.Windows;
using Tarefas.Presentation.ViewModels;

namespace Tarefas.Presentation
{
    public partial class TarefaFormWindow : Window
    {
        public TarefaFormWindow(TarefaFormViewModel viewModel)
        {
            InitializeComponent();
            viewModel.SetWindowToClose(this);

            this.DataContext = viewModel; 

        }
    }
}
