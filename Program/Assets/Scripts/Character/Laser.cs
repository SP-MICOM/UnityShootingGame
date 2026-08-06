using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    [SerializeField] Vector3 aimPosition;
    private Rigidbody rigidbody;
    private Vector3 position;
    private float speed = 15f;
    private float side = 0f;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        position = rigidbody.transform.position;
        aimPosition = GameObject.FindWithTag("Aim").transform.position;

        float joapyoX = position.x - rigidbody.transform.position.x;

        if (aimPosition.x < position.x + joapyoX) // 오른쪽
        {
            side = 1f;
        }
        else if (aimPosition.x > position.x + joapyoX) // 왼쪽
        {
            side = -1f;
        }

        aimPosition.x -= -side * 1.4f;
        aimPosition.y += position.y * 0.1f;
    }
    public void FixedUpdate()
    {
        Fire();
    }

    public void Fire()
    {
        rigidbody.transform.position += (aimPosition - position) * speed * Time.fixedDeltaTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy hitEnemy = collision.gameObject.GetComponent<Enemy>();

            if (hitEnemy is ChargeEnemy)
            {
                ChargeEnemy chargeEnemy = hitEnemy as ChargeEnemy;

                GameObject player = GameObject.FindWithTag("Player");
                Character character = player.GetComponent<Character>();

                chargeEnemy.health -= character.damage;

                Destroy(gameObject);
                
                if(chargeEnemy.health <= 0)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
