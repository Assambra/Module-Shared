using UnityEngine;


public class SpawnerManager : MonoBehaviour
{

    [Header("Serialize Fields Player")]
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private GameObject spawnPointPlayer = null;

    [Header("Serialize Fields Enemy")]
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private GameObject spawnPointEnemy = null;

    private void Awake()
    {
        if (playerPrefab != null)
        {
            if (spawnPointPlayer)
            {
                GameObject go = playerPrefab.gameObject;
                Instantiate(go, spawnPointPlayer.transform.position, spawnPointPlayer.transform.rotation);
                Debug.Log("Player spawned");
            }
            else
                Debug.LogError("You need to add a Player Spawn Point!");
        }
        else
            Debug.LogError("You need to add a Player Prefab!");

        if (enemyPrefab != null)
        {
            if (spawnPointEnemy)
            {
                GameObject go = enemyPrefab.gameObject;
                Instantiate(go, spawnPointEnemy.transform.position, spawnPointEnemy.transform.rotation);
                Debug.Log("Enemy spawned");
            }
            else
                Debug.LogError("You need to add a Enemy Spawn Point!");
        }
        else
            Debug.LogError("You need to add a Enemy Prefab!");
    }
}
