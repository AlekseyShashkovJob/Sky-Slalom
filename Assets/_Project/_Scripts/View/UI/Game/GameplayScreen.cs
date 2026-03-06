using UnityEngine;
using System;
using View.Button;

namespace View.UI.Game
{
    public class GameplayScreen : UIScreen
    {
        [SerializeField] private CustomButton _pause;
        [SerializeField] private UIScreen _pauseScreen;

        [SerializeField] private Misc.SceneManagment.SceneLoader _sceneLoader;

        private void OnEnable()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortraitUpsideDown = false;

            _pause.AddListener(PauseGame);
        }

        private void OnDisable()
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortraitUpsideDown = false;

            _pause.RemoveListener(PauseGame);
        }

        public override void SetupScreen(UIScreen previousScreen)
        {
            throw new NotImplementedException();
        }

        private void PauseGame()
        {
            _pauseScreen.StartScreen();
        }
    }
}