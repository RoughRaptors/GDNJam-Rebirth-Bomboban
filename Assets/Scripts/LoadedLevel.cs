using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class LoadedLevel : MonoBehaviour
    {
        // the internal representation of the board
        Tile[,] level = new Tile[DataManager.NUM_ROWS, DataManager.NUM_COLS];

        public Tile[,] GetLevel()
        {
            return level;
        }

        TileOccupier player;

        public TileOccupier GetPlayer()
        {
            return player;
        }

        // initializes all of the individual tiles
        void InitializeLevel(OccupierType[,] levelOccupiers)
        {
            int levelHeight = levelOccupiers.GetLength(0);
            int levelWidth = levelOccupiers.GetLength(1);
            // create each of our tiles
            for (int row = 0; row < levelHeight; ++row)
            {
                for (int col = 0; col < levelWidth; ++col)
                {
                    var newTile = CreateTile(row, col);

                    OccupierType occupierValue = levelOccupiers[row, col];
                    // where are we in the game space?
                    Vector3 physicalPiecePos = GetGameSpacePosFromRowCol(row, col);
                    TileOccupier occupier = InstantiateOccupier(occupierValue, row, col);

                    AssignOccupier(newTile, occupier, physicalPiecePos);

                    level[row, col] = newTile;
                }
            }
        }

        private void AssignOccupier(Tile newTile, TileOccupier occupier, Vector3 physicalPiecePos)
        {
            if (occupier == null)
                return;
            TryRegisterPlayerOccupier(occupier);

            newTile.SetTileOccupier(occupier);
            occupier.SetCurTile(newTile);
        }

        private void TryRegisterPlayerOccupier(TileOccupier occupier)
        {
            if (occupier.GetComponent<PlayerController>())
            {
                player = occupier;
                occupier.GetComponent<PlayerController>().SetPlayerObject(player);
            }
        }

        private TileOccupier InstantiateOccupier(OccupierType occupierValue, int row, int col)
        {
            // instantiate our occupier, if we have one
            GameObject occupierObj = GetOccupierObjectForLevel(occupierValue);

            if (occupierObj)
            {
                GameObject newOccupierObj = Instantiate(occupierObj);
                Vector3 physicalPiecePos = GetGameSpacePosFromRowCol(row, col);
                newOccupierObj.transform.position = physicalPiecePos;
                return newOccupierObj.GetComponent<TileOccupier>();
            }

            return null;
        }

        private Tile CreateTile(int row, int col)
        {
            // where are we in the game space?
            Vector3 physicalPiecePos = GetGameSpacePosFromRowCol(row, col);
            // instantiate our tile
            GameObject tileObj = GameManager.Instance.GetGroundTileObj();
            GameObject newTileObj = Instantiate(tileObj);
            Tile newTile = newTileObj.GetComponent<Tile>();
            newTile.InitializeData(row, col, physicalPiecePos.x, physicalPiecePos.y);
            return newTile;
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

        private GameObject GetOccupierObjectForLevel(OccupierType occupierType)
        {
            if (occupierType == OccupierType.None)
            {
                return null;
            }
            else if (occupierType == OccupierType.Player)
            {
                return GameManager.Instance.GetPlayerObj();
            }
            else if (occupierType == OccupierType.IceCube)
            {
                return GameManager.Instance.GetIceCubeObj();
            }
            else if (occupierType == OccupierType.CrackedRock)
            {
                return GameManager.Instance.GetCrackedRockObj();
            }
            else if (occupierType == OccupierType.TitaniumBox)
            {
                return GameManager.Instance.GeTitaniumBoxObj();
            }
            else if (occupierType == OccupierType.Wall)
            {
                return GameManager.Instance.GetWallObj();
            }
            else if (occupierType == OccupierType.Hole)
            {
                return GameManager.Instance.GeHoleObj();
            }
            else if (occupierType == OccupierType.Exit)
            {
                return GameManager.Instance.GetExitObj();
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

        public bool LoadLevel(int levelIndex)
        {
            if (levelIndex < DataManager.levelOccupiers.Count)
            {
                var levelOccupiers = DataManager.levelOccupiers[levelIndex];
                InitializeLevel(levelOccupiers);
                return true;
            }

            return false;
        }
    }
}