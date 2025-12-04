using System.Xml.Serialization;

namespace CabinetInfirmier;
[XmlRoot("Visite", Namespace="http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Visite
{
    private String _date;
    [XmlAttribute("date")]
    public String _Date
    {
        get => _date;
        set => _date = value;
    }
    private String _intervenant;

    [XmlAttribute("intervenant")]
    public String _Intervenant
    {
        get => _intervenant;
        set => _intervenant = value;
    }
    
    
    List<Acte> _actes;
    [XmlElement("acte")]
    public List<Acte> _Actes
    {
      get => _actes;
      set => _actes = value;  
    }
    
    public string toString()
    {
        int i = 0;
        String s = "\nDate visite : "+this._Date+"\nIntervenant : "+this._Intervenant+"\n ***** Liste acte ***";
      
        foreach (Acte acte in this._Actes)
        {
            s += "\n   acte "+(i+1) +": "+ acte._Id;
        }
        return s;
    }
}

[XmlRoot("acte", Namespace="http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Acte
{
    private int _id;

    [XmlAttribute("id")]
    public int _Id
    {
        get => _id;
        set => _id = value;
    }

    
}