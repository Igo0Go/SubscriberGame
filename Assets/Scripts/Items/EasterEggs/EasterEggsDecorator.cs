using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggsDecorator : MonoBehaviour
{
    [SerializeField] private List<GameObject> easterEggsObjects;

    private IEnumerator Start()
    {
        yield return null;
        for (int i = 0; i < easterEggsObjects.Count; i++)
        {
            easterEggsObjects[i].SetActive(StatsHolder.unblockedEasterEggsIds.Contains(i));
        }
    }
}
