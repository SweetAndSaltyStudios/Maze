using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class ViewMask : MonoBehaviour
    {
        private Vector2 startingSize;
        private Vector2 targetSize = new Vector2(8, 8);

        private readonly float scaleSpeed = 2f;
        private readonly float moveSpeed = 2f;
        private readonly float duration = 5f;

        private Coroutine iAnimateScale_Coroutine;
        private Coroutine iAnimatePosition_Coroutine;

        public bool IsScalingDone
        {
            get;
            private set;
        }

        public bool IsMovingDone
        {
            get;
            private set;
        }

        private void Awake()
        {
            var width = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
            startingSize = new Vector2(width * 2, width * 2);

            transform.localScale = startingSize;
        }

        public void AnimateScale()
        {
            if(iAnimateScale_Coroutine != null)
            {
                StopCoroutine(iAnimateScale_Coroutine);
            }

            iAnimateScale_Coroutine = StartCoroutine(IAnimateScale(startingSize, targetSize, duration));
        }

        private IEnumerator IAnimateScale( Vector2 currentScale, Vector2 targetScale, float duration)
        {
            var i = 0f;
            var rate = (1f / duration) * scaleSpeed;

            while(i < 1.0f)
            {
                i += rate * Time.unscaledDeltaTime;
                transform.localScale = Vector2.Lerp(currentScale, targetScale, i);
                yield return null;
            }

            IsScalingDone = true;
        }

        public void AnimatePosition(Vector2 targetPosition)
        {
            if(iAnimatePosition_Coroutine != null)
            {
                StopCoroutine(iAnimatePosition_Coroutine);
            }

            iAnimatePosition_Coroutine = StartCoroutine(IAnimatePosition(transform.position, targetPosition, duration));
        }

        private IEnumerator IAnimatePosition(Vector2 startingPosition, Vector2 targetPosition, float duration)
        {
            var i = 0f;
            var rate = (1f / duration) * moveSpeed;

            while(i < 1.0f)
            {
                i += rate * Time.unscaledDeltaTime;
                transform.position = Vector2.Lerp(startingPosition, targetPosition, i);
                yield return null;
            }

            IsMovingDone = true;
        }
    }
}