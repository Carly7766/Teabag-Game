using System.Collections;
using UnityEngine;

namespace Carly.TeabagGame.Prototype.lib
{
    public static class SlideAnimation
    {
        public static IEnumerator DoAnimation(RectTransform transform, Vector3 from, Vector3 to, float animationTime)
        {
            var t = 0f;
            while (true)
            {
                t += Time.deltaTime;
                var ratio = t / animationTime;
                var ease = EasingLerps.InOutQuad(0, 1, ratio);

                transform.anchoredPosition = Vector3.Slerp(from, to, ease);

                if (ratio > 1.0f)
                {
                    break;
                }

                yield return null;
            }
        }
    }
}