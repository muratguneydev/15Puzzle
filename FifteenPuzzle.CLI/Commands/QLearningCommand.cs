namespace FifteenPuzzle.CLI.Commands;

using System.CommandLine;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class QLearningCommand : Command
{
    private readonly QLearning _qLearning;
    private readonly Renderer _renderer;

    //dotnet run qlearning -r

    public QLearningCommand(QLearning qLearning, Renderer renderer)
        : base("qlearning", "Q-learning is a reinforcement learning algorithm, which involves maintaining a Q-table that represents the quality of taking a certain action in a given state.")
    {
        _qLearning = qLearning;
        _renderer = renderer;

        var renderActionsOption = new Option<bool>(new[] { "--renderActions", "-r" }, () => false, "Show the board after each action for 1 second.");
        AddOption(renderActionsOption);
		this.SetHandler(Execute, renderActionsOption);
    }

	private Task Execute(bool renderActions)
    {
		if (renderActions)
			_qLearning.OnBoardActionQValueCalculated += OnBoardActionQValueCalculated;

    	return _qLearning.Learn();
    }

    private void OnBoardActionQValueCalculated(BoardAction boardAction) => _renderer.Render(boardAction.Board);
}
