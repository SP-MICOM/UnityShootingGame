using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public void Continue()
    {
        AudioManager.Instance.PlaySE("button");

        gameObject.SetActive(false);
    }

    public void SetOption()
    {
        PanelManager.Instance.Open(Panel.Option);

        AudioManager.Instance.PlaySE("button");

        gameObject.SetActive(false);
    }

    public void Quit()
    {
        SceneManager.Instance.MoveScene("Title");

        AudioManager.Instance.PlaySE("button");

        gameObject.SetActive(false);
    }
}
