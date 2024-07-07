
internal class Program
{
    private static async Task Main(string[] args)
    {
        string[] fileUrls =
            { "https://picsum.photos/200/300",
                "https://picsum.photos/300/200",
                "https://picsum.photos/400/400"};
        await DownloadImagesAsync(fileUrls);
    }
    public static async Task DownloadImagesAsync(string[] urls)
    {
        List<Task> tasks = new List<Task>();
        for (int i = 0; i < urls.Length; i++) 
        {
            string url = urls[i];
            int index = i;
            tasks.Add(DownloadImageAsync(url, index + 1));
        }
        Task.WaitAll(tasks.ToArray());
    }
    public static async Task DownloadImageAsync(string url, int index)
    {
        using HttpClient client = new HttpClient();
        using HttpResponseMessage response =
            await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

        using Stream stream = await response.Content.ReadAsStreamAsync();
        byte[] buffer = new byte[1024];
        long totalBytesRead = 0;
        long fileSize = (long)response.Content.Headers.ContentLength;
        int lastReportedProgress = -1;

        while (totalBytesRead < fileSize)
        {
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            totalBytesRead += bytesRead;
            int progress = (int)((double)totalBytesRead / fileSize * 100);
            if (progress != lastReportedProgress)
            {
                Console.WriteLine($"Downloading image {index} : {progress}%");
                lastReportedProgress = progress;
            }
        }
        Console.WriteLine($"Download image {index} completed.");
    }
}