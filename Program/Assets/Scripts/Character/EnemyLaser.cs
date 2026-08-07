using UnityEngine;

public class EnemyLaser : Enemy
{
    [SerializeField] Vector3 aimPosition;
    private float side = 0f;

    public void Awake()
    {
        health = 10000;
        speed = 15f;
        damage = 10;
        score = 100;
    }

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        position = rigidbody.transform.position;
        playerPosition = GameObject.FindWithTag("Player").transform.position;
    }
    public void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        rigidbody.transform.position += (playerPosition - position) * speed * Time.fixedDeltaTime;
    }
}
