namespace FifteenPuzzle.Solvers.Api;

using FifteenPuzzle.Game;
using Newtonsoft.Json;

public class MoveConverter : JsonConverter<Move>
{
    public override Move ReadJson(JsonReader reader, Type objectType, Move? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Integer)
        {
            int number = Convert.ToInt32(reader.Value);
            return new Move(number);
        }
        else if (reader.TokenType == JsonToken.String)
        {
			ArgumentNullException.ThrowIfNull(reader.Value);
            string numberString = (string)reader.Value;
            return new Move(numberString);
        }
        else
        {
            throw new JsonSerializationException($"Unexpected token type '{reader.TokenType}' when deserializing Move.");
        }
    }

    public override void WriteJson(JsonWriter writer, Move? value, JsonSerializer serializer)
    {
		ArgumentNullException.ThrowIfNull(value);
        writer.WriteValue(value.Number);
    }
}