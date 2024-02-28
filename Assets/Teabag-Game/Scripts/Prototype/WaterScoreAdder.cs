using System;
using UnityEngine;

namespace Carly.TeabagGame.Prototype
{
    public class WaterScoreAdder : MonoBehaviour
    {
        private PrototypeGameManager _gameManager;

        private void Awake()
        {
            _gameManager = PrototypeGameManager.Instance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Teabag"))
            {
                _gameManager.AddScore();
            }
        }
    }
}