using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public void Start()
    {
        PanelManager.Instance.Open(Panel.Title);
    }
}
