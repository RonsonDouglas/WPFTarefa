using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Tarefas.Presentation.Services.Interfaces;
using Tarefas.Presentation.Dtos;
using Tarefas.Presentation.Enums;
using Tarefas.Presentation.Helpers;
using System.Windows;

namespace Tarefas.Presentation.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ITarefaService _tarefaService;

        private StatusTarefa _selectedStatus;
        private List<TarefaDto> _todasTarefas = new();

        public ObservableCollection<TarefaDto> Tarefas { get; private set; } = new();

        public List<StatusTarefa> StatusTarefaValores => EnumHelper.StatusTarefaValores;

        public StatusTarefa SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                SetProperty(ref _selectedStatus, value);
                FilterTarefas();
            }
        }

        public ICommand AbrirJanelaTarefaCommand { get; }
        public ICommand CarregarTarefasCommand { get; }
        public ICommand EditarTarefaCommand { get; }
        public ICommand ExcluirTarefaCommand { get; }

        public MainViewModel(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
            AbrirJanelaTarefaCommand = new RelayCommand(AbrirJanelaTarefa);
            CarregarTarefasCommand = new RelayCommand(async () => await CarregarTarefasAsync());
            EditarTarefaCommand = new RelayCommand<TarefaDto>(EditarTarefa);
            ExcluirTarefaCommand = new AsyncRelayCommand<TarefaDto>(ExcluirTarefaAsync);

            SelectedStatus = StatusTarefa.Todos;

            // Carrega tarefas ao iniciar
            _ = CarregarTarefasAsync();
        }

        private async Task CarregarTarefasAsync()
        {
            var tarefas = await _tarefaService.ObterTodasAsync();
            _todasTarefas = tarefas.ToList();
            FilterTarefas();
        }

        private void FilterTarefas()
        {
            Tarefas.Clear();

            var tarefasFiltradas = SelectedStatus != StatusTarefa.Todos
                ? _todasTarefas.Where(t => t.Status == SelectedStatus)
                : _todasTarefas;

            foreach (var tarefa in tarefasFiltradas)
            {
                Tarefas.Add(tarefa);
            }
        }

        private void AbrirJanelaTarefa()
        {
            var viewModel = new TarefaFormViewModel(_tarefaService);
            var janelaTarefa = new TarefaFormWindow(viewModel);
            janelaTarefa.ShowDialog();
            _ = CarregarTarefasAsync();
        }

        private void EditarTarefa(TarefaDto tarefa)
        {
            if (tarefa == null) return;

            var viewModel = new TarefaFormViewModel(_tarefaService, tarefa);
            var janelaTarefa = new TarefaFormWindow(viewModel);
            janelaTarefa.ShowDialog();
            _ = CarregarTarefasAsync();
        }

        private async Task ExcluirTarefaAsync(TarefaDto tarefa)
        {
            if (tarefa == null) return;

            // Pergunta ao usuário se ele tem certeza da exclusão
            var resultado = MessageBox.Show(
                "Você tem certeza de que deseja excluir esta tarefa?",
                "Confirmação de Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                await _tarefaService.ExcluirAsync(tarefa.Id);
                await CarregarTarefasAsync();
            }
        }
    }
}
