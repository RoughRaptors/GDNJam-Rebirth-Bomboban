using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 Quick Architecture Overview: 
Every level in the game is represented as a 2D array of Tiles, represented row major.
Each tile has both a tile type (ground, exit, wall, hole) and a tile occupier (ice block, cracked rock, titanium box, player, null).
Each tile knows its own row and column as well as what the object on it is.
Each block inherits from TileOccupier
 */

namespace TEMPJAMNAMEREPLACEME
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

#pragma warning disable CS0649
        [SerializeField]
        GameObject playerObj;
        public GameObject GetPlayerObj() { return playerObj; }

        [SerializeField]
        GameObject groundTileObj;
        public GameObject GetGroundTileObj() { return groundTileObj; }

        [SerializeField]
        GameObject iceCubeObj;
        public GameObject GetIceCubeObj() { return iceCubeObj; }

        [SerializeField]
        GameObject iceShardObj;
        public GameObject GetIceShardObj() { return iceShardObj; }

        [SerializeField]
        GameObject crackedRockObj;
        public GameObject GetCrackedRockObj() { return crackedRockObj; }

        [SerializeField]
        GameObject titaniumBoxObj;
        public GameObject GeTitaniumBoxObj() { return titaniumBoxObj; }

        [SerializeField]
        GameObject wallObj;
        public GameObject GetWallObj() { return wallObj; }

        [SerializeField]
        GameObject holeObj;
        public GameObject GeHoleObj() { return holeObj; }

        [SerializeField]
        GameObject exitObj;
        public GameObject GetExitObj() { return exitObj; }

        [SerializeField]
        GameUI gameUI;
#pragma warning restore CS0649

        private LoadedLevel curLevel;
        private int curLevelIndex = 0;
        public int GetCurLevelIndex() { return curLevelIndex; }

        private int numMoves;
        public int GetNumMoves() { return numMoves; }

        private int numExplodes;
        public int GetNumExplodes() { return numExplodes; }

        private void Awake()
        {
            Instance = this;
            DataManager.InitializeLevelsData();
        }

        private void Start()
        {
            if (!curLevel)
            {
                curLevel = new LoadedLevel();
            }

            curLevelIndex = 0;
            ResetScores();
        }

        public void LoadLevel(int levelIndex)
        {
            if (!(curLevel is null))
            {
                // unload our old one for safety
                curLevel.DeconstructLevel();

                bool loaded = curLevel.LoadLevel(levelIndex);
                curLevelIndex = levelIndex;
            }
        }

        public void LoadNextLevel()
        {
            if (!(curLevel is null))
            {
                curLevelIndex++;
                curLevel.LoadLevel(curLevelIndex);
            }
        }

        void DestroyLevel()
        {
            if (!(curLevel is null))
            {
                curLevel.DeconstructLevel();
            }
        }

        public void OnLoadLevel0ButtonClick()
        {
            LoadLevel(0);
        }

        public void OnDestroyLevelClicked()
        {
            DestroyLevel();
        }

        public Tile GetTileAtLocation(int row, int col)
        {
            Tile[,] level = curLevel.level;
            int rowQty = GetCurLevelRowCount();
            int colsQty = GetCurLevelColCount();
            if (row >= 0 && row < rowQty && col >= 0 && col < colsQty)
            {
                return level[row, col];
            }

            return null;
        }
        public TileOccupier GetOccupierAtLocation(int row, int col)
        {
            Tile[,] level = curLevel.level;
            int rowQty = GetCurLevelRowCount();
            int colsQty = GetCurLevelColCount();
            if (row >= 0 && row < rowQty && col >= 0 && col < colsQty)
            {
                return level[row, col].GetTileOuccupier();
            }

            return null;
        }

        public bool IsValidTile(int newRow, int newCol)
        {
            int rowQty = GetCurLevelRowCount();
            int colsQty = GetCurLevelColCount();
            if (newRow >= 0 && newRow < rowQty && newCol >= 0 && newCol < colsQty)
            {
                return true;
            }

            return false;
        }

        private void ResetScores()
        {
            numMoves = 0;
            numExplodes = 0;
        }

        public void IncrementNumMoves()
        {
            ++numMoves;
            UpdateUI();
        }

        public void IncrementNumExplodes()
        {
            ++numExplodes;
            UpdateUI();
        }

        private void UpdateUI()
        {
            gameUI.UpdateScores();
        }

        public void CompleteLevel()
        {
            int endingMoves = numMoves;
            int endingExplodes = numExplodes;
            gameUI.ShowNextLevelScreen(endingMoves, endingExplodes);

            ResetScores();
        }

        public int GetCurLevelRowCount()
        {
            return curLevel.level.GetLength(0);
        }

        public int GetCurLevelColCount()
        {
            return curLevel.level.GetLength(1);
        }

        public void ResetCurrentLevel()
        {
            LoadLevel(curLevelIndex);
        }
    }
}