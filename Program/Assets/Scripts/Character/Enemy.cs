using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody rigidbody;
    protected Quaternion rotation;
    protected Vector3 position;
    public int health;
    public float speed;
    public int damage;
    public int score;
    protected Vector3 playerPosition;

    public abstract void Move();
}
