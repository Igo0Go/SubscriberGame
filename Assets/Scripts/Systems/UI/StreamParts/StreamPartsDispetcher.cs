using UnityEngine;
using UnityEngine.Events;

public class StreamPartsDispetcher : MonoBehaviour
{
    [SerializeField]
    private GameObject streamPartsContainer;

    [SerializeField]
    private bool blocked;

    [SerializeField]
    private UnityEvent onFirstActivation;

    private void Start()
    {
        if(blocked)
        {
            streamPartsContainer.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !GameCenter.GlobalPause && !blocked)
        {
            streamPartsContainer.SetActive(!streamPartsContainer.activeSelf);

            onFirstActivation.Invoke();
        }
    }

    public void Unblock()
    {
        blocked = false;
        UIPack.NotificationPanel.ShowNotification("Нажмите TAB, чтобы открыть элементы стрима.");
    }
}
