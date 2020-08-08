using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Wall : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer
        {
            get;
            private set;
        }

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}