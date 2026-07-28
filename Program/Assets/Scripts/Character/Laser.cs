using UnityEngine;

public class Laser : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Vector3 vector3;
    private float speed;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        vector3 = transform.position;

        speed = 1.5f;
    }

    public void FixedUpdate()
    {
        vector3.z += speed;

        transform.position = vector3;
    }
}
