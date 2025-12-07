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
    public int counter(string xPath)
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
    
    // Fonction qui, appliquant un chemin XPath à un document XML, renvoie une NodeList.
    public XmlNodeList getNodes(String xpathExpression) {
        XmlNodeList nodeList = null;
        XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new NameTable());
        namespaceManager.AddNamespace("medical", "http://www.univ-grenoble-alpes.fr/l3miage/medical");
        nodeList = doc.SelectNodes(xpathExpression, namespaceManager);
        return nodeList;
    }
    
    public bool verifierNumeroSecuriteSociale(string numero, char sexe, DateTime dateNaissance)
    {
        bool estValide = true;
        
        if (numero.Length != 15)
        {
            Console.WriteLine("Numéro de sécurité sociale invalide : longueur incorrecte.");
            estValide = false;
        }

        int sexeAttendu = sexe == 'M' ? 1 : (sexe == 'F' ? 2 : 0); 
        if (sexeAttendu == 0 || (estValide && int.Parse(numero[0].ToString()) != sexeAttendu))
        {
            Console.WriteLine("Sexe non cohérent avec le numéro de sécurité sociale.");
            estValide = false;
        }

        if (estValide)
        {
            int anneeNaissance = int.Parse(numero.Substring(1, 2));
            int moisNaissance = int.Parse(numero.Substring(3, 2));

            if (anneeNaissance != dateNaissance.Year % 100 || moisNaissance != dateNaissance.Month)
            {
                Console.WriteLine("Date de naissance non cohérente avec le numéro de sécurité sociale.");
                estValide = false;
            }
        }
        
        if (estValide)
        {
            string numeroSansCle = numero.Substring(0, 13);
            int cleDonnee = int.Parse(numero.Substring(13, 2));
            int cleCalculee = 97 - (int)(long.Parse(numeroSansCle) % 97);

            if (cleDonnee != cleCalculee)
            {
                Console.WriteLine("Clé invalide pour le numéro : " + numero + ". Clé attendue : " + cleCalculee);
                estValide = false;
            }
        }

        return estValide;
    }
    
    // verifier que l’ensemble des numéros de sécurité sociale sont valides par rapport aux informations fournies
    public bool verifierTousNumerosSecuriteSociale()
    {
        bool toutValide = true;

        XmlNodeList patients = getNodes("//medical:patients/medical:patient");

        foreach (XmlNode patient in patients)
        {
            XmlNamespaceManager nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("medical", "http://www.univ-grenoble-alpes.fr/l3miage/medical");

            string nom = patient.SelectSingleNode("medical:nom", nsManager) != null ? patient.SelectSingleNode("medical:nom", nsManager).InnerText : "Nom inconnu";
            string prenom = patient.SelectSingleNode("medical:prenom", nsManager) != null ? patient.SelectSingleNode("medical:prenom", nsManager).InnerText : "Prénom inconnu";
            string numeroSecurite = patient.SelectSingleNode("medical:numero", nsManager) != null ? patient.SelectSingleNode("medical:numero", nsManager).InnerText : "000000000000000";

            char sexe = ' ';
            XmlNode nodeSexe = patient.SelectSingleNode("medical:sexe", nsManager);
            if (nodeSexe != null && !string.IsNullOrEmpty(nodeSexe.InnerText))
                sexe = nodeSexe.InnerText[0];

            DateTime dateNaissance = DateTime.MinValue;
            XmlNode nodeNaissance = patient.SelectSingleNode("medical:naissance", nsManager);
            if (nodeNaissance != null && !string.IsNullOrEmpty(nodeNaissance.InnerText))
                dateNaissance = DateTime.Parse(nodeNaissance.InnerText);
            
            
            bool estValide = verifierNumeroSecuriteSociale(numeroSecurite, sexe, dateNaissance);

            if (!estValide)
            {
                Console.WriteLine("Numéro de sécurité sociale invalide pour " + nom + " " + prenom);
                toutValide = false;
            }
        }

        return toutValide;
    }
    // Modification de l'arbre d'instance avec DOM

    // Methode pour ajouter un infirmier
    public void ajouterInfirmier(String nom, String prenom)
    {
        XmlElement infirmier = doc.CreateElement( "infirmier", root.NamespaceURI);
        XmlNodeList infirmiers = getNodes("//medical:infirmiers/medical:infirmier");

        int maxId = 0;
        
        // Recupération de la plus grande valeur des identifiants
        
        foreach (XmlNode infirme in infirmiers)
        {
            if (infirme.Attributes != null && infirme.Attributes["id"] != null)
            {
                int id = int.Parse(infirme.Attributes["id"].Value);
                //Console.WriteLine(id);
                if (id > maxId)
                {
                    maxId = id;
                }
            }
            
        }
        int idNewInf = maxId+1;
        String idfinal = "00"+idNewInf.ToString();
        
        String photoNewInf = prenom + ".png";
        XmlElement firstName = doc.CreateElement( "nom", root.NamespaceURI);
        firstName.InnerText = nom;
        XmlElement lastName = doc.CreateElement( "prenom", root.NamespaceURI);
        lastName.InnerText = prenom;
        XmlElement photo = doc.CreateElement( "photo", root.NamespaceURI);
        photo.InnerText = photoNewInf;

        // Creation de l'infirmier
        
        infirmier.SetAttribute("id", idfinal);
        infirmier.AppendChild(firstName);
        infirmier.AppendChild(lastName);
        infirmier.AppendChild(photo);
        
        // Ajout de l'infirmier dans l'element infirmiers


        var listeInfirmiers = ((XmlElement)root).GetElementsByTagName("infirmiers").Item(0);
        if (listeInfirmiers != null)
        {
           
            listeInfirmiers.AppendChild(infirmier);
            Console.WriteLine("Ajout reussi !!");
            
        }
        else
        {
            Console.WriteLine("Echec d'ajout !!");
        }
    }
    
    // Methode pour ajouter un client
    public void ajouterPatient(String nom, String prenom, String sexe, String dateNaisse, string NSS, string numRue, String rue, String codePostal, String ville)
    {
        XmlElement patient = doc.CreateElement( "patient", root.NamespaceURI);
        XmlElement firstName = doc.CreateElement( "nom", root.NamespaceURI);
        firstName.InnerText = nom;
        XmlElement lastName = doc.CreateElement( "prenom", root.NamespaceURI);
        lastName.InnerText = prenom;
        XmlElement genre = doc.CreateElement("sexe", root.NamespaceURI);
        genre.InnerText = sexe;
        XmlElement Naissance = doc.CreateElement( "naissance", root.NamespaceURI);
        Naissance.InnerText = dateNaisse;
        XmlElement numSecu = doc.CreateElement( "numero", root.NamespaceURI);
        numSecu.InnerText = NSS;
        XmlElement numR = doc.CreateElement("numero", root.NamespaceURI);
        numR.InnerText = numRue;
        XmlElement RUE = doc.CreateElement( "rue", root.NamespaceURI);
        RUE.InnerText = rue;
        XmlElement postal = doc.CreateElement( "codePostal", root.NamespaceURI);
        postal.InnerText = codePostal;
        XmlElement VILLE = doc.CreateElement( "ville", root.NamespaceURI);
        VILLE.InnerText = ville;
        XmlElement adr = doc.CreateElement( "adresse", root.NamespaceURI);
        
        
        // Création de l'adresse 
        
        adr.AppendChild(numR);
        adr.AppendChild(RUE);
        adr.AppendChild(postal);
        adr.AppendChild(VILLE);
        
        // Creation du patient

        patient.AppendChild(firstName);
        patient.AppendChild(lastName);
        patient.AppendChild(genre);
        patient.AppendChild(Naissance);
        patient.AppendChild(numSecu);
        patient.AppendChild(adr);
        
        // Ajout du patient dans l'element patients
        
        var listesPatients = ((XmlElement)root).GetElementsByTagName("patients").Item(0);
        if (listesPatients != null)
        {
            Console.WriteLine("Ajout reussi !!");
            listesPatients.AppendChild(patient);
        }
        else
        {
            Console.WriteLine("Echec d'ajout !!");
        }
    }
    
    // Methode pour ajouter une visite à un patient
     public void ajouterVisite(String numeroSecu, String idActe, String numeroIntervenant, String dateVisite)
    {
        XmlNamespaceManager nsManager = new XmlNamespaceManager(new NameTable());
        nsManager.AddNamespace("medical", "http://www.univ-grenoble-alpes.fr/l3miage/medical");
        XmlNode patientAvisiter = doc.SelectSingleNode($"//medical:patients/medical:patient[medical:numero='{numeroSecu}']", nsManager);
                                                                                                                                     // de Secu passe en paramétre
        XmlNodeList infirmiers = getNodes("//medical:infirmiers/medical:infirmier");
        
        Boolean intervExiste = false;
        
       // Verifie si le numero d'intervenant passer en paramétre correspond à un infirmier

       int i = 0;
       while (i < infirmiers.Count && !intervExiste)
       {
           XmlNode infirm = infirmiers[i];
           
           if (infirm.Attributes != null && infirm.Attributes["id"] != null)
           {
               if (infirm.Attributes["id"].Value ==numeroIntervenant)
               {
                   intervExiste = true;
               }
           }

           i = i + 1;
       }
       // verie si l'idActe passer en paramétre est correcte

       XmlDocument actesDoc = new XmlDocument();
       actesDoc.Load("data/xml/actes.xml");
       
       XmlNamespaceManager namespaceManage = new XmlNamespaceManager(doc.NameTable);
       namespaceManage.AddNamespace("act","http://www.univ-grenoble-alpes.fr/l3miage/actes");
       
       XmlNodeList actes = actesDoc.SelectNodes("//act:actes/act:acte",namespaceManage);
       int j = 0;
       Boolean acteExiste = false;
       while (j < actes.Count && !acteExiste)
       {
           XmlNode acte = actes[i];
           
           
           if (acte.Attributes != null && acte.Attributes["id"] != null)
           {

               if (acte.Attributes["id"].Value == idActe)
               {

                   acteExiste = true;
               }
           }

           i = i + 1;
       }

       if (intervExiste && acteExiste && patientAvisiter != null)
       {
           XmlElement Visite = doc.CreateElement( "visite", root.NamespaceURI);
           XmlElement Acte = doc.CreateElement("acte", root.NamespaceURI);
           
           Acte.SetAttribute("id", idActe);
           
           
           // Creation de la visite

           Visite.SetAttribute("date",dateVisite);
           Visite.SetAttribute("intervenant",numeroIntervenant);
           Visite.AppendChild(Acte);
           
           Console.WriteLine("La visite :"+Visite.InnerXml);
           // Ajout de la visite pour le patient concerné

           patientAvisiter.AppendChild(Visite);
           Console.WriteLine("Ajout effectuée avec succés");
       }
       else
       {
           Console.WriteLine("Echec d'ajout de la visite pour le patient !! Verifier que les informations fournies sont correctes");
       }
       



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
            
            Console.WriteLine("Déserialisation effectuée avec succées !");

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

    // Methode pour visualiser le document xml
    public string toStringDOM()
    {
        return doc.InnerXml;
    }
}