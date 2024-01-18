namespace FifteenPuzzle.CLI.Commands;

using System.CommandLine;
using FifteenPuzzle.Brokers;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class QLearningCommand : Command
{
    private readonly QLearning _qLearning;
    private readonly ConsoleBoardRenderer _renderer;
    private readonly PuzzleLogger _logger;
    private bool _renderActionsOnConsoleEverySecond;

    //dotnet run qlearning -r

    public QLearningCommand(QLearning qLearning, ConsoleBoardRenderer renderer, PuzzleLogger logger)
        : base("qlearning", "Q-learning is a reinforcement learning algorithm, which involves maintaining a Q-table that represents the quality of taking a certain action in a given state.")
    {
        _qLearning = qLearning;
        _renderer = renderer;
        _logger = logger;
        _qLearning.OnBoardActionQValueCalculated += OnBoardActionQValueCalculated;

        var renderActionsOption = new Option<bool>(new[] { "--renderActions", "-r" }, () => false, "Show the board after each action for 1 second.");
        AddOption(renderActionsOption);
		this.SetHandler(Execute, renderActionsOption);
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
    }
}
