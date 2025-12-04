using System.Xml.Serialization;

namespace CabinetInfirmier;
[XmlRoot("infirmiers", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Infirmiers
{
    private List<Infirmier> _listInfirmiers;
    [XmlElement("infirmier")]
    public List<Infirmier> _ListInfirmiers
    {
        get => _listInfirmiers;
        set
        {
            if (value != null)
            {
                this._listInfirmiers = value;
            }
            
        }
    }
    
    public Infirmiers(){}
    
    public void DeserialiserInf(String path)
    {
        using (TextReader reader = new StreamReader(path))
        {
            var xmlInfirmiers = new XmlSerializer(typeof(Infirmiers));
            var deserialized = (Infirmiers)xmlInfirmiers.Deserialize(reader);
            this._listInfirmiers =  deserialized._listInfirmiers;
        }
    }
    
    public void SerialiserPatients(String path)
    {
        
        using (var writer = new StreamWriter(path))
        {
            var xmlPatients = new XmlSerializer(typeof(Patients));
            xmlPatients.Serialize(writer, this);
            Console.WriteLine("Serialisation effectué avec succés");
        } 

    }
    
    public void SerialiserInf(String path)
    {
        
            using (var writer = new StreamWriter(path))
            {
                var xmlInfirmiers = new XmlSerializer(typeof(Infirmiers));
                xmlInfirmiers.Serialize(writer, this);
                Console.WriteLine("Serialisation effectué avec succés");
            } 

    }

    public String toString()
    {
        int j=0;
        String s = "";
        foreach (Infirmier i in this._listInfirmiers)
        {
            s += "\n ****Infirmier " + (j + 1)+"***";
            s+=i.toString();
            j = j + 1;
        }
        
        return s;
    }
    
}