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
        private readonly Window? _windowToClose;

        private readonly Dictionary<string, List<string>> _errors = new();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _errors.Any();

        public TarefaDto Tarefa { get; }
        public ObservableCollection<StatusTarefa> StatusTarefaValores { get; }

        public ICommand SalvarCommand { get; }
        public ICommand CancelarCommand { get; }

        public TarefaFormViewModel(ITarefaService tarefaService, TarefaDto? tarefa = null, Window? window = null)
        {
            _tarefaService = tarefaService;
            _validator = new TarefaDtoValidator();
            _windowToClose = window;

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

            StatusTarefaValores = new ObservableCollection<StatusTarefa>(EnumHelper.StatusTarefaValores);
            SalvarCommand = new AsyncRelayCommand(SalvarAsync);
            CancelarCommand = new RelayCommand(Cancelar);
        }

        private async Task SalvarAsync()
        {
            Validate();

            if (HasErrors) return;

            if (Tarefa.Id == 0)
                await _tarefaService.CriarAsync(Tarefa);
            else
                await _tarefaService.AtualizarAsync(Tarefa);

            _windowToClose?.Close();
        }

        private void Cancelar()
        {
            _windowToClose?.Close();
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
            }
        }

        public System.Collections.IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return Enumerable.Empty<string>();
            return _errors.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<string>();
        }
    }
}
