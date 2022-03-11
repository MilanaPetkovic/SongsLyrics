using System.Configuration;
using System.Net;

try
{
    Console.Write("Enter an artist name: ");
    string artisName = Console.ReadLine();

    Console.Write("Enter a song name: ");
    string songName = Console.ReadLine();

    var request = WebRequest.Create($"https://api.lyrics.ovh/v1/{artisName}/{songName}");
    request.ContentType = "application/json; charset=utf-8";

    string text;
    var response = await request.GetResponseAsync();

    using (var sr = new StreamReader(response.GetResponseStream()))
    {
        text = await sr.ReadToEndAsync();
    }

    text = text.Split("\"lyrics\":")[1]
        .Replace("}", "")
        .Replace("\\\"", "\"")
        .Replace("\\n", Environment.NewLine)
        .Replace("\\r", Environment.NewLine);

    var outputDir = ConfigurationManager.AppSettings["SavePath"];

    if (!Directory.Exists(outputDir))
        Directory.CreateDirectory(outputDir);

    var outputFile = Path.Combine(outputDir, $"{artisName}_{songName}.txt");
    File.WriteAllText(outputFile, text);
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}
