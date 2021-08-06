using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class Tile : MonoBehaviour
    {
        private DataManager.TileType tileType;

        private int row = -1;

        public int GetRow() { return row; }

        private int col = -1;

        public int GetCol() { return col; }

        private float physicalXPos;
        public float GetPhysicalXPos() { return physicalXPos; }

        private float physicalYPos;

        public float GetPhysicalYPos() { return physicalYPos; }

        private TileOccupier tileOccupier;
        public TileOccupier GetTileOuccupier() { return tileOccupier; }
        public void SetTileOccupier(TileOccupier occupier)
        {
            tileOccupier = occupier;
        }

        public void InitializeData(int newRow, int newCol, float xPos, float yPos)
        {
            row = newRow;
            col = newCol;
            physicalXPos = xPos;
            physicalYPos = yPos;

            tileType = DataManager.TileType.Ground;
            tileOccupier = null;

            transform.position = new Vector3(physicalXPos, physicalYPos);
        }
    }
}