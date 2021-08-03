using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{

    public abstract class TileOccupier : MonoBehaviour
    {
        protected Tile curTile;
        public Tile GetCurTile() { return curTile; }
        public void SetCurTile(Tile tile) { curTile = tile; }

        public abstract void ReactToExplode(int fromRow, int fromCol);

        public int GetRow()
        {
            if (curTile)
            {
                return curTile.row;
            }

            return -1;
        }

        public int GetCol()
        {
            if (curTile)
            {
                return curTile.col;
            }

            return -1;
        }
    }
}