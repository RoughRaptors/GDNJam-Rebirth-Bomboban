using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class LoadedLevel : MonoBehaviour
    {
        // the internal representation of the board
        Tile[,] level = new Tile[DataManager.NUM_ROWS, DataManager.NUM_COLS];
        public Tile[,] GetLevel() { return level; }

        TileOccupier player;
        public TileOccupier GetPlayer() { return player; }

        // initializes all of the individual tiles
        void InitializeLevel(int levelIndex)
        {
            // create each of our tiles
            for (int row = 0; row < DataManager.NUM_ROWS; ++row)
            {
                for (int col = 0; col < DataManager.NUM_COLS; ++col)
                {
                    // where are we in the game space?
                    Vector3 physicalPiecePos = GetGameSpacePosFromRowCol(row, col);

                    // instantiate our tile
                    GameObject tileObj = GetTileObjectForLevel(row, col, levelIndex);
                    if (tileObj)
                    {
                        GameObject newTileObj = Instantiate(tileObj);
                        Tile newTile = newTileObj.GetComponent<Tile>();
                        newTile.InitializeData(row, col, physicalPiecePos.x, physicalPiecePos.y);

                        // instantiate our occupier, if we have one
                        GameObject occupierObj = GetOccupierObjectForLevel(row, col, levelIndex);
                        if (occupierObj)
                        {
                            GameObject newOccupierObj = Instantiate(occupierObj);
                            TileOccupier newOccupier = newOccupierObj.GetComponent<TileOccupier>();
                            if (!(newOccupier is null))
                            {
                                if (newOccupier.GetComponent<PlayerController>())
                                {                                    
                                    player = newOccupier;
                                    newOccupier.GetComponent<PlayerController>().SetPlayerObject(player);
                                }
                                
                                newTile.SetTileOccupier(newOccupier);
                                newOccupier.SetCurTile(newTile);
                                newOccupier.gameObject.transform.position = physicalPiecePos;
                            }
                        }

                        level[row, col] = newTile;
                    }
                }
            }
        }

        public void DeconstructLevel()
        {
            for (int row = 0; row < DataManager.NUM_ROWS; ++row)
            {
                for (int col = 0; col < DataManager.NUM_COLS; ++col)
                {
                    // delete our tile
                    Tile tile = level[row, col];
                    if (tile && tile.gameObject)
                    {
                        Destroy(tile.gameObject);

                        // delete our occupier if one exists
                        TileOccupier tileOccupier = level[row, col].GetTileOuccupier();
                        if (tileOccupier)
                        {
                            Destroy(tileOccupier.gameObject);
                        }
                    }
                }
            }

            //Destroy(this);
        }

        public GameObject GetTileObjectForLevel(int row, int col, int levelIndex)
        {
            // not a level
            if (levelIndex >= DataManager.levelTiles.Count)
            {
                return null;
            }

            DataManager.TileType tileType = (DataManager.TileType)DataManager.levelTiles[levelIndex][row, col];
            if(tileType == DataManager.TileType.Ground)
            {
                return GameManager.Instance.GetGroundTileObj();
            }

            return null;
        }

        public GameObject GetOccupierObjectForLevel(int row, int col, int levelIndex)
        {
            // not a level
            if(levelIndex >= DataManager.levelOccupiers.Count)
            {
                return null;
            }

            DataManager.OccupierType occupierType = (DataManager.OccupierType)DataManager.levelOccupiers[levelIndex][row, col];
            if (occupierType == DataManager.OccupierType.None)
            {
                // nothing
            }
            else if (occupierType == DataManager.OccupierType.Player)
            {
                return GameManager.Instance.GetPlayerObj();
            }
            else if(occupierType == DataManager.OccupierType.IceCube)
            {
                return GameManager.Instance.GetIceCubeObj();
            }
            else if (occupierType == DataManager.OccupierType.IceShard)
            {
                return GameManager.Instance.GetIceShardObj();
            }
            else if (occupierType == DataManager.OccupierType.CrackedRock)
            {
                return GameManager.Instance.GetCrackedRockObj();
            }
            else if (occupierType == DataManager.OccupierType.TitaniumBox)
            {
                return GameManager.Instance.GeTitaniumBoxObj();
            }
            else if (occupierType == DataManager.OccupierType.Wall)
            {
                return GameManager.Instance.GetWallObj();
            }
            else if (occupierType == DataManager.OccupierType.Hole)
            {
                return GameManager.Instance.GeHoleObj();
            }
            else if (occupierType == DataManager.OccupierType.Exit)
            {
                return GameManager.Instance.GetExitObj();
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

        public bool LoadLevel(int levelIndex)
        {
            if(levelIndex <= DataManager.levelTiles.Count && levelIndex <= DataManager.levelOccupiers.Count)
            {
                InitializeLevel(levelIndex);
                return true;
            }

            return false;
        }
    }
}