using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DockerStartTimer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string dockerCommand = args[0];
            string url="";
            int width = 30;
            if (args.Length > 1)
            {
                url = args[1];
            }
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("Starting the container: docker " + dockerCommand);
            sw.Start();
            RunProcess runProcess = new RunProcess();
            Process dockerProcess = runProcess.Run("docker", dockerCommand);
            sw.Stop();
            Console.WriteLine("Container Start Minutes".PadRight(width) + ": " + sw.Elapsed.Minutes.ToString());
            Console.WriteLine("Container Start Seconds".PadRight(width) + ": " + sw.Elapsed.Seconds.ToString());
            Console.WriteLine("Container Start Milliseconds".PadRight(width) + ": " + sw.Elapsed.Milliseconds.ToString());

            if (url.Length > 0)
            {
                HttpClient client = new HttpClient();
                HttpRequestMessage request;
                bool waitForResult = true;
                Console.WriteLine();
                Console.WriteLine("Requesting url: " + url);
                sw.Start();
                while (waitForResult)
                {
                    try
                    {
                        request = new HttpRequestMessage(HttpMethod.Get, url);
                        var requestResult = client.SendAsync(request).Result;
                        if (requestResult.StatusCode == HttpStatusCode.OK)
                        {
                            waitForResult = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(".");
                    }
                }
            }
            sw.Stop();
            dockerProcess.Dispose();
            Console.WriteLine();
            Console.WriteLine("TotalMinutes".PadRight(width) + ": " + sw.Elapsed.Minutes.ToString());
            Console.WriteLine("TotalSeconds".PadRight(width) + ": " + sw.Elapsed.Seconds.ToString());
            Console.WriteLine("TotalMilliseconds".PadRight(width) + ": " + sw.Elapsed.Milliseconds.ToString());
            //dockerCommand = "rm -f $(docker ps -q)";
            //Console.WriteLine("Stopping all active containers: docker " + dockerCommand);
            //RunProcess killContainers = new RunProcess();
            //Process killContainerProcess = killContainers.Run("docker", dockerCommand);
            //killContainerProcess.Dispose();
        }
    }
}
