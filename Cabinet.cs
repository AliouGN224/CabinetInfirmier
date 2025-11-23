using System.Xml;

namespace CabinetInfirmier;

public class Cabinet
{
    private XmlDocument doc;
    private XmlNode root;
    private string nom;
    private Adresse adresse;

    public Cabinet(string filename)
    {
        this.doc = new XmlDocument();
        this.doc.Load(filename);
        this.root = this.doc.DocumentElement;
    }

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
    // récupére toutes les noeuds correspondant à xpath
    public XmlNodeList getElementsNodeList(string xPath)
    {
        XmlNodeList nodeList = null;
        XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new NameTable());
        namespaceManager.AddNamespace("medical", "http://www.univ-grenoble-alpes.fr/l3miage/medical");
        nodeList = doc.SelectNodes(xPath, namespaceManager);
        return nodeList;
    }

    // Compte le nombre d'élement correspondant à xpath
    public int count(string xPath)
    {
        XmlNodeList nodeList = this.getElementsNodeList(xPath);
        return nodeList.Count;
    }
     // Verifie si l'adresse du cabinet est compléte
    public bool isCabinetAdresseCompleted()
    {
        XmlNodeList nodeList = this.getElementsNodeList("//medical:cabinet/medical:adresse");
        bool isTrue = false;
        if (nodeList.Item(0).ChildNodes.Count == 4)
        {
            isTrue = true;
        }
        return isTrue;
    }
    // Verifie si toutes les adresses des patients sont corrects
    public bool isAllAdressPatientCompleted()
    {
        bool isTrue = true;
        XmlNodeList clientNodeList = getElementsNodeList("medical:cabinet/medical:patients/medical:patient");
        foreach (XmlNode node in clientNodeList)
        {
            if (node.ChildNodes.Item(5).ChildNodes.Count < 3)
            {
                isTrue = false;
            }
        }
        return isTrue;
    }
    
    
    
}