using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tarefas.Presentation.Dtos;
using Tarefas.Presentation.Enums;
using Tarefas.Presentation.Helpers;
using Tarefas.Presentation.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Tarefas.Presentation.Validators;

namespace Tarefas.Presentation.ViewModels
{
    public class TarefaFormViewModel : ObservableObject, INotifyDataErrorInfo
    {
        private readonly ITarefaService _tarefaService;
        private readonly IValidator<TarefaDto> _validator;
        private Window? _windowToClose;

        private readonly Dictionary<string, List<string>> _errors = new();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _errors.Any();

        public TarefaDto Tarefa { get; }
        public ObservableCollection<StatusTarefa> StatusTarefaValores { get; }

        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; }

        public TarefaFormViewModel(ITarefaService tarefaService, TarefaDto? tarefa = null)
        {
            _tarefaService = tarefaService;
            _validator = new TarefaDtoValidator();

            Tarefa = tarefa != null ? new TarefaDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                DataCriacao = tarefa.DataCriacao,
                DataConclusao = tarefa.DataConclusao,
                Status = tarefa.Status
            } : new TarefaDto
            {
                DataCriacao = DateTime.Now,
                Status = StatusTarefa.Pendente
            };
            if (Tarefa.Id == 0)
            {
                StatusTarefaValores = new ObservableCollection<StatusTarefa>(
                EnumHelper.StatusTarefaValores.Where(x => x == StatusTarefa.Pendente));
                // mantem somente pendente da lista de status
            }
            else
            {
                StatusTarefaValores = new ObservableCollection<StatusTarefa>(
                EnumHelper.StatusTarefaValores.Where(x => x != StatusTarefa.Todos));
                // Excluir a opção "Todas" da lista de status
            }

            


            SalvarCommand = new AsyncRelayCommand(SalvarAsync);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        // Método para configurar a janela a ser fechada
        public void SetWindowToClose(Window window)
        {
            _windowToClose = window;
        }

        private async Task SalvarAsync()
        {
            Validate();

            if (HasErrors) return;

            if (Tarefa.Id == 0)
            {
                await _tarefaService.CriarAsync(Tarefa);
                MessageBox.Show("Tarefa salva com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                await _tarefaService.AtualizarAsync(Tarefa);
                MessageBox.Show("Tarefa atualizada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            _windowToClose?.Close(); // Fecha a janela após salvar
        }

        private void Cancelar()
        {
            _windowToClose?.Close(); // Fecha a janela ao cancelar
        }

        public void Validate()
        {
            _errors.Clear();
            ValidationResult result = _validator.Validate(Tarefa);

            foreach (var error in result.Errors)
            {
                if (!_errors.ContainsKey(error.PropertyName))
                    _errors[error.PropertyName] = new List<string>();

                _errors[error.PropertyName].Add(error.ErrorMessage);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(error.PropertyName));
                MessageBox.Show(error.ErrorMessage, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public System.Collections.IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return Enumerable.Empty<string>();
            return _errors.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<string>();
        }
    }
}
