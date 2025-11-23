using System.Xml;

namespace CabinetInfirmier;

public class Cabinet
{
    private string nom;
    private Adresse adresse;

    public void AnalyseGlobale(string filepath)
    {
        XmlReader reader = XmlReader.Create(filepath);

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Document :
                    Console.WriteLine("Entrée dans le document");
                    break;
                case XmlNodeType.Element:
                    Console.WriteLine("Entrée dans l'element " + reader.Name + ". Contient " + reader.AttributeCount + " attributs");
                    break;
                case XmlNodeType.Text:
                    break;
                case XmlNodeType.EndElement:
                    Console.WriteLine("Sortie dans l'element " + reader.Name + ". Contient " + reader.AttributeCount + " attributs");
                    break;
                case XmlNodeType.Attribute:
                    break;
            }
        }
    }
}