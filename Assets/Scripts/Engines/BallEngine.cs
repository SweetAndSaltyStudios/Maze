using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sweet_And_Salty_Studios
{
    public class BallEngine : MonoBehaviour
    {
        private Rigidbody2D rb2D;
        private Vector2 tilt;
        private readonly float moveSpeed = 300f;

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            tilt = Input.acceleration;
        }

        private void FixedUpdate()
        {
            if(tilt != Vector2.zero)
            {
                rb2D.velocity = tilt * (tilt.y > 0 ? moveSpeed * 2 : moveSpeed) * Time.fixedDeltaTime;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.layer == 8)
            {
                SceneManager.LoadScene(0);
            }         
        }
    }
}