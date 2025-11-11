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
        
        XMLUtils.XslTransform(
            "../../../data/xml/cabinet.xml", 
            "../../../data/xslt/infirmier.xslt", 
            "../../../data/html/infirmier.html");
    }
}