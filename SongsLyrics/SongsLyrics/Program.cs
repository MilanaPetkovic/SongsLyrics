﻿using System.Configuration;
using System.Net;
using System.Text;

Console.Write("Enter an artist name: ");
string artisName = Console.ReadLine();

Console.Write("Enter a song name: ");
string songName = Console.ReadLine();


var request = WebRequest.Create($"https://api.lyrics.ovh/v1/{artisName}/{songName}");
request.ContentType = "application/json; charset=utf-8";

string text;
var response = (HttpWebResponse)request.GetResponse();

using (var sr = new StreamReader(response.GetResponseStream()))
{
    text = sr.ReadToEnd();
}

text = text.Split("\"lyrics\":")[1]
    .Replace("}", "")
    .Replace("\\\"", "\"");
    //.Replace("\"", "");
text = text.Replace("\\n", Environment.NewLine).Replace("\\r", Environment.NewLine);


var outputDir = ConfigurationManager.AppSettings["Path"];

if (!Directory.Exists(outputDir))
    Directory.CreateDirectory(outputDir);

var outputFile = Path.Combine(outputDir, $"{artisName}_{songName}.txt");
File.WriteAllText(outputFile, text);
var stop = "";