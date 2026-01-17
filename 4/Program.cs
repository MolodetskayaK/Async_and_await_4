using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Замени на реальный путь к твоей папке с файлами! (создай папку и добавь .txt файлы)
        string folderPath = @"C:\Users\Cocosik\Documents\TestFolder";

        try
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Папка '{folderPath}' не найдена. Создай её и добавь файлы.");
                return;
            }

            string[] files = Directory.GetFiles(folderPath);
            if (files.Length == 0)
            {
                Console.WriteLine($"Папка '{folderPath}' пуста. Добавь текстовые файлы для теста.");
                return;
            }

            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            long totalSpaces1 = await CountSpacesScenario1(folderPath);
            sw1.Stop();
            Console.WriteLine($"Scenario 1: Total spaces = {totalSpaces1}, Time = {sw1.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в Scenario 1: {ex.Message}");
        }

        try
        {
            Stopwatch sw2 = new Stopwatch();
            sw2.Start();
            long totalSpaces2 = await CountSpacesScenario2(folderPath);
            sw2.Stop();
            Console.WriteLine($"Scenario 2: Total spaces = {totalSpaces2}, Time = {sw2.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в Scenario 2: {ex.Message}");
        }
    }

    static async Task<long> CountSpacesScenario1(string folderPath)
    {
        string[] files = Directory.GetFiles(folderPath);
        var tasks = files.Select(file => Task.Run(() => CountSpacesInFile(file))).ToArray();
        long[] results = await Task.WhenAll(tasks);
        return results.Sum();
    }

    static async Task<long> CountSpacesScenario2(string folderPath)
    {
        string[] files = Directory.GetFiles(folderPath);
        var fileTasks = files.Select(file => Task.Run(async () =>
        {
            string[] lines = File.ReadAllLines(file);
            var lineTasks = lines.Select(line => Task.Run(() => (long)line.Count(c => c == ' '))).ToArray();
            long[] lineResults = await Task.WhenAll(lineTasks);
            return lineResults.Sum();
        })).ToArray();
        long[] fileResults = await Task.WhenAll(fileTasks);
        return fileResults.Sum();
    }

    static long CountSpacesInFile(string filePath)
    {
        string content = File.ReadAllText(filePath);
        return content.Count(c => c == ' ');
    }
}