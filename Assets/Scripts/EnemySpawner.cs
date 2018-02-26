using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float enemySpeed = 1f;
    public float width = 10f;
    public float height = 5f;
    public float spawnDelay = 2f;

    float enemyPadding;
    float xmin;
    float xmax;
    bool reverseDirection = false;

    // Use this for initialization
    void Start () {
        enemyPadding = 0.5f * width;
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftMost.x + enemyPadding;
        xmax = rightMost.x - enemyPadding;

        SpawnUntilFull();
    }

    private void CreateEnemies()
    {
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();
        if(freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
        }
        if(NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }  
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    // Update is called once per frame
    void Update () {

        if (!reverseDirection)
        {
            transform.position += Vector3.left * enemySpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.right * enemySpeed * Time.deltaTime;
        }
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        if(newX == xmin || newX == xmax)
        {
            reverseDirection = !reverseDirection;
        } 
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        if(AllMembersDead())
        {
            CreateEnemies();
        }
    }

    Transform NextFreePosition()
    {
        foreach(Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;
    }

    bool AllMembersDead()
    {
        foreach(Transform childPositionGameObject in transform)
        {
            if(childPositionGameObject.childCount > 0)
            {
                return false;
            }
        }
        return true;
    }
}
