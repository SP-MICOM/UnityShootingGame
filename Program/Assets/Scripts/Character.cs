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

    private void Start()
    {
        speed = 20f;

        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    public void Move()
    {
        vector3.x = Input.GetAxis("Horizontal");
        vector3.y = Input.GetAxis("Vertical");

        transform.position += vector3 * speed * Time.deltaTime;
    }

    public void Rotate()
    {
        quaternion = Quaternion.Euler(0, 0, Input.GetAxis("Horizontal") * -speed);

        transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * speed);
    }
}
