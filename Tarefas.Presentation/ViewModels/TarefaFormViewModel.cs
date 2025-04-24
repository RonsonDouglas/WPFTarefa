using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;
using Tarefas.Presentation.Dtos;
using Tarefas.Presentation.Services.Interfaces;

namespace Tarefas.Presentation.ViewModels
{
    public class TarefaFormViewModel : ObservableObject
    {
        private readonly ITarefaService _tarefaService;
        private TarefaDto _tarefaDto;

        public TarefaDto TarefaDto
        {
            get => _tarefaDto;
            set => SetProperty(ref _tarefaDto, value);
        }

        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; }

        // Construtor modificado para aceitar um TarefaDto opcional
        public TarefaFormViewModel(ITarefaService tarefaService, TarefaDto tarefaDto = null)
        {
            _tarefaService = tarefaService;
            TarefaDto = tarefaDto ?? new TarefaDto(); // Se não passar tarefaDto, cria uma nova
            SalvarCommand = new RelayCommand(Salvar);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        private async void Salvar()
        {
            if (TarefaDto.Id == 0) // Se for uma nova tarefa
            {
                await _tarefaService.CriarAsync(TarefaDto);
            }
            else // Se for uma tarefa existente, atualize
            {
                await _tarefaService.AtualizarAsync(TarefaDto);
            }

            // Após salvar, fechar a janela
            CloseWindow();
        }

        private void Cancelar()
        {
            // Fechar a janela sem realizar alterações
            CloseWindow();
        }

        private void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }
    }
}
