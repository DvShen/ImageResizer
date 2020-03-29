using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ImageResizer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            #region 等候使用者輸入 取消 c 按鍵
            ThreadPool.QueueUserWorkItem(x =>
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.C)
                {
                    cts.Cancel();
                }
            });
            #endregion


            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output");

            //ImageProcess imageProcess = new ImageProcess();
            //var R1 = RunImageProcess(ref imageProcess, sourcePath, destinationPath);
            var R2 = await RunImageProcessAsync(sourcePath, destinationPath, cts.Token);
            //decimal R = (decimal)((decimal)(R1 - R2) / (R1) * 100);

            //Console.WriteLine($"花費時間: {R1} ms");
            Console.WriteLine($"Async 花費時間: {R2} ms");
            //Console.WriteLine($"進步: {R.ToString("0.00")} %");
        }

        static private long RunImageProcess(ref ImageProcess imageProcess, string sourcePath, string destinationPath)
        {
            imageProcess.Clean(destinationPath);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            imageProcess.ResizeImages(sourcePath, destinationPath, 2.0);
            sw.Stop();

            return sw.ElapsedMilliseconds;
        }

        static private long RunImageProcessAsync(ref ImageProcess imageProcess, string sourcePath, string destinationPath)
        {
            imageProcess.Clean(destinationPath);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            imageProcess.ResizeImagesAsync(sourcePath, destinationPath, 2.0, CancellationToken.None).Wait();
            sw.Stop();

            return sw.ElapsedMilliseconds;
        }

        static private async Task<long> RunImageProcessAsync(string sourcePath, string destinationPath, CancellationToken token)
        {
            ImageProcess imageProcess = new ImageProcess();
            imageProcess.Clean(destinationPath);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            await imageProcess.ResizeImagesAsync(sourcePath, destinationPath, 2.0, token);
            sw.Stop();

            return sw.ElapsedMilliseconds;
            //return Task.FromResult(sw.ElapsedMilliseconds);
        }
    }
}
