using UnityEngine;

public class Spawner : MonoBehaviour
{
    private void Awake()
    {
        GameTools.levelSpawner = this;
    }

    public void SpawnMeInPosition(Vector3 position, GameObject obj)
    {
        Instantiate(obj, position, Quaternion.identity);
    }
}
