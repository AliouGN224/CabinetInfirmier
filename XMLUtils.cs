using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace CabinetInfirmier;

public static class  XMLUtils {
    public static async Task ValidateXmlFileAsync(string schemaNamespace, string xsdFilePath, string xmlFilePath)
    {
        var settings = new XmlReaderSettings();
        settings.Schemas.Add(schemaNamespace, xsdFilePath);
        settings.ValidationType = ValidationType.Schema;
        Console.WriteLine("Nombre de schemas utilisés dans la validation : " + settings.Schemas.Count);
        settings.ValidationEventHandler += ValidationCallBack;
        var readItems = XmlReader.Create(xmlFilePath, settings);
        while (readItems.Read()) { }
    }

    private static void ValidationCallBack(object? sender, ValidationEventArgs e)
    {
        if (e.Severity.Equals(XmlSeverityType.Warning))
        {
            Console.Write("WARNING : ");
            Console.WriteLine(e.Message);
        }
        else if (e.Severity.Equals(XmlSeverityType.Error))
        {
            Console.Write("ERROR : ");
            Console.WriteLine(e.Message);
        }
    }
    
    public static void XslTransform(string xmlFilePath, string xsltFilePath, string htmlFilePath, string paramName, string paramValue)
    {
        XPathDocument xpathDoc = new XPathDocument(xmlFilePath);
        
        //Autoriser document() et scripts
        XsltSettings settings = new XsltSettings(enableDocumentFunction: true, enableScript: false);
        XmlUrlResolver resolver = new XmlUrlResolver();
        resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;

        XslCompiledTransform xslt = new XslCompiledTransform();
        xslt.Load(xsltFilePath, settings, resolver);
        
        Console.WriteLine("XML chargé depuis : " + Path.GetFullPath(xmlFilePath));
        Console.WriteLine("XSLT chargé depuis : " + Path.GetFullPath(xsltFilePath));
        Console.WriteLine("Sortie : " + Path.GetFullPath(htmlFilePath));
        
        XsltArgumentList args = new XsltArgumentList();
        args.AddParam(paramName, "", paramValue);
        
        using (XmlTextWriter htmlWriter = new XmlTextWriter(htmlFilePath, null))
        {
            xslt.Transform(xpathDoc, args, htmlWriter, resolver);
        }
    }

}