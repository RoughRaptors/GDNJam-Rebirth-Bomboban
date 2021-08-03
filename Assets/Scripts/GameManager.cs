using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Quick Architecture Overview: 
Every level in the game is represented as a 2D array of Tiles, represented row major.
Each tile has both a tile type (empty, exit, wall, hole) and a tile occupier (ice block, cracked rock, titanium box, player, null).
Each tile knows its own row and column as well as what the object on it is.
Each block inherets from TileOccupier
 */

namespace TEMPJAMNAMEREPLACEME
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        private Level curLevel;
        public void SetLevel(Level newLevel) { curLevel = newLevel; }

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {

        }
    }
}