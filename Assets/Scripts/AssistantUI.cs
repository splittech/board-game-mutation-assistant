using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AssistantLogic))]
public class AssistantUI : MonoBehaviour
{
    [SerializeField] GameObject playerOutNotePrefab;
    [SerializeField] GameObject playerPairNotePrefab;
    [SerializeField] GameObject outScrollContent;
    [SerializeField] GameObject pairsScrollContent;
    [SerializeField] TMP_InputField numberOfPlayersInputField;

    List<PlayerOutNote> playerOutNotes = new();
    List<PlayerPairNote> playerPairNotes = new();

    AssistantLogic assistantLogic;

    private void Awake()
    {
        assistantLogic = GetComponent<AssistantLogic>();
    }

    public void StartGame()
    {
        assistantLogic.SetStartPlayers(Convert.ToInt32(numberOfPlayersInputField.text));

        foreach (Player player in assistantLogic.players)
        {
            GameObject playerNoteObject = Instantiate(playerOutNotePrefab);
            playerNoteObject.transform.SetParent(outScrollContent.transform);

            PlayerOutNote playerNote = playerNoteObject.GetComponent<PlayerOutNote>();
            playerNote.UpdateText(assistantLogic.GetPlayerNameByNumber(player.number));
            playerOutNotes.Add(playerNote);
        }
    }

    public void GeneratePairs()
    {
        List<Player> attackers;
        List<Player> defenders;
        Player noPairPLayer;

        foreach (var playerPairNote in playerPairNotes)
        {
            Destroy(playerPairNote.gameObject);
        }
        playerPairNotes.Clear();

        for (int i = 0; i < playerOutNotes.Count; i++)
        {
            assistantLogic.ChangePlayerOut(i, playerOutNotes[i].GetIsOutState());
        }

        (attackers, defenders, noPairPLayer) = assistantLogic.RandomizePairs();

        for (int i = 0; i < attackers.Count; i++)
        {
            GameObject playerPairNoteObj = Instantiate(playerPairNotePrefab);
            playerPairNoteObj.transform.SetParent(pairsScrollContent.transform);

            PlayerPairNote playerPairNote = playerPairNoteObj.GetComponent<PlayerPairNote>();
            playerPairNotes.Add(playerPairNote);

            playerPairNote.UpdateText(
                assistantLogic.GetPlayerNameByNumber(attackers[i].number),
                assistantLogic.GetPlayerNameByNumber(defenders[i].number),
                false
            );
        }

        if (noPairPLayer != null)
        {
            GameObject playerPairNoteObj = Instantiate(playerPairNotePrefab);
            playerPairNoteObj.transform.SetParent(pairsScrollContent.transform);

            PlayerPairNote playerPairNote = playerPairNoteObj.GetComponent<PlayerPairNote>();
            playerPairNotes.Add(playerPairNote);

            if (assistantLogic.lastOutPlayer != null)
            {
                playerPairNote.UpdateText(
                    assistantLogic.GetPlayerNameByNumber(assistantLogic.lastOutPlayer.number),
                    assistantLogic.GetPlayerNameByNumber(noPairPLayer.number),
                    true
                );
            }
            else
            {
                playerPairNote.UpdateText(
                    "...",
                    assistantLogic.GetPlayerNameByNumber(noPairPLayer.number),
                    true
                );
            }
        }
    }
}
