using UnityEngine;

public class ShootingEnemy : Enemy
{
    [SerializeField] Transform laserTransform;
    private Vector3 wayPosition = Vector3.zero;
    private bool isShot = false;

    public void Awake()
    {
        health = 10;
        speed = 1.2f;
        damage = 10;
        score = 200;
    }

    public void Start()
    {
        position = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
        rigidbody = GetComponent<Rigidbody>();

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            wayPosition = playerPosition;

            wayPosition.x += Random.Range(-5f, 5f);
            wayPosition.y += Random.Range(-5f, 5f);
        }
        else
        {
            playerPosition = gameObject.transform.position;
        }
    }

    public void FixedUpdate()
    {
        Move();
        Shoot();
    }

    public override void Move()
    {
        rigidbody.transform.position += (wayPosition - position) * speed * Time.deltaTime;
    }

    public void Shoot()
    {
        if(Vector3.Distance(rigidbody.transform.position, playerPosition) <= 30 && !isShot)
        {
            GameObject laserResource = Resources.Load<GameObject>("EnemyLaser");
            GameObject laser = Instantiate(laserResource, laserTransform.position, Quaternion.identity);

            laser.GetComponent<EnemyLaser>().speed = 3f;

            isShot = true;
            Destroy(laser,3f);
        }
    }
}
