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

    }
}