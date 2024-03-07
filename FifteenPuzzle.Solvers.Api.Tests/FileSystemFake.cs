namespace FifteenPuzzle.Solvers.Api.Tests;

using System.IO;
using System.Text;
using FifteenPuzzle.Solvers.ReinforcementLearning;

public class FileSystemFake : FileSystem
{
    private readonly string _streamContent;

    public FileSystemFake(string streamContent) => _streamContent = streamContent;

    public override bool FileExists(string filePath) => true;

    public override Stream GetFileStreamToRead(string filePath)
    {
        var byteArray = Encoding.UTF8.GetBytes(_streamContent);
		return new MemoryStream(byteArray);
    }

}