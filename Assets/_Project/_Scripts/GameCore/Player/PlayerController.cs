using UnityEngine;

namespace GameCore.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const float _moveSpeed = 1000.0f;
        private const float _swipeThreshold = 50.0f;
        private const float _movementEpsilon = 0.01f;

        [SerializeField] private Transform _leftPoint;
        [SerializeField] private Transform _rightPoint;

        private Vector3 _targetPosition;
        private Vector2 _swipeStartPos;
        private bool _isSwiping;
        private bool _isMoving;

        private void Awake() => _targetPosition = _rightPoint.position;
        private void Start() => transform.position = _targetPosition;

        private void Update()
        {
            HandleTouchAndKeyboardInput();
            MoveBall();
        }

        private void HandleTouchAndKeyboardInput()
        {
            // Keyboard: движение влево/вправо
            if (!_isMoving)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    if (_targetPosition != _leftPoint.position)
                    {
                        _targetPosition = _leftPoint.position;
                        _isMoving = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    if (_targetPosition != _rightPoint.position)
                    {
                        _targetPosition = _rightPoint.position;
                        _isMoving = true;
                    }
                }
            }

            // Touch: тап слева/справа + свайп вверх (BOOST TOGGLE)
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 pos = touch.position;

                if (touch.phase == TouchPhase.Began)
                {
                    if (!_isMoving)
                    {
                        if (pos.x < Screen.width / 2f)
                        {
                            if (_targetPosition != _leftPoint.position)
                            {
                                _targetPosition = _leftPoint.position;
                                _isMoving = true;
                            }
                        }
                        else
                        {
                            if (_targetPosition != _rightPoint.position)
                            {
                                _targetPosition = _rightPoint.position;
                                _isMoving = true;
                            }
                        }
                    }

                    _swipeStartPos = pos;
                    _isSwiping = true;
                }
                else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && _isSwiping)
                {
                    float deltaY = pos.y - _swipeStartPos.y;
                    if (deltaY > _swipeThreshold)
                    {
                        GameCore.GameManager.Instance.ToggleBoost();
                    }
                    _isSwiping = false;
                }
            }
        }

        private void MoveBall()
        {
            Vector3 newPosition = new(_targetPosition.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, _moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, newPosition) < _movementEpsilon)
            {
                transform.position = newPosition;
                _isMoving = false;
            }
        }
    }
}