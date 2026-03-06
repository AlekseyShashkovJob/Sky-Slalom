using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using View.Button;

namespace View.UI.Menu
{
    public class PrivacyScreen : UIScreen
    {
        [SerializeField] private CustomButton _back;

        private UIScreen _previousScreen;

        private void OnEnable()
        {
            _back.AddListener(BackToMenu);
        }

        private void OnDisable()
        {
            _back.RemoveListener(BackToMenu);
        }

        public override void SetupScreen(UIScreen previousScreen)
        {
            if (_previousScreen == null)
            {
                _previousScreen = previousScreen;
            }
        }

        private void BackToMenu()
        {
            _previousScreen.StartScreen();
            CloseScreen();
        }
    }
}