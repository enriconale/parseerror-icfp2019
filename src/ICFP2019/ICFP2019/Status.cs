using System.Collections.Generic;

namespace ICFP2019
{
    public partial class Status
    {
        public Map<Tile> map;
        public List<Wrappy> wrappies;
        public readonly List<KeyValuePair<Booster, Point>> boosters;
        public List<Booster> collectedBoosters = new List<Booster>();
        public List<Goal> goals;

        public Status(Map<Tile> map, Point wrappyLoc, List<KeyValuePair<Booster, Point>> boosters)
        {
            this.map = map;
            this.wrappies = new List<Wrappy> { new Wrappy(wrappyLoc) };
            this.boosters = boosters;
            // TODO: calculare i corner della mappa qui o ce li facciamo passare in construzione?
        }

        public Map<Tile> Map { get => map; set => map = value; }
    }
}