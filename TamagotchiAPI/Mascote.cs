public class Mascote {
    public List<Abilities> abilities { get; set; }
    public double height { get; set; }
    public double weight { get; set; }
    public string name { get; set; }
    public List<Results> results { get; set; }

}

public class Results
{
    public string name { get; set; }
    public string url { get; set; }
}
