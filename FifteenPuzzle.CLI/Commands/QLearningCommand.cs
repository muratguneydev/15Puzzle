namespace FifteenPuzzle.CLI.Commands;

using System.CommandLine;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class QLearningCommand : Command
{
    private readonly Renderer _renderer;

    //dotnet run qlearning

    public QLearningCommand(QLearning qLearning, Renderer renderer)
        : base("qlearning", "Q-learning is a reinforcement learning algorithm, which involves maintaining a Q-table that represents the quality of taking a certain action in a given state.")
    {
        _renderer = renderer;

		qLearning.OnBoardActionQValueCalculated += OnBoardActionQValueCalculated;
		this.SetHandler(qLearning.Learn);
    }

    private void OnBoardActionQValueCalculated(BoardAction boardAction) => _renderer.Render(boardAction.Board);
}
