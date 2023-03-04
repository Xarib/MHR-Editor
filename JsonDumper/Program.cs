// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using JsonDumper.DataReader;
using MHR_Editor.Data;

// This is a need hidden dependency
Console.WriteLine("Start init...");
DataInit.Init();
Console.WriteLine("Done init");

Console.WriteLine("Dumping json...");

var dataReaders = new List<IDataReader>()
{
    new BowReader(),
    new ChargeAxeReader(),
    new GreatSwordReader(),
    new DualBladesReader(),
    new GunLanceReader(),
    new HammerReader(),
    new HeavyBowgunReader(),
    new HornReader(),
    new InsectGlaiveReader(),
    new LanceReader(),
    new LightBowgunReader(),
    new LongSwordReader(),
    new ShortSwordReader(),
    new SlashAxeReader(),
    new InsectReader(),
    new ArmorReader(),
    new PetalaceReader(),
};

DumpFiles("main", dataReaders);
DumpFiles("decorations", new List<IDataReader>()
{
    new DecorationReader(),
    new RampageDecorationReader(),
});

static void DumpFiles(string fileName, IList<IDataReader> dataReaders)
{
    var fileBasePath = @"D:\mhrData\" + fileName;
    var filePath = fileBasePath + ".json";
    var filePathPretty = fileBasePath + "Pretty.json";
    
    File.Delete(filePath);
    File.Delete(filePathPretty);
    
    var dataDump = new Dictionary<long, object>();
    // Ignore convert to LINQ
    foreach (var data in dataReaders.SelectMany(reader => reader.GetData()))
    {
        dataDump.Add(data.Id, data);
    }

    var file = File.OpenWrite(filePath);
    JsonSerializer.Serialize(file, dataDump);
    file.Close();

    var filePretty = File.OpenWrite(filePathPretty);
    JsonSerializer.Serialize(filePretty, dataDump, new JsonSerializerOptions
    {
        WriteIndented = true,
    });
    filePretty.Close();

    Console.WriteLine($"'{fileName}': Files Written");
}