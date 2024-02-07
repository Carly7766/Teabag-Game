using Carly.TeabagGame.Prototype.lib;
using TMPro;
using UnityEngine;

namespace Carly.TeabagGame.Prototype
{
    public class InGameMenu : MonoBehaviour
    {
        private RectTransform _transform;
        [SerializeField] private Vector3 displayPos;
        [SerializeField] private Vector3 hiddenPos;

        [SerializeField] private TMP_Text text;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();

            var gameManager = PrototypeGameManager.Instance;
            gameManager.OnGameStateChanged.AddListener(status =>
            {
                if (status == GameStatus.InGame)
                {
                    StartCoroutine(SlideAnimation.DoAnimation(_transform, hiddenPos, displayPos, 0.5f));
                }
            });
            gameManager.OnScoreChanged.AddListener(score => text.text = $"Score:{score}");
        }
    }
}