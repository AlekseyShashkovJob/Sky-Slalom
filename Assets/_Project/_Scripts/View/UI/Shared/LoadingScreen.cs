using System.Collections;
using TMPro;
using UnityEngine;

namespace View.UI.Shared
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : MonoBehaviour
    {
        private const string BASE_TEXT = "LOADING";
        private const string DOTS = "...";

        [SerializeField] private GameObject _backgroundPortrait;
        [SerializeField] private GameObject _backgroundLandscape;

        [SerializeField] private TextMeshProUGUI _loadingText;
        [SerializeField] private float _dotAnimSpeed = 0.3f;

        private Coroutine _dotAnimCoroutine;
        private Vector2 _lastScreenSize;

        private void Start()
        {
            _lastScreenSize = new Vector2(Screen.width, Screen.height);
            UpdateBackground();
        }

        void OnEnable()
        {
            _loadingText.text = BASE_TEXT + DOTS;
            _dotAnimCoroutine = StartCoroutine(AnimateDots());
        }

        void OnDisable()
        {
            if (_dotAnimCoroutine != null)
                StopCoroutine(_dotAnimCoroutine);
        }

        private void Update()
        {
            if (ScreenSizeChanged())
            {
                UpdateBackground();
            }
        }

        private bool ScreenSizeChanged()
        {
            Vector2 currentSize = new Vector2(Screen.width, Screen.height);
            if (currentSize != _lastScreenSize)
            {
                _lastScreenSize = currentSize;
                return true;
            }
            return false;
        }

        private void UpdateBackground()
        {
            bool isPortrait = Screen.height >= Screen.width;
            _backgroundPortrait.SetActive(isPortrait);
            _backgroundLandscape.SetActive(!isPortrait);
        }

        private IEnumerator AnimateDots()
        {
            yield return new WaitForSeconds(0.5f);

            int dotCount = 0;
            while (true)
            {
                dotCount = (dotCount + 1) % 4;

                string currentDots = new string('.', dotCount);
                _loadingText.text = BASE_TEXT + currentDots;
                yield return new WaitForSeconds(_dotAnimSpeed);

                if (dotCount == 3)
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
    }
}