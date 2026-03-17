using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace GameCore
{
    public class GameManager : MonoBehaviour
    {
        private const float _baseObjectSpeed = 425.0f;
        private const float _boostedObjectSpeed = 445.0f;

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<GameManager>();
                return _instance;
            }
        }

        public event Action<bool> OnBoostStateChanged;

        public float BaseObjectSpeed { get; private set; }
        public float BoostedObjectSpeed { get; private set; }

        public int CurrentScore { get; private set; } = 0;
        public int TotalScore { get; private set; } = 0;
        public int CurrentCoins { get; private set; } = 100;

        public bool IsBoosted { get; private set; } = false;

        public bool IsGameActive { get; private set; } = false;
        public bool IsSceneChanging { get; private set; } = false;

        [SerializeField] private View.UIScreen _winScreen;
        [SerializeField] private View.UIScreen _tutorialScreen;
        [SerializeField] private Misc.SceneManagment.SceneLoader _sceneLoader;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _coinsText;

        private readonly List<Objects.GameplayObjectMovement> _activeMovers
            = new List<Objects.GameplayObjectMovement>();

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                LoadData();

                BaseObjectSpeed = _baseObjectSpeed;
                BoostedObjectSpeed = _boostedObjectSpeed;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            IsSceneChanging = false;

            IsGameActive = false;
            CurrentScore = 0;

            Time.timeScale = 0.0f;

            UpdateScoreUI();
            OnBoostStateChanged += HandleBoostStateChanged;

            _tutorialScreen.StartScreen();
        }

        private void OnDestroy()
        {
            OnBoostStateChanged -= HandleBoostStateChanged;
            if (_instance == this)
                _instance = null;
        }

        private void Update()
        {
            if (IsSceneChanging || !IsGameActive) return;

            if (Input.GetKeyDown(KeyCode.Space))
                ToggleBoost();
        }

        public void StartGameFromTutorial()
        {
            _tutorialScreen.CloseScreen();

            IsGameActive = true;
            Time.timeScale = 1.0f;
        }

        internal void ToggleBoost()
        {
            if (IsSceneChanging || !IsGameActive) return;

            IsBoosted = !IsBoosted;
            OnBoostStateChanged?.Invoke(IsBoosted);
        }

        internal void BoostObjectsSpeed()
        {
            ToggleBoost();
        }

        internal void AddScore(int amount)
        {
            if (IsSceneChanging || !IsGameActive) return;

            CurrentScore += amount;
            UpdateScoreUI();
        }

        internal void AddCoins()
        {
            if (IsSceneChanging || !IsGameActive) return;

            CurrentCoins++;
            SaveData();
            UpdateScoreUI();
        }

        internal void RestartGame()
        {
            if (IsSceneChanging) return;

            IsSceneChanging = true;
            IsGameActive = false;
            SaveData();

            Time.timeScale = 1.0f;
            _sceneLoader.ChangeScene(Misc.Data.SceneConstants.GAME_SCENE);
        }

        internal void GoToMenu()
        {
            if (IsSceneChanging) return;

            IsSceneChanging = true;
            IsGameActive = false;
            SaveData();

            Time.timeScale = 1.0f;
            _sceneLoader.ChangeScene(Misc.Data.SceneConstants.MENU_SCENE);
        }

        internal void FinishGame()
        {
            if (IsSceneChanging || !IsGameActive) return;

            IsGameActive = false;
            Time.timeScale = 0.0f;

            TotalScore += CurrentScore;
            SaveData();

            _winScreen.StartScreen();
            UpdateScoreUI();
        }

        private void UpdateScoreUI()
        {
            if (_scoreText != null)
                _scoreText.text = $"{CurrentScore}";
            if (_coinsText != null)
                _coinsText.text = $"{CurrentCoins}";
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt(GameConstants.TOTAL_SCORE_KEY, TotalScore);
            PlayerPrefs.SetInt(GameConstants.CURRENT_COINS_KEY, CurrentCoins);
            PlayerPrefs.Save();
        }

        private void LoadData()
        {
            TotalScore = PlayerPrefs.GetInt(GameConstants.TOTAL_SCORE_KEY, TotalScore);
            CurrentCoins = PlayerPrefs.GetInt(GameConstants.CURRENT_COINS_KEY, CurrentCoins);
        }

        private void HandleBoostStateChanged(bool isBoosted)
        {
            foreach (var mover in _activeMovers)
                mover.Speed = isBoosted ? BoostedObjectSpeed : BaseObjectSpeed;
        }

        internal void RegisterMover(Objects.GameplayObjectMovement mover)
        {
            if (!_activeMovers.Contains(mover))
                _activeMovers.Add(mover);

            mover.Speed = IsBoosted ? BoostedObjectSpeed : BaseObjectSpeed;
        }

        internal void UnregisterMover(Objects.GameplayObjectMovement mover)
        {
            _activeMovers.Remove(mover);
        }
    }
}