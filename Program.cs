using System.Diagnostics;

namespace ParallelTask
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var testFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TestFiles");

            await ProcessFolderAsync(testFolder);
        }

        static async Task ProcessFolderAsync(string pathDir)
        {
            if (!Directory.Exists(pathDir))
            {
                Console.WriteLine("Folder not found");

                return;
            }

            var files = Directory.GetFiles(pathDir);

            if (files.Length == 0)
            {
                Console.WriteLine("Empty folder");

                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Task<int>> tasks = new();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(() =>
                {
                    string text = File.ReadAllText(file);

                    return text.Count(c => c == ' ');
                }));
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"File No.{i + 1}: {tasks[i].Result} spaces");
            }

            Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
