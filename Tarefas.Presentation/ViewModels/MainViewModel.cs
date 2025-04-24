using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Tarefas.Presentation.Services.Interfaces;
using Tarefas.Presentation.Dtos;

namespace Tarefas.Presentation.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ITarefaService _tarefaService;

        public ObservableCollection<TarefaDto> Tarefas { get; } = new();

        public ICommand AbrirJanelaTarefaCommand { get; }
        public ICommand CarregarTarefasCommand { get; }
        public ICommand EditarTarefaCommand { get; }
        public ICommand ExcluirTarefaCommand { get; }

        public MainViewModel(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
            AbrirJanelaTarefaCommand = new RelayCommand(AbrirJanelaTarefa);
            CarregarTarefasCommand = new RelayCommand(CarregarTarefas);
            EditarTarefaCommand = new RelayCommand<TarefaDto>(EditarTarefa);
            ExcluirTarefaCommand = new AsyncRelayCommand<TarefaDto>(ExcluirTarefaAsync);
        }

        private async void CarregarTarefas()
        {
            var tarefas = await _tarefaService.ObterTodasAsync();
            Tarefas.Clear();
            foreach (var tarefa in tarefas)
            {
                Tarefas.Add(tarefa);
            }
        }

        private void AbrirJanelaTarefa()
        {
            var viewModel = new TarefaFormViewModel(_tarefaService);
            var janelaTarefa = new TarefaFormWindow(viewModel);
            janelaTarefa.ShowDialog();
            CarregarTarefas();
        }

        private void EditarTarefa(TarefaDto tarefa)
        {
            if (tarefa == null) return;

            var viewModel = new TarefaFormViewModel(_tarefaService, tarefa); // Abre com dados preenchidos
            var janelaTarefa = new TarefaFormWindow(viewModel);
            janelaTarefa.ShowDialog();
            CarregarTarefas(); // Atualiza após edição
        }

        private async Task ExcluirTarefaAsync(TarefaDto tarefa)
        {
            if (tarefa == null) return;

            await _tarefaService.ExcluirAsync(tarefa.Id);
            CarregarTarefas(); // Atualiza após exclusão
        }
    }
}
