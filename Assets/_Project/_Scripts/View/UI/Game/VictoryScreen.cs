using UnityEngine;
using TMPro;
using View.Button;
using System;

namespace View.UI.Game
{
    public class VictoryScreen : UIScreen
    {
        [SerializeField] private CustomButton _home;
        [SerializeField] private CustomButton _restart;

        [SerializeField] private Misc.SceneManagment.SceneLoader _sceneLoader;

        [SerializeField] private TMP_Text _currentScoreText;
        [SerializeField] private TMP_Text _totalScoreText;
        [SerializeField] private TMP_Text _currentCoins;

        private void OnEnable()
        {
            _home.AddListener(BackToMenu);
            _restart.AddListener(Restart);
        }

        private void OnDisable()
        {
            _home.RemoveListener(BackToMenu);
            _restart.RemoveListener(Restart);
        }

        public override void SetupScreen(UIScreen previousScreen)
        {
            throw new NotImplementedException();
        }

        public override void StartScreen()
        {
            base.StartScreen();

            _currentScoreText.text = $"{GameCore.GameManager.Instance.CurrentScore}";
            _totalScoreText.text = $"{GameCore.GameManager.Instance.TotalScore}";
            _currentCoins.text = $"{GameCore.GameManager.Instance.CurrentCoins}";
        }

        private void BackToMenu()
        {
            CloseScreen();
            GameCore.GameManager.Instance.GoToMenu();
        }

        private void Restart()
        {
            CloseScreen();
            GameCore.GameManager.Instance.RestartGame();
        }
    }
}