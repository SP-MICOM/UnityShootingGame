using Unity.VisualScripting;
using UnityEngine;

public class Title : MonoBehaviour
{
    public void StartGame()
    {
        
    }

    public void SetOptions()
    {
        PanelManager.Instance.Open(Panel.Option);

        gameObject.SetActive(false);
    }

    public void Quit()
    {

    }
}
