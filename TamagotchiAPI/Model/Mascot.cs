public class Mascot {
    public List<Abilities> abilities { get; set; }
    public double height { get; set; }
    public double weight { get; set; }
    public string name { get; set; }
    public List<Results> results { get; set; }

    public int Humor { get; set; }
    public int Food { get; set; }

    public Mascot()
    {
        Random rnd = new Random();
        Humor = rnd.Next(0, 10);
        Food = rnd.Next(0, 10);
    }

}

public class Results
{
    public string name { get; set; }
    public string url { get; set; }
}

