using UnityEngine;
using UnityEngine.TextCore.Text;

public class Laser : MonoBehaviour
{
    [SerializeField] Vector3 aimPosition;
    private Rigidbody rigidbody;
    private Vector3 vector3;
    private Quaternion quaternion;
    private float speed = 0.5f;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        vector3 = rigidbody.transform.position;
        aimPosition = GameObject.FindWithTag("Aim").transform.position;
        quaternion = Quaternion.LookRotation(vector3, aimPosition);
    }

    public void FixedUpdate()
    {
        rigidbody.transform.position += (aimPosition - vector3) * speed;
    }
}
