using System.Xml.Serialization;

namespace CabinetInfirmier;
[XmlRoot("patient",Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Patient
{
    private String _numero;

    [XmlElement("numero")]
    public String _Numero
    {
        get => _numero;
        set => _numero = value;
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
    private Adresse _adresse;

    [XmlElement("adresse")]
    public Adresse _Adresse
    {
        get => _adresse;
        set => _adresse = value;
    }
    
    private List<Visite> _visite;

    [XmlElement("visite")]
    public List<Visite> _Visite
    {
        get => _visite;
        set => _visite = value;
    }
    

    public override string ToString()
    {
        String s = "\nNom :"+this._Nom;
        s += "\nPrenom :"+this._Prenom;
        s += this._Adresse.ToString();
        s += "\n *** Les visites ****";
        foreach (Visite visite in this._Visite)
        {
            s += visite.toString();
        }
        return s;
    }
}