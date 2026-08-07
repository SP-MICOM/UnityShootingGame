using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ParticleManager : Singleton<ParticleManager>
{
    private Dictionary<string, GameObject> particles = new Dictionary<string, GameObject>();

    public void Emit(Vector3 position)
    {
        GameObject clone = null;

        if (particles.TryGetValue("Explosion", out clone) == false)
        {
            clone = Resources.Load<GameObject>("Explosion");

            particles.Add("Explosion", clone);
        }

        Destroy(Instantiate(clone, position, Quaternion.identity), 2.5f);
    }
}
