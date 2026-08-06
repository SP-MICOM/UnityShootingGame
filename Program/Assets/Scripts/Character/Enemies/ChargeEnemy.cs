using UnityEngine;

public class ChargeEnemy : Enemy
{
    public ChargeEnemy()
    {
        health = 10;
        speed = 2f;
        damage = 10;
    }

    public void Start()
    {
        position = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
        rigidbody = GetComponent<Rigidbody>();

        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
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
