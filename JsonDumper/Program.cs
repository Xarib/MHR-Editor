﻿// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using JsonDumper.DataReader;
using MHR_Editor.Data;

// This is a need hidden dependency
Console.WriteLine("Start init...");
DataInit.Init();
Console.WriteLine("Done init");

Console.WriteLine("Dumping json...");

const string FILE_PATH = @"D:\dump.json";

File.Delete(FILE_PATH);

var dataReaders = new List<IDataReader>()
{
    //new BowReader(),
    //new ChargeAxeReader(),
    //new GreatSwordReader(),
    //new DualBladesReader(),
    //new GunLanceReader(),
    //new HammerReader(),
    //new HeavyBowgunReader(),
    //new HornReader(),
    //new InsectGlaiveReader(),
    //new LanceReader(),
    //new LightBowgunReader(),
    //new LongSwordReader(),
    //new ShortSwordReader(),
    //new InsectReader(),
    new ArmorReader(),
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