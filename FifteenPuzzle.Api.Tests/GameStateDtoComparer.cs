namespace FifteenPuzzle.Api.Tests;

using System.Diagnostics.CodeAnalysis;
using FifteenPuzzle.Api.Contracts;

public class GameStateDtoComparer : IEqualityComparer<GameStateDto>
{
    public bool Equals(GameStateDto? x, GameStateDto? y) =>
        x == null || y == null ? false : new BoardDtoComparer().Equals(x.Board, y.Board);


    public int GetHashCode([DisallowNull] GameStateDto obj) => obj.GetHashCode();
}