using UnityEngine;
using UnityEngine.UI;

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
        GameObject selfObj;
        public GameObject GetSelfObj() { return selfObj; }

        [SerializeField]
        GameObject groundTileObj;
        public GameObject GetGroundTileObj() { return groundTileObj; }

        [SerializeField]
        GameObject wallTileObj;
        public GameObject GetWallTileObj() { return wallTileObj; }

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
        GameObject holeObj;
        public GameObject GeHoleObj() { return holeObj; }

        [SerializeField]
        GameObject exitObj;
        public GameObject GetExitObj() { return exitObj; }

        [SerializeField]
        Button level0Btn;

        [SerializeField]
        Button level1Btn;

        [SerializeField]
        Button destroyLevelsBtn;
#pragma warning restore CS0649

        private LoadedLevel curLevel = null;
        public void SetCurLevel(LoadedLevel newLevel) { curLevel = newLevel; }
        public LoadedLevel GetCurLevel() { return curLevel; }

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

            level0Btn.GetComponentInChildren<Text>().text = "Load level 0";
            level1Btn.GetComponentInChildren<Text>().text = "Load level 1";
            destroyLevelsBtn.GetComponentInChildren<Text>().text = "Destroy current level";
        }

        private void Update()
        {

        }

        public void LoadLevel(int levelIndex)
        {
            if (!(curLevel is null))
            {
                // unload our old one for safety
                curLevel.DeconstructLevel();

                curLevel.LoadLevel(levelIndex);
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

        public void OnLoadLevel1ButtonClick()
        {
            LoadLevel(1);
        }

        public void OnDestroyLevelClicked()
        {
            DestroyLevel();
        }

        public Tile GetTileAtLocation(int row, int col)
        {
            if (row >= 0 && row < DataManager.NUM_ROWS && col >= 0 && col < DataManager.NUM_COLS)
            {
                return curLevel.GetLevel()[row, col];
            }
            return null;
        }
        public TileOccupier GetOccupierAtLocation(int row, int col)
        {
            if (row >= 0 && row < DataManager.NUM_ROWS && col >= 0 && col < DataManager.NUM_COLS)
            {
                return curLevel.GetLevel()[row, col].GetTileOuccupier();
            }
            return null;
        }

        public TileOccupier GetPlayer()
        {
            return curLevel.GetPlayer();
        }
    }
}