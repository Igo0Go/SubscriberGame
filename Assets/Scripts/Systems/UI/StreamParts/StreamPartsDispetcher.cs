using UnityEngine;

public class StreamPartsDispetcher : MonoBehaviour
{
    [SerializeField]
    private GameObject streamPartsContainer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !GameCenter.GlobalPause)
        {
            streamPartsContainer.SetActive(!streamPartsContainer.activeSelf);
        }
    }
}
