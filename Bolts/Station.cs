namespace Bolts
{
    public class Station
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Z { get; set; }
        public double Distance { get; set; }
        public double BoltLength { get; set; }
        public int TravelTime { get; set; }

        public Station (string name, int x, int z, double distance = 0, double boltLength = 0, int travelTime = 0)
        {
            this.Name = name;
            this.X = x;
            this.Z = z;
            this.Distance = distance;
            this.BoltLength = boltLength;
            this.TravelTime = travelTime;
        }
    }
}
