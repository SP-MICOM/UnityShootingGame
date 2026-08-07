using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Character : MonoBehaviour
{
    // 스테이터스
    [SerializeField] public float speed;
    [SerializeField] public int health;
    [SerializeField] public int damage;

    // 컴포넌트
    private Quaternion rotation;
    private Vector3 position;
    private Rigidbody rigidbody;

    // 롤링
    public bool isRolling = false;
    private float rollingComboTime = 0f;
    private float rollingCoolTime = 0f;
    private int rollingDirection; // 1 = Left, -1 = Right
    private int rollingCount;
    private static float comboTime = 0.4f;
    private static float coolTime = 0.5f;

    // 레이저
    [SerializeField] Transform laserPositionA;
    [SerializeField] Transform laserPositionB;
    [SerializeField] Transform laserInventoryObject;
    [SerializeField] GameObject aim;
    private Queue<GameObject> laserInventory = new Queue<GameObject>();
    private Vector3 aimPosition;
    private GameObject laser;

    // 체력
    [SerializeField] Slider healthBar;

    // 스코어
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;

    private void Start()
    {
        speed = 20f;
        health = 100;
        damage = 10;

        AudioManager.Instance.PlayBGM("BGM");
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Control();
        Draw();
        Pause();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        Rolling();
    }

    public void Control()
    {
        if (rollingDirection != 0)
        {
            position.x = Input.GetAxis("Horizontal") + (0.3f * -rollingDirection) * Mathf.Abs(Input.GetAxis("Horizontal"));
        }
        else
        {
            position.x = Input.GetAxis("Horizontal");
        }

        position.y = -Input.GetAxis("Vertical");

        rotation = Quaternion.Euler(Input.GetAxis("Vertical") * 15f, Input.GetAxis("Horizontal") * 15f, -Input.GetAxis("Horizontal") * 15f);

        // 입력 확인
        if (Input.GetKey(KeyCode.E))
        {
            rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            rotation = Quaternion.Euler(0, 0, 90);
        }

        if(Input.GetKeyDown(KeyCode.Return) && Time.timeScale != 0)
        {
            AudioManager.Instance.PlaySE("laser");
            Shoot(laserPositionA);
            Shoot(laserPositionB);
        }

        // 입력 취소 확인
        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Q))
        {
            rollingComboTime = Time.time + comboTime;
        }
    }

    public void Draw()
    {
        aimPosition.x = transform.position.x + (Input.GetAxis("Horizontal") * 5f);
        aimPosition.y = transform.position.y + (-Input.GetAxis("Vertical") * 4f);
        aimPosition.z = transform.position.z + 5f;

        aim.transform.position = Vector3.Lerp(aim.transform.position, aimPosition, Time.deltaTime * speed / 2f);

        scoreText.text = GameManager.Instance.score.ToString();
        highScoreText.text = GameManager.Instance.highScore.ToString();
    }

    public void GetDamaged(int damage)
    {
        health -= damage;

        healthBar.value = health;

        if (health <= 0)
        {
            Explose();
        }
        else
        {
            ParticleManager.Instance.Emit(transform.position);
            AudioManager.Instance.PlaySE("damaged");
        }
    }

    public bool CheckCombo()
    {
        if (isRolling) return false;

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(Time.time < rollingComboTime)
            {
                rollingDirection = -1; 
                rollingCoolTime = Time.time + coolTime;

                return true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.time < rollingComboTime)
            {
                rollingDirection = 1;
                rollingCoolTime = Time.time + coolTime;

                return true;
            }
        }

        return false;
    }

    public void Move()
    {
        float width = (Screen.width / 1000f) * 8f;
        float height = (Screen.height / 1000f) * 8f;

        float positionX = rigidbody.transform.position.x + position.x;
        float positionY = rigidbody.transform.position.y + position.y;

        bool isMoveX = Mathf.Abs(positionX) < width;
        bool isMoveY = Mathf.Abs(positionY) < height;

        if (isMoveX && isMoveY)
        {
            rigidbody.transform.position += position * speed * Time.fixedDeltaTime;
        }
        else
        {
            Vector3 move = position * speed * Time.fixedDeltaTime;

            if (!isMoveX)  move.x = 0;
            if (!isMoveY) move.y = 0;

            rigidbody.transform.position += move;
        }
    }

    public void Rotate()
    {
        if (!isRolling)
        {
            Debug.Log("Rotating");

            rigidbody.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * speed);
        }
    }

    public void Rolling()
    {
        if (Time.fixedTime > rollingCoolTime)
        {
            rollingDirection = 0;
            isRolling = false;
        }

        if (CheckCombo() && !isRolling)
        {
            Debug.Log("Rolling");

            rollingCount = 0;
            isRolling = true;
        }

        if (isRolling && Time.fixedTime < rollingCoolTime - (coolTime / 2))
        {
            rotation = Quaternion.Euler(0, 0, rollingCount * rollingDirection * (360f / (speed * coolTime)));

            rigidbody.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * speed);

            rollingCount++;
        }
        else if(isRolling)
        {
            rigidbody.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * speed);
        }
    }

    public void Shoot(Transform transform)
    {
        laser = Resources.Load<GameObject>("Laser");
        GameObject loadedLaser = null;

        for(int i = 0; i < laserInventory.Count; i++)
        {
            loadedLaser = laserInventory.Peek();

            if (loadedLaser != laser) laserInventory.Dequeue();
            else break;
        }

        int count = laserInventory.Count;

        if (laserInventory.Count <= 20)
        {
            loadedLaser = Instantiate(laser, transform.position, Quaternion.identity, laserInventoryObject);

            laserInventory.Enqueue(loadedLaser);
        }
        else
        {
            loadedLaser = laserInventory.Dequeue();

            loadedLaser.transform.position = transform.position;
            loadedLaser.transform.rotation = Quaternion.identity;

            laserInventory.Enqueue(loadedLaser);
        }

        Destroy(loadedLaser, 3f);
    } 

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;

            PanelManager.Instance.Open(Panel.Pause);
        }
    }

    public void Explose()
    {
        ParticleManager.Instance.Emit(transform.position);

        AudioManager.Instance.PlaySE("explosion");

        AudioManager.Instance.StopBGM();

        Destroy(gameObject);

        GameManager.Instance.ResetGame();

        GameManager.Instance.ReturnToTitle();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy hitEnemy = collision.gameObject.GetComponent<Enemy>();

            if (hitEnemy is ChargeEnemy)
            {
                ChargeEnemy chargeEnemy = hitEnemy as ChargeEnemy;

                GetDamaged(chargeEnemy.damage);
            }
            else if (hitEnemy is ShootingEnemy)
            {
                ShootingEnemy shootingEnemy = hitEnemy as ShootingEnemy;

                GetDamaged(shootingEnemy.damage);
            }
            else if (hitEnemy is EnemyLaser && !isRolling)
            {
                EnemyLaser enemyLaser = hitEnemy as EnemyLaser;

                GetDamaged(enemyLaser.damage);
            }
        }
    }
}
