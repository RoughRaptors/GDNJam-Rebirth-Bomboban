using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class DataManager
    {
        public const int NUM_ROWS = 20;
        public const int NUM_COLS = 20;

        // the distance between tiles
        public const float DISTANCE_BETWEEN_TILES = 1.0f;

        public const float LERP_COROUTINE_WAIT_TIME = 0.3f;

        // the physical start location of the board
        // ex: GameManager.Instance.GetLevel()[0,0] is the top left physically, so give the center of that square
        public static readonly Vector3 PHYSICAL_START_POS_VEC = new Vector3(-10, 10, 0);

        public const KeyCode moveUpKeybind = KeyCode.W;
        public const KeyCode moveDownKeybind = KeyCode.S;
        public const KeyCode moveLeftKeybind = KeyCode.A;
        public const KeyCode moveRightKeybind = KeyCode.D;

        public const KeyCode explodeKeybind = KeyCode.Space;

        public enum Direction
        {
            None = 0,
            Up,
            Down,
            Left,
            Right
        }

        public enum TileType
        {
            Ground = 0,
        }

        // cache off our levels since we are going to need to do this after the refactor and tell the game when we are ready for them
        // each "level" is composed of a set of tiles AND a set of occupiers - not ideal but we don't have time to do it right and i'm bad with bitmasks
        // we can access our levels easily this way, especially for testing, just be careful to not mix up indicies for tileLevels and occupierLevels, it's error prone
        public static List<OccupierType[,]> levelOccupiers = new List<OccupierType[,]>();

        public static void InitializeLevelsData()
        {
            AddLevel(
                @"#####
#P.G#
#####");
            AddLevel(
                @"##########
#R..R....#
#........#
#......###
#R.P..R.G#
#......###
#........#
#...R....#
##########");

            AddLevel(
                @"##########
##......##
#PY.....G#
##......##
##########");
            AddLevel(
                @"##########
#YH.YHY.P#
#......HH#
#......###
#YY...YHG#
#......###
#.H......#
#...Y....#
##########");

            AddLevel(
                @"##########
#PT.T....#
#T#......#
#.....RRR#
#.....T.G#
#.....R###
#........#
#...T....#
##########");

            AddLevel(
                @"####################
#G#................#
#.T.TTTTTTTTTTTTTT.#
#.T.R...R.R..R...T.#
#.TR..RR.R.RR..R.T.#
#.T.RRR.R..R.....T.#
#.T.R..RR....R...T.#
#.T.......R..R...T.#
#.T..R.R.R.R...R.T.#
#.T..............T.#
#.T.R........R...T.#
#.T.....PY.......T.#
#.T..............T.#
#.TTTTTTTTTTTTTTTT.#
#..................#
####################");

            AddLevel(
                @"############
#######....#
#GHHY..YPT.#
#######....#
############");

            AddLevel(
                @"##########
#####...##
#P.Y.Y..##
#...##..##
#.YR..TH.#
#..HT.H.T#
#..Y.HH..#
#..H.HH.##
#.H..H.HG#
##########");

            AddLevel(
                @"############
#......#.Y.#
#...Y..#T..#
#.......PT.#
#......#H#G#
#...R...HT.#
#.TR..T#HTY#
######.....#
############");

            AddLevel(
                @"###########
#.........#
#..##R##..#
#.#YYGRR#.#
#.#YRRRH#.#
#..#RRH#..#
#...#H#...#
#....#....#
#P........#
###########");
        }

        private static void AddLevel(string levelText)
        {
            var levelLines = levelText.Replace("\r\n", "\n").Split('\n');
            int height = levelLines.Length;
            int width = levelLines[0].Length;

            OccupierType[,] convertedLevel = new OccupierType[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    convertedLevel[i, j] = ConvertPrototypeCharToInt(levelLines, i, j);
                }
            }

            levelOccupiers.Add(convertedLevel);
        }

        private static OccupierType ConvertPrototypeCharToInt(string[] levelLines, int i, int j)
        {
            char c = levelLines[i][j];

            switch (c)
            {
                case '#':
                    return OccupierType.Wall;
                case 'P':
                    return OccupierType.Player;
                case 'Y':
                    return OccupierType.IceCube;
                case 'H':
                    return OccupierType.Hole;
                case 'T':
                    return OccupierType.TitaniumBox;
                case 'R':
                    return OccupierType.CrackedRock;
                case 'G':
                    return OccupierType.Exit;
                case '.':
                    return OccupierType.None;
                default:
                    Debug.LogError(
                        $"Trying to convert invalid character {c.ToString()} from string to OccupierType enum");
                    return OccupierType.None;
            }
        }
    }
}