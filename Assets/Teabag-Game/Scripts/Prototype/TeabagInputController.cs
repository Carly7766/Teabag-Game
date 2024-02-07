using UnityEngine;

namespace Carly.TeabagGame.Prototype
{
    public class TeabagInputController : MonoBehaviour
    {
        private Camera _mainCamera;
        [SerializeField] private Rigidbody2D jointRootRigidbody2D;
        private bool _canInput = false;

        private void Awake()
        {
            _mainCamera = Camera.main;

            PrototypeGameManager.Instance.OnGameStateChanged.AddListener(status =>
            {
                if (status == GameStatus.InGame)
                {
                    _canInput = true;
                }
                else
                {
                    _canInput = false;
                }
            });
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0) && _canInput)
            {
                var targetPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                targetPos.z = 0;
                jointRootRigidbody2D.position = targetPos;
            }
        }
    }
}