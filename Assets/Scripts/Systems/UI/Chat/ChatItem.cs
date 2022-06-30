using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ChatItem : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;

    public void SetUP(ChatMessageInfo info)
    {
        text.text = info.messageText;
        image.color = info.authorColor;
    }
}
