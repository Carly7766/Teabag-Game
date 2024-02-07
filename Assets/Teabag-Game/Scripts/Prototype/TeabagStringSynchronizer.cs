using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Carly.TeabagGame.Prototype
{
    public class TeabagStringSynchronizer : MonoBehaviour
    {
        [SerializeField] private Transform teabagRootTransform;
        [SerializeField] private Transform teabagJointTransform;
        [SerializeField] private LineRenderer lineRenderer;

        private void Update()
        {
            lineRenderer.SetPositions(new[]
            {
                teabagRootTransform.position,
                teabagJointTransform.position
            });
        }
    }
}