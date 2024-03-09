namespace FifteenPuzzle.Cli.Tools.BoardRendering;

using FifteenPuzzle.Game;

public class ConsoleBoardRenderer
{
    private readonly TableBoardRenderer _tableBoardRenderer;
    private readonly FlatBoardRenderer _flatBoardRenderer;

    public ConsoleBoardRenderer(TableBoardRenderer tableBoardRenderer, FlatBoardRenderer flatBoardRenderer)
	{
        _tableBoardRenderer = tableBoardRenderer;
        _flatBoardRenderer = flatBoardRenderer;
    }

    public void Render(Board board)
	{
		_tableBoardRenderer.Render(board);
		_flatBoardRenderer.Render(board);
		Thread.Sleep(1000);
	}
}
