using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
    public List<GameObject> initialEnemies;
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        foreach (GameObject enemy in initialEnemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false);
                pool.Enqueue(enemy);
            }
        }
    }

    private void Start()
    {
        Debug.Log("Pooler baþlatýlýyor, düþmanlar aktif ediliyor.");
        foreach (GameObject enemy in initialEnemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
                Debug.Log(enemy.name + " aktif edildi.");
            }
        }
    }

    public GameObject GetEnemy()
    {
        if (pool.Count == 0) return null;
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        pool.Enqueue(obj);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}