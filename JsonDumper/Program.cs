// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using JsonDumper.DataReader;
using MHR_Editor.Common;
using MHR_Editor.Common.Data;
using MHR_Editor.Data;

Console.WriteLine("Start init...");
// This is needed
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
});
DumpFiles("rampage_decorations", new List<IDataReader>()
{
    new RampageDecorationReader(),
});
DumpSkillsNames("skill_names", DataHelper.SKILL_NAME_LOOKUP);
DumpSkillsNames("rampage_skill_names", DataHelper.RAMPAGE_SKILL_NAME_LOOKUP);

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
    JsonSerializer.Serialize(file, dataDump, new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    });
    file.Close();

    var filePretty = File.OpenWrite(filePathPretty);
    JsonSerializer.Serialize(filePretty, dataDump, new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    });
    filePretty.Close();

    Console.WriteLine($"'{fileName}': Files Written");
}

static void DumpSkillsNames(string fileName, Dictionary<Global.LangIndex, Dictionary<uint, string>> names)
{
    var file = File.OpenWrite($@"D:\mhrData\{fileName}.json");
    JsonSerializer.Serialize(file, names[Global.LangIndex.eng], new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    });
    file.Close();
}