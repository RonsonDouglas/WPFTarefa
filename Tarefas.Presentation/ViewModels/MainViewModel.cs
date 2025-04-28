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

        private int _currentPage = 1;
        private int _totalPages;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    FilterTarefas();
                }
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            private set => SetProperty(ref _totalPages, value);
        }

        public ICommand AbrirJanelaTarefaCommand { get; }
        public ICommand CarregarTarefasCommand { get; }
        public ICommand EditarTarefaCommand { get; }
        public ICommand ExcluirTarefaCommand { get; }
        public ICommand ProximaPaginaCommand { get; }
        public ICommand PaginaAnteriorCommand { get; }

        public MainViewModel(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
            AbrirJanelaTarefaCommand = new RelayCommand(AbrirJanelaTarefa);
            CarregarTarefasCommand = new RelayCommand(async () => await CarregarTarefasAsync());
            EditarTarefaCommand = new RelayCommand<TarefaDto>(EditarTarefa);
            ExcluirTarefaCommand = new AsyncRelayCommand<TarefaDto>(ExcluirTarefaAsync);
            ProximaPaginaCommand = new RelayCommand(ProximaPagina);
            PaginaAnteriorCommand = new RelayCommand(PaginaAnterior);

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

            // Implementando a paginação
            var tarefasParaExibir = tarefasFiltradas
                .Skip((CurrentPage - 1) * 5)
                .Take(5);

            foreach (var tarefa in tarefasParaExibir)
            {
                Tarefas.Add(tarefa);
            }

            // Atualizando o total de páginas
            TotalPages = (int)Math.Ceiling(tarefasFiltradas.Count() / 5.0);
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

            await _tarefaService.ExcluirAsync(tarefa.Id);
            await CarregarTarefasAsync();
        }

        private void ProximaPagina()
        {
            if (CurrentPage < TotalPages)
                CurrentPage++;
        }

        private void PaginaAnterior()
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }
    }
}
