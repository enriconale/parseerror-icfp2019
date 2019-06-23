using System.Collections.Generic;

namespace ICFP2019
{
    public partial class Status
    {
        public Map<Tile> map;
        public List<Wrappy> wrappies;
        public List<KeyValuePair<Booster, Point>> boosters;
        public List<Booster> collectedBoosters = new List<Booster>();
        public List<Goal> goals;

        public Status(Map<Tile> map, Point wrappyLoc, List<KeyValuePair<Booster, Point>> boosters)
        {
            this.map = map;
            this.wrappies = new List<Wrappy> { new Wrappy(wrappyLoc) };
            this.boosters = boosters;
            updateMap(wrappies[0]);
            CalculateGoals();
        }

        public Map<Tile> Map { get => map; set => map = value; }

        public void CalculateGoals()
        {
            goals = new List<Goal>();
            System.Console.Write("Boosters positions: ");
            foreach (var b in boosters)
            {
                goals.Add(Goal.NewGoTo(b.Value.x, b.Value.y));
                System.Console.Write(new Point(b.Value.x, b.Value.y) + " ");
            }
            System.Console.WriteLine();
            for (int y = 0; y < map.H; ++y)
            {
                for (int x = 0; x < map.W; ++x)
                {
                    if (map.isCorner(new Point(x, y), Tile.Empty))
                        goals.Add(Goal.NewGoTo(x, y));
                }
            }
        }
    }
}