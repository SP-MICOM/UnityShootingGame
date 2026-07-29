using UnityEngine;
using UnityEngine.TextCore.Text;

public class Laser : MonoBehaviour
{
    [SerializeField] Vector3 aimPosition;
    private Rigidbody rigidbody;
    private Vector3 vector3;
    private Quaternion quaternion;
    private Character character;
    private float speed;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        vector3 = transform.position;
        quaternion = Quaternion.identity;
        character = new Character();
        
        speed = 1.5f;
    }

    public void FixedUpdate()
    {
        aimPosition = character.aimPosition;

        rigidbody.transform.position = Vector3.MoveTowards(vector3, aimPosition, speed);
    }
}
