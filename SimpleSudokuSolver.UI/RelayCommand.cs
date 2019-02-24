using System;
using System.Diagnostics;
using System.Windows.Input;

namespace SimpleSudokuSolver.UI
{
  internal class RelayCommand : ICommand
  {
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    internal RelayCommand(Action execute, Func<bool> canExecute = null)
    {
      _execute = execute ?? throw new ArgumentNullException(nameof(execute));
      _canExecute = canExecute;
    }

    [DebuggerStepThrough]
    private bool CanExecute()
    {
      return _canExecute == null ? true : _canExecute();
    }

    private void Execute()
    {
      _execute();
    }

    bool ICommand.CanExecute(object parameter)
    {
      return CanExecute();
    }

    void ICommand.Execute(object parameter)
    {
      Execute();
    }

    event EventHandler ICommand.CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }
  }
}
