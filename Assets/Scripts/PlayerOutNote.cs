using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOutNote : MonoBehaviour
{
    [SerializeField] TMP_Text playerNoteText;
    [SerializeField] Toggle toggle;

    public void UpdateText(string name)
    {
        playerNoteText.text = "Выбыл: " + name;
    }

    public bool GetIsOutState()
    {
        return toggle.isOn;
    }
}
