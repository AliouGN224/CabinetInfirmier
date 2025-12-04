// See https://aka.ms/new-console-template for more information

using CabinetInfirmier;
using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        /*await XMLUtils.ValidateXmlFileAsync(
            "http://www.univ-grenoble-alpes.fr/l3miage/medical",
            "data/xsd/cabinet.xsd",
            "data/xml/cabinet.xml"
        );*/
        
        /*await XMLUtils.ValidateXmlFileAsync(
            "http://www.univ-grenoble-alpes.fr/l3miage/actes",
            "data/xsd/actes.xsd",
            "data/xml/actes.xml"
        );*/
        
        /*XMLUtils.XslTransform(
            "../../../data/xml/cabinet.xml", 
            "../../../data/xslt/Orouge.xslt", 
            "../../../data/xml/Orouge.xml",
            "nomPatient",
            "Kapoëtla"); */

        // ======> Test de la methode AnalyseGlobale(stringfilepath)
        //Cabinet.AnalyseGlobale("../../../data/xml/cabinet.xml");  // Test de la methode AnalyseGlobale(stringfilepath)
        
        // ======> Test récupération de texte d'éléments particuliers
        /*foreach (var n in Cabinet.RecupererElementsFiltres("./data/xml/cabinet.xml", "nom", "patient")){
            Console.WriteLine(n);
            Console.WriteLine("---------------");
        }*/
        
        // ======> Test compte combien d’actes différents ont été effectués
        //Console.WriteLine("Nombre actes effectués = " + Cabinet.CompterDifferentsActesEffectues("./data/xml/cabinet.xml")); // 6
        
        
        Cabinet cb = new Cabinet("data/xml/cabinet.xml");
        Console.WriteLine("Le nombre est : "+cb.isAllAdressPatientCompleted());
        
        Infirmiers inf = new Infirmiers();
        inf.DeserialiserInf("data/xml/infirmiers.xml"); 
        Console.WriteLine(inf.toString());
        Infirmier inf1 = new Infirmier();
        inf1._Id = "006";
        inf1._Nom = "TenHAG";
        inf1._Prenom = "Darmanan";
        inf1._Photo = "darmanan.png";
        Infirmier inf2 = new Infirmier();
        inf2._Id = "007";
        inf2._Nom = "DIAMILA";
        inf2._Prenom = "Beyrou";
        inf2._Photo = "beyrou.png";
        List<Infirmier> list = new List<Infirmier>();
        list.Add(inf1);
        list.Add(inf2);

         Infirmiers inf3 = new Infirmiers
         {
             _ListInfirmiers = new List<Infirmier>
             {
                 inf1,inf2
             }
         };
         inf3.SerialiserInf("../../../data/xml/infirmiers1.xml");
         
         
         Console.WriteLine("_____________________________Serialisation du cabinet______________________________________________");
         Cabinet cb2 = new Cabinet();
         cb2.DeserialiserCabinet("data/xml/cabinet.xml");
         Console.WriteLine(cb2.ToString());
         
         
        Console.WriteLine("_______________________________Désérialisation du cabinet___________________________________________");
        Acte acte = new Acte{_Id = 109};
        Acte acte2 = new Acte{_Id = 101};
        Acte acte3 = new Acte{_Id = 102};
        Visite v1 = new Visite { _Intervenant = "001", _Date = "2015-02-11", _Actes = new List<Acte> { acte, acte2, acte3 } };
        Visite v2 = new Visite { _Intervenant = "003", _Date = "2015-03-21", _Actes = new List<Acte> {  acte2, acte3 } };
        Visite v3 = new Visite { _Intervenant = "003", _Date = "2015-05-11", _Actes = new List<Acte> { acte, acte2 } };
        
        Adresse adresse = new Adresse{_Etage = 9,_Numero = 20,_Rue = "Rue Anthoard", _CodePostal = "38000",_Ville = "Grenoble"};
        Adresse adresse1 = new Adresse{_Etage = 2,_Numero = 75,_Rue = "Gabriel Peri", _CodePostal = "38400",_Ville = "Saint Martin d'heres"};
        Adresse adresse3 = new Adresse{_Etage = 7,_Numero = 20,_Rue = "Rue Anthoard", _CodePostal = "38000",_Ville = "Grenoble"};
        
        


        



    }
}