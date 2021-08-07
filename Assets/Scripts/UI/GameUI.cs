using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TEMPJAMNAMEREPLACEME
{
    public class GameUI : MonoBehaviour
    {
        [Header("Main Menu Vars")]
        [SerializeField] private GameObject mainMenuObject = null;
        [SerializeField] private Button startGameButton = null;
        [SerializeField] private Button quitGameButton = null;

        [Header("Game Over Vars")]
        [SerializeField] private GameObject gameOverObject = null;
        [SerializeField] private Button retryButton = null;
        [SerializeField] private Button quitToMenuButton = null;
        [SerializeField] private TextMeshProUGUI scoreText = null;

       // [Header("Level Vars")]
       // [SerializeField] private GameObject levelObject = null;
       // [SerializeField] private Button settingsButton = null;

        private void Start()
        {
            SetupButtonListners();

            mainMenuObject.SetActive(true);
            gameOverObject.SetActive(false);
        }

        #region Main Menu Methods

        public void ReturnToMainMenu()
        {
            mainMenuObject.SetActive(true);
            GameManager.Instance.OnDestroyLevelClicked();
        }

        public void HideMainMenu()
        {
            mainMenuObject.SetActive(false);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void StartGame()
        {
            HideMainMenu();
            GameManager.Instance.OnLoadLevel0ButtonClick();
        }
        #endregion


        #region Game Over Methods

        // When the player dies call this method.
        private void HandleGameOver()
        {
            gameOverObject.SetActive(true);
            // show score
        }

        // Needs to be updated if there is more than one level
        private void RetryLevel()
        {
            gameOverObject.SetActive(false);
            GameManager.Instance.OnLoadLevel0ButtonClick();
        }


        #endregion

        private void SetupButtonListners()
        {
            startGameButton.onClick.AddListener(StartGame);
            quitGameButton.onClick.AddListener(QuitGame);

            retryButton.onClick.AddListener(RetryLevel);
            quitToMenuButton.onClick.AddListener(ReturnToMainMenu);
        }

    }
}
