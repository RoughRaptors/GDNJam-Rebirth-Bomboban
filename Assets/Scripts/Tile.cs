using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class Tile : MonoBehaviour
    {
        private DataManager.TileType tileType;

        public int row = -1;
        public int col = -1;

        private float physicalXPos;
        private float physicalYPos;

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

        void Start()
        {

        }
    }
}