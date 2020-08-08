using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Cell : MonoBehaviour
    {
        private Wall[] walls;
        private GameObject wallParent = null;
        private SpriteRenderer spriteRenderer;
        private bool isInitialized;

        public Vector2Int Position
        {
            get
            {
                return new Vector2Int(Position_X, Position_Y);
            }
        }
        public int Position_X
        {
            get;
            private set;
        }
        public int Position_Y
        {
            get;
            private set;
        }

        // !!!???
        public bool IsVisited
        {
            get;
            set;
        }

        private void Start()
        {
            wallParent.SetActive(false);
        }

        private void OnEnable()
        {
            LevelManager.Instance.RegisterOnGameStart(SetSpriteMaskInteraction);
        }

        private void OnDisable()
        {
            LevelManager.Instance.UnregisterOnGameStart(SetSpriteMaskInteraction);
        }

        public void SetSpriteMaskInteraction(SpriteMaskInteraction spriteMaskInteraction)
        {
            spriteRenderer.maskInteraction = spriteMaskInteraction;

            for(int i = 0; i < walls.Length; i++)
            {
                walls[i].SpriteRenderer.maskInteraction = spriteMaskInteraction;
            }          
        }

        public void Initialize(int x, int y, string name)
        {
            if(isInitialized)
                return;

            wallParent = transform.GetChild(0).gameObject;
            spriteRenderer = GetComponent<SpriteRenderer>();

            var result = wallParent.GetComponentsInChildren<Wall>();

            walls = new Wall[result.Length];

            for(int i = 0; i < result.Length; i++)
            {
                walls[i] = result[i];
            }

            Position_X = x;
            Position_Y = y;
            gameObject.name = name;

            isInitialized = true;
        }

        public void ChangeColor(Color color)
        {
            spriteRenderer.color = color;
        }

        public void SetWall(Vector2Int direction, bool isActive)
        {
            switch(direction.x)
            {
                case 1:

                    walls[1].gameObject.SetActive(isActive);

                    break;
                case -1:

                    walls[3].gameObject.SetActive(isActive);

                    break;
            }

            switch(direction.y)
            {
                case 1:

                    walls[0].gameObject.SetActive(isActive);

                    break;
                case -1:

                    walls[2].gameObject.SetActive(isActive);

                    break;
            }

            wallParent.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.layer == 9)
            {
                ChangeColor(Color.grey);
                enabled = false;
            }
        }
    }
}