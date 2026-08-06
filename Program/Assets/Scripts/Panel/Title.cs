using Unity.VisualScripting;
using UnityEngine;

public class Title : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.Instance.MoveScene("Game");

        AudioManager.Instance.PlaySE("button");

        gameObject.SetActive(false);
    }

    public void SetOptions()
    {
        PanelManager.Instance.Open(Panel.Option);

        AudioManager.Instance.PlaySE("button");

        gameObject.SetActive(false);
    }

    public void Quit()
    {
        AudioManager.Instance.PlaySE("button");

        Application.Quit();
    }
}
