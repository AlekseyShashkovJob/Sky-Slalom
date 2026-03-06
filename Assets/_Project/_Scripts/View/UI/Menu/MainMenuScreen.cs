using UnityEngine;
using System;
using TMPro;
using View.Button;

namespace View.UI.Menu
{
    public class MainMenuScreen : UIScreen
    {
        [SerializeField] private UIScreen _shopScreen;
        [SerializeField] private UIScreen _settingsScreen;
        [SerializeField] private UIScreen _privacyScreen;

        [SerializeField] private Misc.SceneManagment.SceneLoader _sceneLoader;

        [SerializeField] private CustomButton _startGame;
        [SerializeField] private CustomButton _shop;
        [SerializeField] private CustomButton _settings;
        [SerializeField] private CustomButton _privacy;

        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _totalScoreText;

        private void OnEnable()
        {
            _startGame.AddListener(OpenGame);
            _shop.AddListener(OpenShop);
            _settings.AddListener(OpenSettings);
            _privacy.AddListener(OpenPrivacy);
        }

        private void OnDisable()
        {
            _startGame.RemoveListener(OpenGame);
            _shop.RemoveListener(OpenShop);
            _settings.RemoveListener(OpenSettings);
            _privacy.RemoveListener(OpenPrivacy);
        }

        public override void SetupScreen(UIScreen previousScreen)
        {
            throw new NotImplementedException();
        }

        public override void StartScreen()
        {
            base.StartScreen();

            int currentCoins = PlayerPrefs.GetInt(GameCore.GameConstants.CURRENT_COINS_KEY, 0);
            int totalScore = PlayerPrefs.GetInt(GameCore.GameConstants.TOTAL_SCORE_KEY, 0);

            _coinsText.text = $"{currentCoins}";
            _totalScoreText.text = $"{totalScore}";
        }

        private void OpenGame()
        {
            _sceneLoader.ChangeScene(Misc.Data.SceneConstants.GAME_SCENE);
            CloseScreen();
        }

        private void OpenShop()
        {
            _shopScreen.SetupScreen(this);
            _shopScreen.StartScreen();

            CloseScreen();
        }

        private void OpenSettings()
        {
            _settingsScreen.SetupScreen(this);
            _settingsScreen.StartScreen();

            CloseScreen();
        }

        private void OpenPrivacy()
        {
            _privacyScreen.SetupScreen(this);
            _privacyScreen.StartScreen();

            CloseScreen();
        }
    }
}