using UnityEngine;

public class ChargeEnemy : Enemy
{
    public void Awake()
    {
        health = 10;
        speed = 1f;
        damage = 10;
        score = 100;
    }

    public void Start()
    {
        position = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
        rigidbody = GetComponent<Rigidbody>();
        
        if(GameObject.FindGameObjectWithTag("Player"))
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
        else
        {
            playerPosition = gameObject.transform.position;
        }
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
