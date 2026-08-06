using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int sec = 119;

    public void Update()
    {
        sec += 1;

        if (sec >= 120)
        {
            Instantiate(Resources.Load<GameObject>("Charge Enemy"), transform.position, transform.rotation);

            sec = 0;
        }
    }
}
