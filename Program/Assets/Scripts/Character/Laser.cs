using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    [SerializeField] Vector3 aimPosition;
    private Rigidbody rigidbody;
    private Vector3 vector3;
    private Quaternion quaternion;
    private float speed = 15f;
    private float side = 0f;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        vector3 = rigidbody.transform.position;
        aimPosition = GameObject.FindWithTag("Aim").transform.position;

        float joapyoX = vector3.x - rigidbody.transform.position.x;

        if (aimPosition.x < vector3.x + joapyoX) // 오른쪽
        {
            side = 1f;
        }
        else if (aimPosition.x > vector3.x + joapyoX) // 왼쪽
        {
            side = -1f;
        }

        aimPosition.x -= -side * 1.5f;

        quaternion = GameObject.FindWithTag("Aim").transform.rotation;
    }
    public void FixedUpdate()
    {
        Fire();
    }

    public void Fire()
    {
        rigidbody.transform.position += (aimPosition - vector3) * speed * Time.fixedDeltaTime;
    }
}
