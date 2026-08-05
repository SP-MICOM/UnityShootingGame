using Unity.VectorGraphics;
using UnityEngine;

public class Option : MonoBehaviour
{
    public void Save()
    {
        Cancel();
    }

    public void Cancel()
    {
        GameObject TitleManager = GameObject.FindGameObjectWithTag("Manager");

        if (TitleManager != null)
        {
            PanelManager.Instance.Open(Panel.Title);
        }
        else
        {
            PanelManager.Instance.Open(Panel.Pause);
        }

        gameObject.SetActive(false);
    }
}
