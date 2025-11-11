// See https://aka.ms/new-console-template for more information

using CabinetInfirmier;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        await XMLUtils.ValidateXmlFileAsync(
            "http://www.univ-grenoble-alpes.fr/l3miage/medical",
            "data/xsd/cabinet.xsd",
            "data/xml/cabinet.xml"
        );
    }
}