using UnityEngine;

namespace GameCore.Objects
{
    public enum ObjectTag
    {
        Ball,
        Box,
        Coin,
        Wall,
        Other
    }

    public class GameplayObjectMovement : MonoBehaviour
    {
        public GameplayObjectPool Pool { get; set; }
        public float Speed { get; set; }

        private GameCore.GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameCore.GameManager.Instance;
        }

        private void OnEnable()
        {
            _gameManager.OnBoostStateChanged += HandleBoostStateChanged;

            _gameManager.RegisterMover(this);

            Speed = _gameManager.IsBoosted
                ? _gameManager.BoostedObjectSpeed
                : _gameManager.BaseObjectSpeed;
        }

        private void OnDisable()
        {
            _gameManager.OnBoostStateChanged -= HandleBoostStateChanged;
            _gameManager.UnregisterMover(this);
        }

        private void Update()
        {
            transform.Translate(Vector3.down * Speed * Time.deltaTime);
        }

        private void HandleBoostStateChanged(bool isBoosted)
        {
            Speed = isBoosted ? _gameManager.BoostedObjectSpeed : _gameManager.BaseObjectSpeed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ObjectTag objectTag = GetTag(gameObject.tag);
            ObjectTag otherTag = GetTag(other.tag);

            if (otherTag == ObjectTag.Ball)
            {
                switch (objectTag)
                {
                    case ObjectTag.Box:
                        Misc.Services.VibroManager.Vibrate();
                        _gameManager.AddScore(10);
                        break;
                    case ObjectTag.Coin:
                        Misc.Services.VibroManager.Vibrate();
                        _gameManager.AddCoins();
                        break;
                    case ObjectTag.Wall:
                        Misc.Services.VibroManager.Vibrate();
                        _gameManager.FinishGame();
                        break;
                    default:
                        Misc.Services.VibroManager.Vibrate();
                        _gameManager.AddScore(1);
                        break;
                }
            }
            else
            {
                _gameManager.AddScore(1);
            }

            Pool.ReturnObject(gameObject);
        }

        private ObjectTag GetTag(string tag)
        {
            return tag switch
            {
                "Ball" => ObjectTag.Ball,
                "Box" => ObjectTag.Box,
                "Coin" => ObjectTag.Coin,
                "Wall" => ObjectTag.Wall,
                _ => ObjectTag.Other
            };
        }
    }
}