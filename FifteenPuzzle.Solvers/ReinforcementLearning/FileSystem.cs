namespace FifteenPuzzle.Solvers.ReinforcementLearning;

public class FileSystem
{
	public virtual Stream GetFileStreamToRead(string filePath) => new FileStream(filePath, FileMode.Open, FileAccess.Read);
	public virtual Stream GetFileStreamToWrite(string filePath) => new FileStream(filePath, FileMode.Open, FileAccess.Write);
	public virtual bool FileExists(string filePath) => File.Exists(filePath);
}