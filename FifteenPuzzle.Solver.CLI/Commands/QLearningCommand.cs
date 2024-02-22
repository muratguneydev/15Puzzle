namespace FifteenPuzzle.Solver.Cli.Commands;

using System.CommandLine;
using FifteenPuzzle.Brokers;
using FifteenPuzzle.Cli.Tools.BoardRenering;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class QLearningCommand : Command
{
    private readonly QLearning _qLearning;
    private readonly ConsoleBoardRenderer _renderer;
    private readonly PuzzleLogger _logger;
    private bool _renderActionsOnConsoleEverySecond;
	private int _iterationCounter = 1;
	private int _actionCounter = 1;

    //dotnet run qlearning -r

    public QLearningCommand(QLearning qLearning, ConsoleBoardRenderer renderer, PuzzleLogger logger)
        : base("qlearning", "Q-learning is a reinforcement learning algorithm, which involves maintaining a Q-table that represents the quality of taking a certain action in a given state.")
    {
        _qLearning = qLearning;
        _renderer = renderer;
        _logger = logger;
        SetUpIterationEvents();

        var renderActionsOption = new Option<bool>(new[] { "--renderActions", "-r" }, () => false, "Show the board after each action for 1 second.");
        AddOption(renderActionsOption);
        this.SetHandler(Execute, renderActionsOption);
    }

    private void SetUpIterationEvents()
    {
        _qLearning.OnBoardActionQValueCalculated += OnBoardActionQValueCalculated;
        _qLearning.OnIterationCompleted += OnIterationCompleted;
    }

    private Task Execute(bool renderActions)
    {
		_renderActionsOnConsoleEverySecond = renderActions;
    	return _qLearning.Learn();
    }

    private void OnBoardActionQValueCalculated(BoardAction boardAction)
    {
		if (_renderActionsOnConsoleEverySecond)
        	_renderer.Render(boardAction.Board);
		
		_logger.LogInformation(new BoardText(boardAction.Board).Text);

		_actionCounter++;
		_logger.LogInformation($"Iteration:{_iterationCounter} Action:{_actionCounter}");
    }

	private void OnIterationCompleted(int obj)
    {
		_logger.LogInformation($"Iteration:{_iterationCounter} completed with Number Of Actions:{_actionCounter}");
		_iterationCounter++;
		_actionCounter = 1;

    }
}
