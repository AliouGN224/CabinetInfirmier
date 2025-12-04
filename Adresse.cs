using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CabinetInfirmier;

[XmlRoot("adresse", Namespace="http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Adresse
{
    private int _etage;

    [XmlElement("etage")]
    public int _Etage
    {
        get => _etage;
        set
        {
           _etage = value;
           if (_etage < 0)
           {
               _etage = -(_etage);
           }
        }
    }
    private int _numero;

    [XmlElement("numero")]
    public int _Numero
    {
        get => _numero;
        set
        {
            _numero = value;
            if (_numero < 0)
            {
                _numero = -(_numero);
            }
        }
    }
    private String _rue;

    [XmlElement("rue")]
    public String _Rue
    {
        get => _rue;
        set => _rue = value;
    }
    private String _codePostal;

    [XmlElement("codePostal")]
    public String _CodePostal
    {
        get => _codePostal;
        set
        {
            if (!string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^\d+$"))
            {
                _codePostal = value;
            }
        }
    }
    private String _ville;

    [XmlElement("ville")]
    public String _Ville
    {
        get => _ville;
        set => _ville = value;
    }

    public override string ToString()
    {
        string s ="\nAdresse : "+this._numero+" "+this._Rue+
                  this._CodePostal+" "+this._Ville;
        return s;
    }


    public void DeserialiserInf(String path)
    {
        using (TextReader reader = new StreamReader(path))
        {
            var xmlAdresse = new XmlSerializer(typeof(Adresse));
            var deserialized = (Adresse)xmlAdresse.Deserialize(reader);
            this._Etage = deserialized._Etage;
            this._Numero = deserialized._Numero;
            this._Rue = deserialized._Rue;
            this._CodePostal = deserialized._CodePostal;
            this._Ville = deserialized._Ville;
            
        }
    }
    
    public void SerialiserInf(String path)
    {
        
        using (var writer = new StreamWriter(path))
        {
            var xmlAdresse = new XmlSerializer(typeof(Adresse));
            xmlAdresse.Serialize(writer, this);
            Console.WriteLine("Serialisation effectué avec succés");
        } 
    }

}