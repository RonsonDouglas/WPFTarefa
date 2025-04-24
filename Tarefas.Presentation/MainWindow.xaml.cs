using System.Windows;
using Tarefas.Presentation.ViewModels;

namespace Tarefas.Presentation
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
