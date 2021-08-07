using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TEMPJAMNAMEREPLACEME
{
    public class GameUI : MonoBehaviour
    {
        [Header("HUD Vars")]
        [SerializeField] private TextMeshProUGUI scoreTextMoves = null;
        [SerializeField] private TextMeshProUGUI scoreTextExplodes = null;

        [Header("Main Menu Vars")]
        [SerializeField] private GameObject mainMenuObject = null;
        [SerializeField] private Button startGameButton = null;
        [SerializeField] private Button quitGameButton = null;

        [Header("Game Over Vars")]
        [SerializeField] private GameObject gameOverObject = null;
        [SerializeField] private TextMeshProUGUI gameOverScoreObject = null;
        [SerializeField] private Button retryButton = null;
        [SerializeField] private Button quitToMenuButton = null;

        [Header("Level Vars")]
        [SerializeField] private GameObject nextLevelObject = null;
        [SerializeField] private Button nextLevelButton = null;
        [SerializeField] private TextMeshProUGUI scoreText = null;

        private void Start()
        {
            SetupButtonListners();

            mainMenuObject.SetActive(true);
            gameOverObject.SetActive(false);

            scoreTextMoves.enabled = false;
            scoreTextExplodes.enabled = false;
        }

        #region Main Menu Methods

        private void ReturnToMainMenu()
        {
            mainMenuObject.SetActive(true);
            GameManager.Instance.OnDestroyLevelClicked();
        }

        private void HideMainMenu()
        {
            mainMenuObject.SetActive(false);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void StartGame()
        {
            scoreTextMoves.enabled = true;
            scoreTextExplodes.enabled = true;
            UpdateScores();

            HideMainMenu();
            GameManager.Instance.LoadLevel(0);
        }
        #endregion


        #region Game Over Methods

        private void RetryLevel()
        {
            gameOverObject.SetActive(false);
            GameManager.Instance.LoadLevel(GameManager.Instance.GetCurLevelIndex());
        }

        #endregion

        private void SetupButtonListners()
        {
            startGameButton.onClick.AddListener(StartGame);
            quitGameButton.onClick.AddListener(QuitGame);

            retryButton.onClick.AddListener(RetryLevel);
            quitToMenuButton.onClick.AddListener(ReturnToMainMenu);

            nextLevelButton.onClick.AddListener(NextLevel);
        }

        public void UpdateScores()
        {
            scoreTextMoves.text = "Num Moves: " + GameManager.Instance.GetNumMoves().ToString();
            scoreTextExplodes.text = "Num Explodes: " + GameManager.Instance.GetNumExplodes().ToString();
        }

        private void NextLevel()
        {
            GameManager.Instance.LoadNextLevel();
        }

        public void ShowNextLevelScreen(int endingMoves, int endingExplodes)
        {
            // first off, did we win? separate screen
            if(GameManager.Instance.GetCurLevelIndex() == DataManager.levelTiles.Count - 1)
            {
                gameOverObject.SetActive(true);
                gameOverScoreObject.text = "YOU WIN! \n" + "Num Moves: " + endingMoves.ToString() + "\n" + "Num Explodes: " + endingExplodes.ToString();
            }
            else
            {
                nextLevelObject.SetActive(true);
                scoreText.text = "Num Moves: " + endingMoves.ToString() + "\n" + "Num Explodes: " + endingExplodes.ToString();
            }
        }
    }
}
