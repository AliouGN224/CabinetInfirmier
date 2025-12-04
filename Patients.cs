using System.Xml.Serialization;

namespace CabinetInfirmier;
[XmlRoot("patients", Namespace = "http://www.univ-grenoble-alpes.fr/l3miage/medical")]
[Serializable]
public class Patients
{
    private List<Patient> _patients;

    [XmlElement("patient")]
    public List<Patient> _Patients
    {
        get => _patients;
        set => _patients = value;
    }
    
    public void DeserialiserPatient(String path)
    {
        using (TextReader reader = new StreamReader(path))
        {
            var xmlPatients = new XmlSerializer(typeof(Patients));
            var deserialized = (Patients)xmlPatients.Deserialize(reader);
            this._patients =  deserialized._patients;
        }
    }
    public override string ToString()
    {
        int i = 0;
        String s = "\n****** Liste des patients *******"+this._patients.Count;
        Console.WriteLine("Je suis ici");
        foreach (Patient patient in _patients)
        {
            Console.WriteLine("jjjjj");
            s += "\npatient numero :" + (i + 1);
            s += patient.ToString();
            i = i + 1;
        }
        
        return s;
    }
}