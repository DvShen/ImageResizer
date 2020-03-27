using System;
using System.Diagnostics;
using System.IO;

namespace ImageResizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = Path.Combine(Environment.CurrentDirectory, "images");
            string destinationPath = Path.Combine(Environment.CurrentDirectory, "output");

            ImageProcess imageProcess = new ImageProcess();
            var R1 = RunImageProcess(ref imageProcess, sourcePath, destinationPath);
            var R2 = RunImageProcessAsync(ref imageProcess, sourcePath, destinationPath);
            decimal R = (decimal)((decimal)(R1 - R2) / (R1) * 100);

            Console.WriteLine($"花費時間: {R1} ms");
            Console.WriteLine($"Async 花費時間: {R2} ms");
            Console.WriteLine($"進步: {R.ToString("0.00")} %");
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
            imageProcess.ResizeImagesAsync(sourcePath, destinationPath, 2.0).Wait();
            sw.Stop();

            return sw.ElapsedMilliseconds;
        }
    }
}
