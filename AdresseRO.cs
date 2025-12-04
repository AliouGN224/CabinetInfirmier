using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CabinetInfirmier;
[XmlRoot("adresse", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class AdresseRO
{
    private int _etage;

    [XmlElement("etage")]
    public int _Etage
    {
        get => _etage;
        init
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
        init
        {
            _numero = value;
            if (_numero < 0)
            {
                _numero = -(_numero);
            }
        }
    }

    private String _rue;

    [XmlElement("rue")] public String _Rue
    {
        get => _rue;
        init
        {
            if (value != null)
            {
                _rue = value;
            }
        }
    }

    private String _codePostal;
    
    [XmlElement("code_postal")]
    public String _CodePostal
    {
        get => _codePostal;
        init
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
        init
        {
            if (!string.IsNullOrEmpty(value))
            {
                _ville = value;
            }
        }
    }
}