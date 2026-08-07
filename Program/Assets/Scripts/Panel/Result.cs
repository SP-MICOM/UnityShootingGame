using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject scoreText;
    [SerializeField] GameObject highScoreText;
    [SerializeField] GameObject newRecord;
    [SerializeField] GameObject confirmButton;
    private int count = 0;

    public void Confirm()
    {
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        GameObject[] _object = { scoreText, highScoreText, newRecord, confirmButton };

        StartCoroutine(ActiveCoroutine(2f, _object));
    }

    IEnumerator ActiveCoroutine(float sec, GameObject[] gameObjects)
    {
        yield return new WaitForSeconds(sec);
    }
}
