using System;
using UnityEngine;
using UnityEngine.UI;
using View.Button;
using View.UI.Menu;

namespace View.UI.Game
{
    public class PausedScreen : UIScreen
    {
        [SerializeField] private CustomButton _continue;
        [SerializeField] private CustomButton _restart;
        [SerializeField] private CustomButton _home;

        [SerializeField] private CustomButton _sound;
        [SerializeField] private CustomButton _vibro;
        [SerializeField] private Image _soundImage;
        [SerializeField] private Image _vibroImage;

        [SerializeField] private Sprite _spriteOn;
        [SerializeField] private Sprite _spriteOff;

        private bool _isSoundOn = true;
        private bool _isVibroOn = false;

        private void OnEnable()
        {
            _continue.AddListener(ContinueGame);
            _restart.AddListener(Restart);
            _home.AddListener(OpenGameMenu);
            _sound.AddListener(ToggleSoundImage);
            _vibro.AddListener(ToggleVibroImage);

            _isSoundOn = PlayerPrefs.GetInt(Misc.Services.PlayerPrefsKeys.SoundOn, 1) == 1;
            _isVibroOn = PlayerPrefs.GetInt(Misc.Services.PlayerPrefsKeys.VibroOn, 0) == 1;

            UpdateSoundUI();
            UpdateVibroUI();
        }

        private void OnDisable()
        {
            _continue.RemoveListener(ContinueGame);
            _restart.RemoveListener(Restart);
            _home.RemoveListener(OpenGameMenu);
            _sound.RemoveListener(ToggleSoundImage);
            _vibro.RemoveListener(ToggleVibroImage);
        }

        public override void SetupScreen(UIScreen previousScreen)
        {
            throw new NotImplementedException();
        }

        public override void StartScreen()
        {
            base.StartScreen();
            Time.timeScale = 0.0f;
        }

        private void ContinueGame()
        {
            Time.timeScale = 1.0f;
            CloseScreen();
        }

        private void Restart()
        {
            CloseScreen();
            GameCore.GameManager.Instance.RestartGame();
        }

        private void OpenGameMenu()
        {
            CloseScreen();
            GameCore.GameManager.Instance.GoToMenu();
        }

        private void ToggleSoundImage()
        {
            _isSoundOn = !_isSoundOn;
            PlayerPrefs.SetInt(Misc.Services.PlayerPrefsKeys.SoundOn, _isSoundOn ? 1 : 0);
            UpdateSoundUI();
            SettingsScreen.OnSoundStateChanged?.Invoke();
        }

        private void ToggleVibroImage()
        {
            _isVibroOn = !_isVibroOn;
            PlayerPrefs.SetInt(Misc.Services.PlayerPrefsKeys.VibroOn, _isVibroOn ? 1 : 0);
            UpdateVibroUI();
            SettingsScreen.OnVibroStateChanged?.Invoke();
        }

        private void UpdateSoundUI()
        {
            _soundImage.sprite = _isSoundOn ? _spriteOn : _spriteOff;
        }

        private void UpdateVibroUI()
        {
            _vibroImage.sprite = _isVibroOn ? _spriteOn : _spriteOff;
        }
    }
}