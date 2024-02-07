using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Transform = log4net.Util.Transform;

namespace Carly.TeabagGame.Prototype
{
    [RequireComponent(typeof(SpriteShapeController))]
    public class WaterShapeController : MonoBehaviour
    {
        [SerializeField] private int topLeftPointIndex;
        [SerializeField] private int topRightPointIndex;
        [SerializeField] private GameObject springReferencePrefab;

        [Space(16)] [SerializeField, Range(0, 100)]
        private int waveCount;

        [Space(16)] [SerializeField] private float springStiffness = 0.1f;
        [SerializeField] private float dampening = 0.03f;
        [SerializeField] private float spread = 0.006f;

        private SpriteShapeController _spriteShapeController;
        [SerializeField] private Spline spline;

        private SpringModel[] _springProperties;
        private WaterSpringReference[] _springReferences;

        private void Awake()
        {
            _spriteShapeController = GetComponent<SpriteShapeController>();
            spline = _spriteShapeController.spline;

            InitializeWaves();
        }

        private void InitializeWaves()
        {
            var topLeftPosition = spline.GetPosition(topLeftPointIndex);
            var topRightPosition = spline.GetPosition(topRightPointIndex);
            var waterWidth = Mathf.Abs(topLeftPosition.x - topRightPosition.x);

            var spacingPerWave = waterWidth / (waveCount + 1);
            for (var i = 0; i < waveCount; i++)
            {
                var index = topLeftPointIndex + 1;

                var x = topLeftPosition.x + spacingPerWave * (waveCount - i);

                var wavePoint = new Vector3(x, topLeftPosition.y, topLeftPosition.z);

                spline.InsertPointAt(index, wavePoint);
                spline.SetHeight(index, 0.1f);
                spline.SetCorner(index, false);
                spline.SetTangentMode(index, ShapeTangentMode.Continuous);
            }

            _springProperties = new SpringModel[waveCount + 2];
            _springReferences = new WaterSpringReference[waveCount + 2];
            for (var i = 0; i < waveCount; i++)
            {
                var index = topLeftPointIndex + i + 1;

                var smoothedTangent = GetSmoothedTangent(index);
                spline.SetLeftTangent(index, smoothedTangent.leftTangent);
                spline.SetRightTangent(index, smoothedTangent.rightTangent);

                var springRefGameObject = Instantiate(springReferencePrefab, transform);

                var springPosition = spline.GetPosition(index);
                springRefGameObject.transform.localPosition = springPosition;

                var springRef = springRefGameObject.GetComponent<WaterSpringReference>();
                springRef.Init(i, springPosition);
                springRef.OnCollisionEnterEvent += OnCollisionSpringReference;

                _springProperties[i] = new SpringModel(springPosition.y, springPosition.y);
                _springReferences[i] = springRef;
            }
        }

        private (Vector3 leftTangent, Vector3 rightTangent) GetSmoothedTangent(int index)
        {
            var position = spline.GetPosition(index);
            var positionPrev = position;
            var positionNext = position;
            if (index > 1)
            {
                positionPrev = spline.GetPosition(index - 1);
            }

            if (index - 1 <= waveCount)
            {
                positionNext = spline.GetPosition(index + 1);
            }

            var forward = gameObject.transform.forward;

            var scale = Mathf.Min((positionNext - position).magnitude, (positionPrev - position).magnitude) * 0.33f;

            var leftTangent = (positionPrev - position).normalized * scale;
            var rightTangent = (positionNext - position).normalized * scale;

            SplineUtility.CalculateTangents(position, positionPrev, positionNext, forward, scale, out rightTangent,
                out leftTangent);

            return (leftTangent, rightTangent);
        }

        private void FixedUpdate()
        {
            UpdateSprings();
        }

        private void UpdateSprings()
        {
            for (var i = 0; i < waveCount; i++)
            {
                var index = topLeftPointIndex + i + 1;

                _springProperties[i].UpdateSpring(springStiffness, dampening);

                var springPosition = spline.GetPosition(index);
                springPosition.y = _springProperties[i].Height;

                _springReferences[i].UpdatePosition(springPosition);
                spline.SetPosition(index, springPosition);
            }

            ApplySpread();
        }

        private void ApplySpread()
        {
            var length = _springProperties.Length;
            var left_deltas = new float[length];
            var right_deltas = new float[length];

            for (var i = 0; i < length; i++)
            {
                if (i > 0)
                {
                    left_deltas[i] = spread * (_springProperties[i].Height - _springProperties[i - 1].Height);
                    _springProperties[i - 1].ApplyVelocity(left_deltas[i]);
                }

                if (i < length - 1)
                {
                    right_deltas[i] = spread * (_springProperties[i].Height - _springProperties[i + 1].Height);
                    _springProperties[i + 1].ApplyVelocity(right_deltas[i]);
                }
            }
        }

        private void OnCollisionSpringReference(int index, Vector3 velocity)
        {
            _springProperties[index].ApplyVelocity(velocity.y);
        }
    }
}