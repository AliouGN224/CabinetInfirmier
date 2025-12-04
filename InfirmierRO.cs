using System.Xml.Serialization;

namespace CabinetInfirmier;
[XmlRoot("infirmier", Namespace="http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class InfirmierRO
{
    private String _id;

    [XmlAttribute("id")]
    public String _Id
    {
        get => _id;
        init => _id = value;
    }
    private String _nom;

    [XmlElement("nom")]
    public String _Nom
    {
        get => _nom;
        init => _nom = value;
    }
    private String _prenom;

    [XmlElement("prenom")]
    public String _Prenom
    {
        get => _prenom;
        init => _prenom = value;
    }
    private String _photo;

    [XmlElement("photo")]
    public String _Photo
    {
        get => _photo;
        init => _photo = value;
    }
}