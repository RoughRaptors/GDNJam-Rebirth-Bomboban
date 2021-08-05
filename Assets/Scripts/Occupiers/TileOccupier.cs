using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public abstract class TileOccupier : MonoBehaviour
    {
        private DataManager.OccupierType occupierType;

        protected Tile curTile;
        public Tile GetCurTile() { return curTile; }
        public void SetCurTile(Tile tile) { curTile = tile; }

        public abstract void ReactToExplode(int fromRow, int fromCol);

    }
}