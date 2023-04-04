public class Mascot {
    public string Name { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public List<Abilities> Abilities { get; set; }
    public DateTime BirthDate { get; set; }
    public int Humor { get; set; }
    public int Food { get; set; }

    public Mascot() {
        Random rnd = new Random();
        Humor = rnd.Next(0, 10);
        Food = rnd.Next(0, 10);
        BirthDate = DateTime.Now;
    }
}
