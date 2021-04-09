
public class BetterDoctorResult
{
    public Meta meta { get; set; }
    public Datum[] data { get; set; }
}

public class Meta
{
    public string data_type { get; set; }
    public string item_type { get; set; }
    public int total { get; set; }
    public int count { get; set; }
    public int skip { get; set; }
    public int limit { get; set; }
    public string fields_requested { get; set; }
}

public class Datum
{
    public Practice[] practices { get; set; }
    public ProfileD profile { get; set; }
    public string npi { get; set; }
}

public class Practice
{
    public string location_slug { get; set; }
    public bool within_search_area { get; set; }
    public float distance { get; set; }
    public float lat { get; set; }
    public float lon { get; set; }
    public string uid { get; set; }
    public string name { get; set; }
    public bool accepts_new_patients { get; set; }
    public string[] insurance_uids { get; set; }
    public Visit_Address visit_address { get; set; }
    public object[] office_hours { get; set; }
    public Phone[] phones { get; set; }
    public Language1[] languages { get; set; }
    public string website { get; set; }
    public Medium[] media { get; set; }
}

public class Visit_Address
{
    public string city { get; set; }
    public float lat { get; set; }
    public float lon { get; set; }
    public string state { get; set; }
    public string state_long { get; set; }
    public string street { get; set; }
    public string street2 { get; set; }
    public string zip { get; set; }
}

public class Phone
{
    public string number { get; set; }
    public string type { get; set; }
}

public class Language1
{
    public string name { get; set; }
    public string code { get; set; }
}

public class Medium
{
    public string uid { get; set; }
    public string status { get; set; }
    public string url { get; set; }
    public string[] tags { get; set; }
    public Versions versions { get; set; }
}

public class Versions
{
    public string small { get; set; }
    public string medium { get; set; }
    public string large { get; set; }
    public string hero { get; set; }
}


public class ProfileD
{
    public string first_name { get; set; }
    public string middle_name { get; set; }
    public string last_name { get; set; }
    public string slug { get; set; }
    public string title { get; set; }
    public string image_url { get; set; }
    public string gender { get; set; }
    public LanguageD[] languages { get; set; }
    public string bio { get; set; }
}

public class LanguageD
{
    public string name { get; set; }
    public string code { get; set; }
}
