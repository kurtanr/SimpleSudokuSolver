using SimpleSudokuSolver.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SimpleSudokuSolver.UI.ViewModel
{
  public class MainViewModel : INotifyPropertyChanged
  {
    public ICommand NewGameCommand { get; }
    public ICommand LoadGameCommand { get; }
    public ICommand SaveGameCommand { get; }
    public ICommand SolveGameCommand { get; }
    public ICommand UndoGameStepCommand { get; }
    public ICommand SolveGameStepCommand { get; }
    public ICommand ExitGameCommand { get; }

    public event Func<SudokuPuzzle> LoadGame;
    public event Action<SudokuPuzzle> SaveGame;
    public event Action ExitGame;
    public event Action InvalidSudokuLoaded;

    public Tuple<int, int> LastUpdatedCellIndex { get; private set; }

    private bool _showCandidates;
    public bool ShowCandidates
    {
      get => _showCandidates;
      set
      {
        _showCandidates = value;
        OnPropertyChanged(nameof(ShowCandidates));
      }
    }

    private SudokuPuzzle _solvedSudokuPuzzle;
    public SudokuPuzzle SolvedSudokuPuzzle
    {
      get => _solvedSudokuPuzzle;
      private set
      {
        _solvedSudokuPuzzle = value;
        OnPropertyChanged(nameof(SolvedSudokuPuzzle));
      }
    }

    private SudokuPuzzle _sudokuPuzzle;
    public SudokuPuzzle SudokuPuzzle
    {
      get => _sudokuPuzzle;
      private set
      {
        _sudokuPuzzle = value;
        OnPropertyChanged(nameof(SudokuPuzzle));
      }
    }

    private string _message;
    public string Message
    {
      get => _message;
      private set
      {
        _message = value;
        OnPropertyChanged(nameof(Message));
      }
    }

    private string _statusMessage;
    public string StatusMessage
    {
      get => _statusMessage;
      private set
      {
        _statusMessage = value;
        OnPropertyChanged(nameof(StatusMessage));
      }
    }

    private readonly ISudokuPuzzleProvider _puzzleProvider;
    private readonly ISudokuSolver _solver;

    public MainViewModel(
      ISudokuPuzzleProvider puzzleProvider,
      ISudokuSolver solver)
    {
      _puzzleProvider = puzzleProvider;
      _solver = solver;

      NewGameCommand = new RelayCommand(ExecuteNewGameCommand);
      LoadGameCommand = new RelayCommand(ExecuteLoadGameCommand);
      SaveGameCommand = new RelayCommand(ExecuteSaveGameCommand, () => SudokuPuzzle != null);
      SolveGameCommand = new RelayCommand(ExecuteSolveGameCommand, () => SudokuPuzzle != null);
      UndoGameStepCommand = new RelayCommand(ExecuteUndoGameStepCommand,
        () => SudokuPuzzle != null && SudokuPuzzle.NumberOfSteps > 0);
      SolveGameStepCommand = new RelayCommand(ExecuteSolveGameStepCommand, () => SudokuPuzzle != null);
      ExitGameCommand = new RelayCommand(ExecuteExitGameCommand);

      StatusMessage = "Start new game or load and existing one";
      ShowCandidates = true;
    }

    private void ExecuteNewGameCommand()
    {
      var sudoku = _puzzleProvider.GetPuzzle();
      SudokuPuzzle = new SudokuPuzzle(sudoku);
      SolvedSudokuPuzzle = _solver.Solve(sudoku);
      LastUpdatedCellIndex = null;
      Message = string.Empty;
      UpdateStatusMessage();
    }

    private void ExecuteLoadGameCommand()
    {
      var loadGame = LoadGame;
      if (loadGame == null)
      {
        return;
      }

      var sudokuPuzzle = loadGame();
      if (sudokuPuzzle == null)
      {
        return;
      }

      try
      {
        SolvedSudokuPuzzle = _solver.Solve(sudokuPuzzle.ToIntArray());
      }
      catch (Exception)
      {
        InvalidSudokuLoaded?.Invoke();
        return;
      }

      SudokuPuzzle = sudokuPuzzle;
      LastUpdatedCellIndex = null;
      Message = string.Empty;
      UpdateStatusMessage();
    }

    private void ExecuteSaveGameCommand()
    {
      SaveGame?.Invoke(SudokuPuzzle);
    }

    private void ExecuteSolveGameCommand()
    {
      if (SudokuPuzzle.IsSolved())
        return;

      var puzzle = _solver.Solve(SudokuPuzzle.ToIntArray());
      SudokuPuzzle = puzzle;

      foreach (var step in SudokuPuzzle.Steps)
      {
        AppendMessage(step.SolutionDescription);
      }

      if (!SudokuPuzzle.IsSolved())
        AppendMessage("Cannot solve puzzle");

      LastUpdatedCellIndex = null;
      UpdateStatusMessage();
    }

    private void ExecuteUndoGameStepCommand()
    {
      var solution = SudokuPuzzle.UndoLastSingleStepSolution();
      if (solution == null)
        return;

      if (solution.Result != null)
      {
        LastUpdatedCellIndex = new Tuple<int, int>(solution.Result.IndexOfRow,
          solution.Result.IndexOfColumn);
      }

      AppendMessage($"UNDO: {solution.SolutionDescription}");
      UpdateBoard();
      UpdateStatusMessage();
    }

    private void ExecuteSolveGameStepCommand()
    {
      if (SudokuPuzzle.IsSolved())
        return;

      var singleStepSolution = _solver.SolveSingleStep(SudokuPuzzle);
      if (singleStepSolution == null)
      {
        AppendMessage("Cannot solve puzzle step");
      }
      else
      {
        SudokuPuzzle.ApplySingleStepSolution(singleStepSolution);

        if (singleStepSolution.Result != null)
        {
          LastUpdatedCellIndex = new Tuple<int, int>(singleStepSolution.Result.IndexOfRow,
            singleStepSolution.Result.IndexOfColumn);
        }

        AppendMessage(singleStepSolution.SolutionDescription);
        UpdateBoard();
      }

      UpdateStatusMessage();
    }

    private void ExecuteExitGameCommand()
    {
      ExitGame?.Invoke();
    }

    private void UpdateBoard()
    {
      // to update UI, SudokuPuzzle property must change (ugly solution)
      var sudokuPuzzle = SudokuPuzzle;
      SudokuPuzzle = null;
      SudokuPuzzle = sudokuPuzzle;
    }

    public void AppendMessage(string message)
    {
      Message += $"{message}{Environment.NewLine}";
    }

    public void UpdateStatusMessage()
    {
      var isSolved = SudokuPuzzle.IsSolved();
      if (isSolved)
      {
        StatusMessage = "Sudoku is solved";
        return;
      }

      var sb = new StringBuilder("Values left: ");
      for (int i = 1; i <= SudokuPuzzle.NumberOfRowsOrColumnsInPuzzle; i++)
      {
        var numberOfValuesLeft = SudokuPuzzle.NumberOfRowsOrColumnsInPuzzle - SudokuPuzzle.Cells.OfType<Cell>().Count(x => x.Value == i);
        var separator = (i == SudokuPuzzle.NumberOfRowsOrColumnsInPuzzle) ? string.Empty : ",";
        if (numberOfValuesLeft > 0)
          sb.Append($"{i} x{numberOfValuesLeft}{separator} ");
      }

      StatusMessage = sb.ToString();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
