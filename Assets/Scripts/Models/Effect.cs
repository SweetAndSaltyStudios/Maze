using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Effect : MonoBehaviour
    {
        public float AniamtionDuration = 5f;
        public float AnimationRate = 0.02f;
        public float AnimationScaleSpeed = 4f;
        public bool IsLooping;
        public Vector2 MaxScaleEffectSize = new Vector2(2, 2);
        public Sprite[] Sprites;

        private AudioSource audioSource;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            transform.localScale = Vector2.zero;
        }

        private IEnumerator Start()
        {
            if(Sprites == null || Sprites.Length <= 0)
            {
                yield break;
            }

            var waitDelay = new WaitForSecondsRealtime(AnimationRate);

            StartCoroutine(IAnimateSprites(waitDelay));
            StartCoroutine(IScaleEffect(transform.localScale, MaxScaleEffectSize));
        }

        private IEnumerator IAnimateSprites(WaitForSecondsRealtime waitDelay)
        {
            do
            {
                for(int i = 0; i < Sprites.Length; i++)
                {
                    spriteRenderer.sprite = Sprites[i];
                    yield return waitDelay;
                }

            } while(IsLooping);  
        }

        private IEnumerator IScaleEffect(Vector2 startScaleSize, Vector2 targetScaleSize)
        {
            do
            {
                yield return StartCoroutine(ILerp(startScaleSize, targetScaleSize, AnimationScaleSpeed));
                yield return StartCoroutine(ILerp(targetScaleSize, startScaleSize, AnimationScaleSpeed));

            } while(IsLooping);
        }

        private IEnumerator ILerp(Vector2 startScale, Vector2 targetScale, float lerpSpeed)
        {
            var i = 0f;
            var rate = (1f / AniamtionDuration) * lerpSpeed;

            while(i < 1.0f)
            {
                i += rate * Time.unscaledDeltaTime;
                transform.localScale = Vector2.Lerp(startScale, targetScale, i);
                yield return null;
            }
        }
    }
}
