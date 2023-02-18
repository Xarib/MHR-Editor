// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using JsonDumper.DataReader;
using JsonDumper.GameData;

const string FILE_PATH = @"D:\dump.json";

var dataReaders = new List<IDataReader>()
{
    new GreatSwordReader(),
};

var dataDump = dataReaders
    .SelectMany(reader => reader.GetData())
    .ToDictionary(data => data.Id);

var file = File.OpenWrite(FILE_PATH);
JsonSerializer.Serialize(file, dataDump, new JsonSerializerOptions
{
    WriteIndented = true,
});
file.Close();

Console.WriteLine("File Written");