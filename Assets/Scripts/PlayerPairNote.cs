using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PlayerPairNote : MonoBehaviour
{
    [SerializeField] TMP_Text player1Text;
    [SerializeField] TMP_Text player2Text;

    [SerializeField] GameObject attackImage;
    [SerializeField] GameObject deadImage;

    public void UpdateText(string firstPlayerName, string secondPlayerName, bool isDead)
    {
        player1Text.text = firstPlayerName;
        player2Text.text = secondPlayerName;

        attackImage.SetActive(!isDead);
        deadImage.SetActive(isDead);
    }
}
