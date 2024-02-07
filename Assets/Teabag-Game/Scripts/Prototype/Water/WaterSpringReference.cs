using UnityEngine;

namespace Carly.TeabagGame.Prototype
{
    public class WaterSpringReference : MonoBehaviour
    {
        public delegate void OnCollisionEnterEventHandler(int index, Vector3 velocity);

        public event OnCollisionEnterEventHandler OnCollisionEnterEvent;
        [SerializeField] private int index;

        private Transform _transform;

        private void Awake()
        {
            OnCollisionEnterEvent = delegate { };
            _transform = GetComponent<Transform>();
        }

        public void Init(int index, Vector3 position)
        {
            this.index = index;
            UpdatePosition(position);
        }

        public void UpdatePosition(Vector3 position)
        {
            _transform.localPosition = position;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.rigidbody == null) return;
            OnCollisionEnterEvent?.Invoke(index, other.rigidbody.velocity);
        }
    }
}