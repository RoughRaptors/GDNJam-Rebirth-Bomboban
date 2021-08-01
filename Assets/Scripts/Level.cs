using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class Level : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField]
        GameObject tileObj;
#pragma warning restore CS0649

        // the internal representation of the board
        Tile[,] level = new Tile[DataManager.NUM_ROWS, DataManager.NUM_COLS];

        void Start()
        {
            InitializeTiles();
            GameManager.Instance.SetLevel(this);
        }

        // initializes all of the individual tiles
        void InitializeTiles()
        {
            for (int row = 0; row < DataManager.NUM_ROWS; ++row)
            {
                for (int col = 0; col < DataManager.NUM_COLS; ++col)
                {
                    // if we don't have a tile here, make one
                    Tile tile = level[row, col];
                    if (tile is null)
                    {
                        // where are we in the game space?
                        Vector3 physicalPiecePos = GetGameSpacePosFromRowCol(row, col);

                        GameObject newTile = Instantiate(tileObj);
                        tile = newTile.GetComponent<Tile>();
                        tile.InitializeData(row, col, physicalPiecePos.x, physicalPiecePos.z);
                        level[row, col] = tile;
                    }
                }
            }
        }

        // return the game space physical location for the row and column
        public Vector3 GetGameSpacePosFromRowCol(int row, int col)
        {
            Vector3 retVec = new Vector3(0, DataManager.PHYSICAL_START_POS_VEC.y, 0);
            if (row >= 0 && row < DataManager.NUM_ROWS && col >= 0 && col < DataManager.NUM_COLS)
            {
                retVec.x = DataManager.PHYSICAL_START_POS_VEC.x + (row * DataManager.DISTANCE_BETWEEN_TILES);
                retVec.z = DataManager.PHYSICAL_START_POS_VEC.z + (col * DataManager.DISTANCE_BETWEEN_TILES);
            }

            return retVec;
        }
        public TileOccupier GetTileOccupierAtPosition(int row, int col)
        {
            if (row < DataManager.NUM_ROWS && col < DataManager.NUM_COLS)
            {
                return level[row, col].GetTileOuccupier();
            }

            return null;
        }
    }
}