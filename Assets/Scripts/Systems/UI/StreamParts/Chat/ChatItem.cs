using UnityEngine.UI;
using UnityEngine;

public class ChatItem : MonoBehaviour
{
    [SerializeField] private Text author;
    [SerializeField] private Text messageText;
    [SerializeField] private Image image;

    public void SetUP(ChatMessageInfo info)
    {
        author.text = info.authorName;
        if(info.useColor)
        {
            author.color = info.authorColor;
        }
        messageText.text = info.messageText;
        image.color = info.authorColor;
    }
}