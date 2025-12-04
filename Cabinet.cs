using System.Xml;
using System.Xml.Serialization;

namespace CabinetInfirmier;
[XmlRoot("cabinet",Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Cabinet
{
    private String _nom;

    [XmlElement("nom")]
    public String _Nom
    {
        get => _nom;
        set => _nom = value;
    }
    private Adresse _adresse;

    [XmlElement("adresse")]
    public Adresse _Adresse
    {
        get => _adresse;
        set => _adresse = value;
    }
    private Infirmiers _infirmiers;

    [XmlElement("infirmiers")]
    public Infirmiers _Infirmiers
    {
        get => _infirmiers;
        set => _infirmiers = value;
    }
    private Patients _patients;

    [XmlElement("patients")]
    public Patients _Patients
    {
        get => _patients;
        set => _patients = value;
    }

    public Cabinet() {}
    
    
    
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
    
    public static void AnalyseGlobale(string filepath)
    {
        XmlReader reader = XmlReader.Create(filepath);

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Depth == 0) {
                        Console.WriteLine("=> Entree dans le document");
                    }
                    Console.WriteLine("===> Entrée dans l'element " + reader.Name + ". Contient " + reader.AttributeCount + " attribut(s)");
                    
                    if (reader.HasAttributes) {  
                        for (int i = 0; i < reader.AttributeCount; i++) {
                            reader.MoveToAttribute(i);
                            Console.WriteLine("=====> Attribut " + (i+1) + " : " + reader.Name+  " = " + reader.Value);
                        }
                        
                        reader.MoveToElement();
                    }
                    
                    break;
                case XmlNodeType.Text:
                    Console.WriteLine("-------> Texte : " + reader.Value);
                    break;
                case XmlNodeType.EndElement:
                    Console.WriteLine("===> Sortie de l'element " + reader.Name);
                    break;
            }
        }
    }
    
    
    // récupération de texte d'éléments particuliers
    /*public static List<string> RecupererElements(string filepath, string elementRecherche)
    {
        List<string> resultats = new List<string>();

        XmlReader reader = XmlReader.Create(filepath);
        bool insideTargetElement = false;

        while (reader.Read()){
            switch (reader.NodeType){
                case XmlNodeType.Element:
                    if (reader.Name == elementRecherche) {
                        insideTargetElement = true;
                    }
                    break;

                case XmlNodeType.Text:
                    if (insideTargetElement) {
                        resultats.Add(reader.Value.Trim());
                    }
                    break;
                case XmlNodeType.EndElement:
                    if (reader.Name == elementRecherche) {
                        insideTargetElement = false;
                    }
                    break;
            }
        }

        return resultats;
    }*/
    
    // récupération de texte d'éléments particuliers
    public static List<string> RecupererElementsFiltres(string filepath, string elementRecherche, string? parentRecherche = null){
        List<string> resultats = new List<string>();
        Stack<string> pile = new Stack<string>();

        using XmlReader reader = XmlReader.Create(filepath);

        while (reader.Read()) {
            switch (reader.NodeType) {
                case XmlNodeType.Element:
                    pile.Push(reader.Name); 
                    break;

                case XmlNodeType.Text:
                    if (pile.Count >= 1 && pile.Peek() == elementRecherche) {
                        // Si pas de parent demande on prend tout
                        if (parentRecherche == null) {
                            resultats.Add(reader.Value.Trim());
                        }else if (pile.Count >= 2) {
                            string parent = pile.Skip(1).First();

                            if (parent == parentRecherche) {
                                resultats.Add(reader.Value.Trim());
                            }
                        }
                    }
                    break;

                case XmlNodeType.EndElement:
                    // en quittant une balise on enlève de la pile
                    if (pile.Count > 0)
                        pile.Pop();
                    break;
            }
        }

        return resultats;
    }

    // compte combien d’actes différents ont été effectués
    public static int CompterDifferentsActesEffectues(string filepath)
    {
        HashSet<string> actesUniques = new HashSet<string>();

        using (XmlReader reader = XmlReader.Create(filepath)){
            while (reader.Read())
            {
                // On détecte un élément acte
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "acte") {
                    if (reader.HasAttributes) {
                        string id = reader.GetAttribute("id");
                        if (!string.IsNullOrEmpty(id))
                        {
                            actesUniques.Add(id);
                        }
                    }
                }
            }
        }

        return actesUniques.Count;
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
    
    
    // Les méthodes pour serialiser et deserialiser un cabinet 
    
    public void DeserialiserCabinet(String path)
    {
        using (TextReader reader = new StreamReader(path))
        {
            var xmlCabinet = new XmlSerializer(typeof(Cabinet));
            var deserialized = (Cabinet)xmlCabinet.Deserialize(reader);
            this._nom = deserialized._nom;
            this._adresse = deserialized._adresse;
            this._infirmiers = deserialized._infirmiers;
            this._patients = deserialized._patients;

        }
    }
    
    public void SerialiserCabinet(String path)
    {
        
        using (var writer = new StreamWriter(path))
        {
            var xmlCabinet = new XmlSerializer(typeof(Cabinet));
            xmlCabinet.Serialize(writer, this);
            Console.WriteLine("Serialisation effectué avec succés");
        } 
    }
    
    
    // Methode string pour afficher toutes les informations du cabinet

    public override string ToString()
    {
        String s ="Nom du cabinet : "+this._Nom+"\nAdresse du cabinet : "+this._Adresse._Numero+" "+this._Adresse._Rue+" "+this._Adresse._CodePostal+" "+this._Adresse._Ville;
        // recuperer les informations des infirmier

        s+= "\n _____La liste des infirmiers______ ";
        s += _Infirmiers.toString();
        s += _Patients.ToString();
        
       
        return s;
    }
}