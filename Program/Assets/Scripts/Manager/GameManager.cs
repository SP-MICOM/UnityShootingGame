using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int score = 0;
    public int highScore = 10000;
    int preHighScore = 0;
    private int sec = 0;
    private Dictionary<Enemies, GameObject> enemyList = new Dictionary<Enemies, GameObject>();
    private Queue<GameObject> enemyInventory = new Queue<GameObject>();

    public void Update()
    {
        if(Time.timeScale != 1f)
        {
            return;
        }

        preHighScore = highScore;

        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            sec++;

            int random = Random.Range(0, 2);

            if(sec >= 180)
            {
                switch(random)
                {
                    case 0:
                        Spawn(Enemies.ShootingEnemy);
                        break;
                    case 1:
                        Spawn(Enemies.ChargeEnemy);
                        break;
                    default:
                        break;
                }

                sec = 0;
            }
        }
    }

    public void Spawn(Enemies enemies)
    {
        GameObject clone = null;
        GameObject cloneType = null;

        if (enemyList.TryGetValue(enemies, out cloneType) == false) // 적 목록에 해당 적이 없을 시
        {
            cloneType = Resources.Load<GameObject>(enemies.ToString());

            cloneType.name = cloneType.name.Replace("(Clone)", "");

            enemyList.Add(enemies, cloneType);
        }
        else // 적 목록에 해당 적이 있을 시
        {
            cloneType = enemyList[enemies];
        }

        int count = enemyInventory.Count;

        for (int i = 0; i < count; i++) // 삭제된 적 보관함에서 삭제 
        {
            clone = enemyInventory.Peek();

            if (clone == null) enemyInventory.Dequeue();
            else break;
        }

        if (enemyInventory.Count >= 10) // 보관함에 빈 공간이 없음
        {
            Destroy(enemyInventory.Dequeue());
        }

        clone = Instantiate<GameObject>(cloneType, transform.position, transform.rotation);

        enemyInventory.Enqueue(clone);
    }

    public void GetScore(int gotScore)
    {
        score += gotScore;

        if(score > highScore)
        {
            highScore += gotScore;
        }
    }

    public void ReturnToTitle()
    {
        IEnumerator gameover = GameoverCoroutine();

        StartCoroutine(gameover);
    }

    IEnumerator GameoverCoroutine()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.Instance.MoveScene("Title");
        PanelManager.Instance.Open(Panel.Result);
    }

    public void ResetGame()
    {
        score = 0;
        highScore = preHighScore;
    }
}
