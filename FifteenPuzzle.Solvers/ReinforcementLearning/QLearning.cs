using FifteenPuzzle.Brokers;

namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public class QLearning
{
    private readonly QLearningHyperparameters _parameters;
    private readonly QValueReader _qValueReader;
    private readonly QValueWriter _qValueWriter;
    private readonly PuzzleLogger _logger;

    //TODO: Dispose QValueWriter by the caller.
    public QLearning(QLearningHyperparameters parameters, QValueReader qValueReader, QValueWriter qValueWriter,
		PuzzleLogger logger)
	{
        _parameters = parameters;
        _qValueReader = qValueReader;
        _qValueWriter = qValueWriter;
        _logger = logger;
    }

    public int NumberOfIterations { get; private set; }

    public async Task Learn()
    {
        var qValueTable = await LoadPreviousLearningResults();
        for (var iteration = 0; iteration < _parameters.NumberOfIterations; iteration++)
        {

            NumberOfIterations++;
        }

        await SaveLearningResults(qValueTable);
    }

    private async Task SaveLearningResults(QValueTable qValueTable)
    {
		try
		{
			await _qValueWriter.Write(qValueTable);
		}
		catch(Exception e)
		{
			_logger.LogError("Error while saving learning results.", e);
			throw;
		}
    }

    private async ValueTask<QValueTable> LoadPreviousLearningResults()
    {
  		try
		{
			var boardActionQValuesCollection = await _qValueReader.Read();
			return new QValueTable(boardActionQValuesCollection, _parameters);
		}
		catch(Exception e)
		{
			_logger.LogError("Error while loading previous learning results.", e);
			return QValueTable.Empty(_parameters);
		}
    }
}

/*
from random import random, choice as random_choice, shuffle

# Pseudo-code for Q-learning iteration

# Initialize Q-values for all state-action pairs to arbitrary values. i.e. read existing from previously saved file, return empty if no file
Q = initialize_q_values()

# Set hyperparameters
alpha = 0.1  # Learning rate
gamma = 0.9  # Discount factor
epsilon = 0.1  # Exploration probability

# Number of iterations
num_iterations = 1000

# Main learning loop
for iteration in range(num_iterations):
    # Reset the environment to the initial state. i.e. get a random board
    state = reset_environment()

    # Iterate within an episode until a terminal state is reached
    while not is_terminal_state(state):
        # Choose an action using epsilon-greedy policy
        action = epsilon_greedy_policy(Q, state, epsilon)

        # Take the chosen action and observe the next state and reward
        next_state, reward = take_action(state, action)

        # Update the Q-value for the current state-action pair
        update_q_value(Q, state, action, next_state, reward, alpha, gamma)

        # Move to the next state
        state = next_state

# Function to initialize Q-values
def initialize_q_values():
    # Initialize Q-values to arbitrary values (e.g., all zeros)
    return {state: {action: 0.0 for action in all_possible_actions} for state in all_possible_states}

# Function to choose an action using epsilon-greedy policy
def epsilon_greedy_policy(Q, state, epsilon):
    # Exploration vs. exploitation trade-off
    if random() < epsilon:
        # Explore: Choose a random action
        return random_choice(all_possible_actions)
    else:
        # Exploit: Choose the action with the highest Q-value
        return argmax(all_possible_actions, key=lambda action: Q[state][action])

# Function to update the Q-value for a state-action pair
def update_q_value(Q, state, action, next_state, reward, alpha, gamma):
    # Q-learning update rule
    current_q_value = Q[state][action]
    max_future_q_value = max(Q[next_state].values()) if not is_terminal_state(next_state) else 0.0
    new_q_value = current_q_value + alpha * (reward + gamma * max_future_q_value - current_q_value)
    Q[state][action] = new_q_value

# Function to reset the environment to the initial state i.e. RandomBoard
def reset_environment():
    # Generate a list representing the solved state [1, 2, 3, ..., n**2-1, 0]
    solved_state = list(range(1, n**2)) + [0]

    # Shuffle the list to create a random initial state
    initial_state = solved_state[:]
    shuffle(initial_state)

    # Reshape the list into a 2D grid representing the puzzle
    initial_state_grid = [initial_state[i:i+n] for i in range(0, n**2, n)]

    return initial_state_grid

# Placeholder for other utility functions:

# Function to take an action and observe the next state and reward
def take_action(state, action):
    # Implement based on your environment
    pass

# Function to check if a state is a terminal state
def is_terminal_state(state):
    # Implement based on your environment
    pass

# Function to choose the element with the maximum value from a collection
def argmax(collection, key=None):
    # Implement based on your requirements
    pass

*/