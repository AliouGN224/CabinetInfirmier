using System.Xml.Serialization;

namespace CabinetInfirmier;

[XmlRoot("infirmier",Namespace="http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Infirmier
{
    private String _id;

    [XmlAttribute("id")]
    public String _Id
    {
        get => _id;
        set => _id = value;
    }
    private String _nom;

    [XmlElement("nom")]
    public String _Nom
    {
        get => _nom;
        set => _nom = value;
    }
    private String _prenom;

    [XmlElement("prenom")]
    public String _Prenom
    {
        get => _prenom;
        set => _prenom = value;
    }
    private String _photo;

    [XmlElement("photo")]
    public String _Photo
    {
        get => _photo;
        set => _photo = value;
    }

    public Infirmier(){}
    public String toString()
    {
        String s = "\nIdentifiant : "+this._id+
                   "\nNom :"+this._nom+
                   "\n Prenom :"+this._prenom;
        return s;
    }

}