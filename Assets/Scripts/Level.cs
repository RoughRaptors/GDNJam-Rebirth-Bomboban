using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class Level : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField]
        GameObject selfObj;

        [SerializeField]
        GameObject groundTileObj;

        [SerializeField]
        GameObject wallTileObj;

        [SerializeField]
        GameObject iceCubeObj;

        [SerializeField]
        GameObject iceShardObj;

        [SerializeField]
        GameObject crackedRockObj;

        [SerializeField]
        GameObject titaniumBoxObj;

        [SerializeField]
        GameObject holeObj;

        [SerializeField]
        GameObject exitObj;
#pragma warning restore CS0649

        // the internal representation of the board
        Tile[,] level = new Tile[DataManager.NUM_ROWS, DataManager.NUM_COLS];

        void Start()
        {
            InitializeLevel();
            GameManager.Instance.SetLevel(this);
        }

        // initializes all of the individual tiles
        void InitializeLevel()
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

                        // instantiate our tile
                        GameObject tileObj = GetTileObject(row, col);
                        GameObject newTile = Instantiate(tileObj);
                        tile = newTile.GetComponent<Tile>();
                        tile.InitializeData(row, col, physicalPiecePos.x, physicalPiecePos.y);

                        // instantiate our occupier, if we have one
                        GameObject occupierObj = GetOccupierObject(row, col);
                        if (occupierObj)
                        {
                            GameObject newOccupierObj = Instantiate(occupierObj);
                            TileOccupier newOccupier = newOccupierObj.GetComponent<TileOccupier>();
                            if (!(newOccupier is null))
                            {
                                tile.SetTileOccupier(newOccupier);
                                newOccupier.gameObject.transform.position = physicalPiecePos;
                            }

                            level[row, col] = tile;
                        }
                    }
                }
            }
        }

        public GameObject GetTileObject(int row, int col)
        {
            DataManager.TileType tileType = (DataManager.TileType)DataManager.testLevelTiles[row, col];
            if(tileType == DataManager.TileType.Ground)
            {
                return groundTileObj;
            }
            else if(tileType == DataManager.TileType.Wall)
            {
                return wallTileObj;
            }

            return null;
        }

        public GameObject GetOccupierObject(int row, int col)
        {
            DataManager.OccupierType occupierType = (DataManager.OccupierType)DataManager.testLevelBlocks[row, col];
            if (occupierType == DataManager.OccupierType.Self)
            {
                return selfObj;
            }
            else if(occupierType == DataManager.OccupierType.IceCube)
            {
                return iceCubeObj;
            }
            else if (occupierType == DataManager.OccupierType.IceShard)
            {
                return iceShardObj;
            }
            else if (occupierType == DataManager.OccupierType.CrackedRock)
            {
                return crackedRockObj;
            }
            else if (occupierType == DataManager.OccupierType.TitaniumBox)
            {
                return titaniumBoxObj;
            }
            else if (occupierType == DataManager.OccupierType.Hole)
            {
                return holeObj;
            }
            else if (occupierType == DataManager.OccupierType.Exit)
            {
                return exitObj;
            }
            else if(occupierType == DataManager.OccupierType.None)
            {
                // nothing
            }
            else
            {
                Debug.Log("Invalid occupier type on (" + row.ToString() + ", " + col.ToString() + ")");
            }

            return null;
        }

        // return the game space physical location for the row and column
        public Vector3 GetGameSpacePosFromRowCol(int row, int col)
        {
            Vector3 retVec = new Vector3(0, 0);
            if (row >= 0 && row < DataManager.NUM_ROWS && col >= 0 && col < DataManager.NUM_COLS)
            {
                retVec.x = DataManager.PHYSICAL_START_POS_VEC.x + (col * DataManager.DISTANCE_BETWEEN_TILES);
                retVec.y = DataManager.PHYSICAL_START_POS_VEC.y - (row * DataManager.DISTANCE_BETWEEN_TILES);
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