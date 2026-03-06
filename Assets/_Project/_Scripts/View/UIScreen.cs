using System.Threading.Tasks;
using UnityEngine;

namespace View
{
    public abstract class UIScreen : MonoBehaviour
    {
        public abstract void SetupScreen(UIScreen previousScreen);

        public virtual void StartScreen()
        {
            gameObject.SetActive(true);
        }

        public void CloseScreen()
        {
            gameObject.SetActive(false);
        }
    }
}