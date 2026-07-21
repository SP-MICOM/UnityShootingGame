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

    private bool isRolling = false;
    private float comboTime = 0f;
    
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
    }

    public void Control()
    {
        vector3.x = Input.GetAxis("Horizontal");
        vector3.y = Input.GetAxis("Vertical");

        quaternion = Quaternion.Euler(0, 0, Input.GetAxis("Horizontal") * -speed);

        if (Input.GetKey(KeyCode.E))
        {
            quaternion = Quaternion.Euler(0, 0, -90);
            comboTime = Time.time + 0.25f;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            quaternion = Quaternion.Euler(0, 0, 90);
        }
    }

    public void Check()
    {

    }

    public void Move()
    {
        rigidbody.transform.position += vector3 * speed * Time.deltaTime;
    }

    public void Rotate()
    {
        rigidbody.transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * speed);
    }


    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelManager.Instance.Open(Panel.Pause);
        }
    }
}
