using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private int coinsInside;
    [SerializeField] private ItemsDatabase itemsDatabase;

    private void Start()
    {
        LevelProggress.maxCoinsOnLevel += coinsInside;
        UIPack.LevelProgressPanel.UpdateCoins();
    }

    public void SpawnCoins()
    {
        while (coinsInside > 0)
        {
            bool forceQuit = true;
            foreach (var item in itemsDatabase.coinsSpawnerItems)
            {
                if (coinsInside >= item.price)
                {
                    coinsInside -= item.price;
                    forceQuit = false;
                    SpawnCoinPrefab(item.prefab);
                    break;
                }
            }

            if (forceQuit)
                break;
        }
    }

    private void SpawnCoinPrefab(GameObject coinPrefab)
    {
        Instantiate(coinPrefab, transform.position + new Vector3(Random.Range(0, 1f), 0, Random.Range(0, 1f))
            , Quaternion.identity);
    }
}
