using Carly.TeabagGame.Prototype.lib;
using UnityEngine;
using UnityEngine.UI;

namespace Carly.TeabagGame.Prototype
{
    public class StartMenu : MonoBehaviour
    {
        private RectTransform _transform;
        [SerializeField] private Vector3 displayPos;
        [SerializeField] private Vector3 hiddenPos;

        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();

            var gameManager = PrototypeGameManager.Instance;
            gameManager.OnGameStateChanged.AddListener(status =>
            {
                if (status == GameStatus.InGame)
                {
                    StartCoroutine(SlideAnimation.DoAnimation(_transform, displayPos, hiddenPos, 0.5f));
                }
            });

            startButton.onClick.AddListener(gameManager.GameStart);
            quitButton.onClick.AddListener(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        }
    }
}