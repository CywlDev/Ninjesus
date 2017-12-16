using System;
using System.Collections.Generic;
using System.Linq;
using RoomGen.Enums;

namespace RoomGen
{
    public class LevelGenerator
    {
        private static LevelGenerator _instance;
        public static LevelGenerator Instance {
            get { return _instance ?? (_instance = new LevelGenerator()); }
        }

    private const double SpawnChanceBonus = 0.3333333;

        private LevelGenerator()
        {
        }
        
        /// <summary>
        /// Generate a single level with spawn-, boss- and key-room.
        /// </summary>
        /// <param name="depth">Depth of the level.</param>
        /// <returns>Level</returns>
        public int[][] GenerateLevel(int depth = 3)
        {
            var rooms = new int[15][];
            bool success;
            do
            {
                // empty level
                for (var i = 0; i < rooms.Length; i++)
                {
                    rooms[i] = new int[15];
                }

                // spawn room
                rooms[7][7] = 1;

                success = GenerateLevelInternal(rooms, new List<Tuple<int, int>> { new Tuple<int, int>(7, 7) }, 0, depth);
            } while (!success);
            return rooms;
        }

        private static bool GenerateLevelInternal(int[][] rooms, List<Tuple<int, int>> definiteLocations, int currentIteration,
            int maxIterations)
        {
            // last iteration
            if (currentIteration == maxIterations)
            {
                // generate key and boss room
                if (definiteLocations.Count < 2) return false;

                // weigth positions to get maximum distance between key and boss room
                var weighted = definiteLocations.Select(x => new { Tuple = x, Val = x.Item1 + x.Item2 }).OrderBy(x => x.Val).ToList();

                // boss room
                var boss = weighted.First();
                weighted.Remove(boss);
                rooms[boss.Tuple.Item1][boss.Tuple.Item2] = (int)RoomType.Boss;

                // key room
                var key = weighted.Last();
                weighted.Remove(key);
                rooms[key.Tuple.Item1][key.Tuple.Item2] = (int)RoomType.Key;

                // generate bonus rooms for remaining paths
                foreach (var rest in weighted)
                {
                    var hostile = Util.RandomGenerator.NextDouble() > SpawnChanceBonus;
                    rooms[rest.Tuple.Item1][rest.Tuple.Item2] = hostile ? (int)RoomType.Trap : (int)RoomType.Bonus;
                }

                return true;
            }

            var nextIteration = new List<Tuple<int, int>>();

            foreach (var d in definiteLocations)
            {
                var possible = GetPossibleLocations(rooms.ToList(), d.Item1, d.Item2);
                var definite = GetDefiniteLocations(rooms, possible, d.Item1, d.Item2);

                var count = definite.Count;
                // spawn point
                if (count == 4)
                {
                    rooms[d.Item1][d.Item2] = (int)RoomType.Spawn;
                    definite.RemoveAt(Util.RandomGenerator.Next(0, 4));
                    foreach (var tuple in definite)
                    {
                        rooms[tuple.Item1][tuple.Item2] = (int)RoomType.Normal;
                    }
                    nextIteration = definite;
                }

                // 3 paths available => create bonus room or new path
                else if (count == 3)
                {
                    var nums = Util.GetRandomDistinctNumbers(3);
                    var bonusLocation = definite[nums[0]];
                    var next = definite[nums[1]];

                    var createBonus = Util.RandomGenerator.Next(0, 2) == 0;
                    if (createBonus)
                    {
                        // 60% chance to spawn hostile room
                        var hostile = Util.RandomGenerator.NextDouble() > SpawnChanceBonus;
                        rooms[bonusLocation.Item1][bonusLocation.Item2] = hostile
                            ? (int)RoomType.Trap
                            : (int)RoomType.Bonus;
                    }
                    else
                    {
                        rooms[bonusLocation.Item1][bonusLocation.Item2] = (int)RoomType.Normal;
                        nextIteration.Add(new Tuple<int, int>(bonusLocation.Item1, bonusLocation.Item2));
                    }
                    rooms[next.Item1][next.Item2] = (int)RoomType.Normal;
                    nextIteration.Add(new Tuple<int, int>(next.Item1, next.Item2));
                }
                // 2 or 1 path available, choose random path to go on
                else if (count > 0)
                {
                    var num = Util.RandomGenerator.Next(0, definite.Count);
                    var next = definite[num];
                    rooms[next.Item1][next.Item2] = (int)RoomType.Normal;
                    nextIteration.Add(new Tuple<int, int>(next.Item1, next.Item2));
                }
            }

            return GenerateLevelInternal(rooms, nextIteration, currentIteration + 1, maxIterations);
        }

        /// <summary>
        /// Get possible positions for location.
        /// </summary>
        private static List<Tuple<int, int>> GetPossibleLocations(List<int[]> rooms, int x, int y)
        {
            var locs = new List<Tuple<int, int>>();
            var up = rooms[x][y - 1];
            var right = rooms[x + 1][y];
            var down = rooms[x][y + 1];
            var left = rooms[x - 1][y];

            // 0 ... room may be generated here
            if (up == 0) locs.Add(new Tuple<int, int>(x, y - 1));
            if (right == 0) locs.Add(new Tuple<int, int>(x + 1, y));
            if (down == 0) locs.Add(new Tuple<int, int>(x, y + 1));
            if (left == 0) locs.Add(new Tuple<int, int>(x - 1, y));

            return locs;
        }

        /// <summary>
        /// Get locations where a room can be placed without touching another one
        /// </summary>
        private static List<Tuple<int, int>> GetDefiniteLocations(int[][] rooms, List<Tuple<int, int>> possibleLocations,
            int x, int y)
        {
            var definiteLocations = new List<Tuple<int, int>>();
            foreach (var loc in possibleLocations)
            {
                var surrounding = GetPossibleLocations(rooms.ToList(), loc.Item1, loc.Item2);
                // remove start position from surrounding
                var cnt = surrounding.Count(pos => !(pos.Item1 == x && pos.Item2 == y));
                if (cnt == 3)
                {
                    // cnt 3 => room to be generated has no adjoint rooms
                    definiteLocations.Add(loc);
                }
            }
            return definiteLocations;
        }
    }
}
