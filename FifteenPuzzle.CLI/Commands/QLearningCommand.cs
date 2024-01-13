namespace FifteenPuzzle.CLI.Commands;

using System.CommandLine;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class QLearningCommand : Command
{
    //dotnet run qlearning

    public QLearningCommand(QLearning qLearning)
        : base("qlearning", "Q-learning is a reinforcement learning algorithm, which involves maintaining a Q-table that represents the quality of taking a certain action in a given state.")
    {
		this.SetHandler(qLearning.Learn);
    }
}
