using UnityEngine;
using UnityEngine.UI;

public class EasterEggItemButton : MonoBehaviour
{
    [SerializeField]
    private Text nameField;

    private int id;

    public void Init(int id, string name)
    {
        this.id = id;
        nameField.text = name;
    }

    public void OnClick()
    {
        UIPack.EasterEggSystem.ShowItemWithId(id);
    }
}
