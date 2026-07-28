using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Character : MonoBehaviour
{
    // 스테이터스
    [SerializeField] float speed;
    [SerializeField] int Health;

    // 컴포넌트
    [SerializeField] Quaternion quaternion;
    [SerializeField] Vector3 vector3;
    [SerializeField] Rigidbody rigidbody;

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
    [SerializeField] GameObject aim;
    [SerializeField] Vector3 aimPosition;
    private GameObject laser;

    private void Start()
    {
        speed = 20f;

        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Control();
        Draw();
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
            vector3.x = Input.GetAxis("Horizontal") + (0.3f * -rollingDirection) * Mathf.Abs(Input.GetAxis("Horizontal"));
        }
        else
        {
            vector3.x = Input.GetAxis("Horizontal");
        }

        vector3.y = -Input.GetAxis("Vertical");

        quaternion = Quaternion.Euler(0, 0, Input.GetAxis("Horizontal") * -speed);

        // 입력 확인
        if (Input.GetKey(KeyCode.E))
        {
            quaternion = Quaternion.Euler(0, 0, -90);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            quaternion = Quaternion.Euler(0, 0, 90);
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
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
        aimPosition.x = transform.position.x * (Screen.width / 50) + (Input.GetAxis("Horizontal") * 300);
        aimPosition.y = transform.position.y * (Screen.height / 50) + (-Input.GetAxis("Vertical") * 300);

        aim.transform.localPosition = Vector3.Lerp(aim.transform.localPosition, aimPosition, Time.deltaTime * speed / 2);
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
        float width = (Screen.width / 1000f) * 6f;
        float height = (Screen.height / 1000f) * 6f;

        float positionX = rigidbody.transform.position.x + vector3.x;
        float positionY = rigidbody.transform.position.y + vector3.y;

        bool isMoveX = Mathf.Abs(positionX) < width;
        bool isMoveY = Mathf.Abs(positionY) < height;

        if (isMoveX && isMoveY)
        {
            rigidbody.transform.position += vector3 * speed * Time.fixedDeltaTime;
        }
        else
        {
            Vector3 move = vector3 * speed * Time.fixedDeltaTime;

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

            rigidbody.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.fixedDeltaTime * speed);
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
            quaternion = Quaternion.Euler(0, 0, rollingCount * rollingDirection * (360f / (speed * coolTime)));

            rigidbody.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.fixedDeltaTime * speed);

            rollingCount++;
        }
        else if(isRolling)
        {
            rigidbody.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.fixedDeltaTime * speed);
        }
    }

    public void Shoot(Transform transform)
    {
        laser = Resources.Load<GameObject>("Laser");
        Instantiate(laser, transform.position, Quaternion.identity, gameObject.transform);
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelManager.Instance.Open(Panel.Pause);
        }
    }
}
