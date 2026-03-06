using UnityEngine;
using TMPro;
using View.Button;

namespace View.UI.Menu
{
    public class ShopScreen : UIScreen
    {
        [SerializeField] private CustomButton _back;
        [SerializeField] private TMP_Text _coinsText;

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
            int currentCoins = PlayerPrefs.GetInt(GameCore.GameConstants.CURRENT_COINS_KEY, 0);
            _coinsText.text = $"{currentCoins}";

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