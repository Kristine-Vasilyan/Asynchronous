using System;
using System.ComponentModel;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        string programDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "AsynchronousHomework.txt";
        string filePath = System.IO.Path.Combine(programDirectory, fileName);
        ReadFileAsync(filePath);
    }
    static void ReadFileAsync(string filePath)
    {
        BackgroundWorker worker = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        worker.DoWork += (sender, e) =>
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] buffer = new byte[4096];
            long totalBytesRead = 0;
            long fileSize = fileStream.Length;
            int lastReportedProgress = -1;
            int wordCount = 0;
            while (totalBytesRead < fileSize)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                totalBytesRead += bytesRead;
            }
            string text = UTF8Encoding.UTF8.GetString(buffer);
            int analyzedIndex = 0;
            int part = 100;
            lastReportedProgress = -1;
            int textsize = text.Length;
            while(analyzedIndex + part < textsize) 
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                int start = analyzedIndex + part;
                int end = Math.Min(start + part, textsize);
                int words = CountWord(text, start, end);
                analyzedIndex += part;
                wordCount += words;

                int progress = (int)((double) end/ textsize * 100);
                if (progress != lastReportedProgress)
                {
                    worker.ReportProgress(progress);
                    Thread.Sleep(50);
                    lastReportedProgress = progress;
                }
            }
            Console.WriteLine($"File analysis completed. Total word count: {wordCount}");
            fileStream.Close();
        };

        worker.ProgressChanged += (sender, e) =>
        {
            Console.WriteLine($"Analyze {e.ProgressPercentage}%...");
        };

        worker.RunWorkerCompleted += (sender, e) =>
        {
            if (e.Cancelled)
            {
                Console.WriteLine("File read cancelled.");
            }
            else if (e.Error != null)
            {
                Console.WriteLine($"Error: {e.Error.Message}");
            }
            else
            {
               // Console.WriteLine("File read completed.");
            }
        };

        worker.RunWorkerAsync();
        Console.WriteLine("Reading file...");

        // Keep the main thread alive to receive progress updates
        Console.ReadLine();
    }
    static int CountWord(string text, int start, int end)
    {
        string part = text.Substring(start, end - start);
        var words = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }
}