namespace FifteenPuzzle.Play.Cli;

using FifteenPuzzle.Solvers.ReinforcementLearning;

public class RecommendationRenderer
{
    public string Render(ActionQValues actionQValues) =>
        string.Join(Environment.NewLine, actionQValues.Select(RenderActionQValue));

    private string RenderActionQValue(ActionQValue aqv) =>
		$"[blue]*{Padded(aqv.Move.Number) + "-" + aqv.QValue.ToString()}*[/]";

    private string Padded(int number) => Padded(number.ToString());
    private string Padded(string value) => value.PadLeft(2, ' ');
}
