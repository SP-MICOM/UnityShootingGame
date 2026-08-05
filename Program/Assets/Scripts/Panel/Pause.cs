using UnityEngine;

public class Pause : MonoBehaviour
{
    public void SetOption()
    {
        PanelManager.Instance.Open(Panel.Option);

        gameObject.SetActive(false);
    }

    public void Quit()
    {

    }
}
