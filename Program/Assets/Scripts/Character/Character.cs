using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int Health;
    [SerializeField] Quaternion quaternion;
    [SerializeField] Vector3 vector3;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] bool isRolling = false;
    [SerializeField] float rollingComboTime = 0f;
    [SerializeField] float rollingCoolTime = 0f;
    [SerializeField] int rollingDirection; // 1 = Left, -1 = Right
    [SerializeField] int rollingCount;

    private static float comboTime = 0.3f;
    private static float coolTime = 0.75f;

    private void Start()
    {
        speed = 20f;

        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Control();
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

        vector3.y = Input.GetAxis("Vertical");

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

        // 입력 취소 확인
        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Q))
        {
            rollingComboTime = Time.time + comboTime;
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
        rigidbody.transform.position += vector3 * speed * Time.fixedDeltaTime;
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

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelManager.Instance.Open(Panel.Pause);
        }
    }
}
