// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using JsonDumper.DataReader;
using MHR_Editor.Data;

// This is a need hidden dependency
DataInit.Init();

const string FILE_PATH = @"D:\dump.json";

File.Delete(FILE_PATH);

var dataReaders = new List<IDataReader>()
{
    //new BowReader(),
    //new ChargeAxeReader(),
    //new GreatSwordReader(),
    //new DualBladesReader(),
    //new GunLanceReader(),
    new HammerReader(),
};

var dataDump = new Dictionary<long, object>();

// Ignore convert to LINQ
foreach (var data in dataReaders.SelectMany(reader => reader.GetData()))
{
    dataDump.Add(data.Id, data);
}

var file = File.OpenWrite(FILE_PATH);
JsonSerializer.Serialize(file, dataDump, new JsonSerializerOptions
{
    WriteIndented = true,
});
file.Close();

Console.WriteLine("File Written");