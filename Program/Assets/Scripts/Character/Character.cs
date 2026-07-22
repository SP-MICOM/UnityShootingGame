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
    
    private static float comboTime = 0.5f;
    private static float coolTime = 0.5f;

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
        Rolling();
        Rotate();
    }

    public void Control()
    {
        vector3.x = Input.GetAxis("Horizontal");
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
        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q))
        {
            if(Time.time < rollingComboTime)
            {
                rollingCoolTime = Time.time + coolTime;

                return true;
            }
        }

        return false;
    }

    public void Move()
    {
        rigidbody.transform.position += vector3 * speed * Time.deltaTime;
    }

    public void Rotate()
    {
        if (!isRolling)
        {
            rigidbody.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * speed);
        }
    }

    public void Rolling()
    {
        if (Time.time > rollingCoolTime)
        {
            isRolling = false;
        }

        if(CheckCombo() && !isRolling)
        {
            Debug.Log("Rolling");

            isRolling = true;
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
