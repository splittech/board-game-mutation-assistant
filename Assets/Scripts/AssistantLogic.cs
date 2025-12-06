using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int number;
    public bool isOut;
    public int wasAttackerCount;
    public int wasDefenderCount;
}

public class AssistantLogic : MonoBehaviour
{
    public Player[] players;

    public Player lastOutPlayer;

    public void ChangePlayerOut(int number, bool isOut)
    {
        players[number].isOut = isOut;
        if (isOut)
        {
            lastOutPlayer = players[number];
        }
    }

    public void SetStartPlayers(int numberOfPlayers)
    {
        players = new Player[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = new Player();
            players[i].number = i;
            players[i].isOut = false;
            players[i].wasAttackerCount = 0;
            players[i].wasDefenderCount = 0;
            
        }
    }

    public (List<Player>, List<Player>, Player) RandomizePairs()
    {
        // Отбираем только не выбывших игроков.
        List<Player> notOutPlayersSorted = new List<Player>();
        List<Player> notOutPlayers = new List<Player>();

        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].isOut)
            {
                notOutPlayersSorted.Add(players[i]);
            }
        }

        // Перемешиваем список не выбывших игроков случайным образом.
        System.Random random = new System.Random();
        int numberOfPlayers = notOutPlayersSorted.Count;
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Player randomPlayer = notOutPlayersSorted[random.Next(notOutPlayersSorted.Count)];
            notOutPlayers.Add(randomPlayer);
            notOutPlayersSorted.Remove(randomPlayer);
        }

        // Выбираем в качестве атакующих игроков половину всех игроков, кто меньше всего был в атаке.
        List<Player> attackers = new List<Player>();
        for (int i = 0; i < numberOfPlayers / 2; i++)
        {
            Player minAttacksPlayer = notOutPlayers[0];
            for (int j = 0; j < notOutPlayers.Count; j++)
            {
                if (notOutPlayers[j].wasAttackerCount < minAttacksPlayer.wasAttackerCount)
                {
                    minAttacksPlayer = notOutPlayers[j];
                }
            }
            minAttacksPlayer.wasAttackerCount++;
            attackers.Add(minAttacksPlayer);
            notOutPlayers.Remove(minAttacksPlayer);
        }

        // Выбираем в качестве оставшихся игроков по порядку, тех кто меньше всего был в защите.
        List<Player> defenders = new List<Player>();
        numberOfPlayers += numberOfPlayers % 2;
        for (int i = 0; i < numberOfPlayers / 2; i++)
        {
            Player minBattlesPlayer = notOutPlayers[0];
            for (int j = 0; j < notOutPlayers.Count; j++)
            {
                if (notOutPlayers[j].wasAttackerCount + notOutPlayers[j].wasDefenderCount <
                    minBattlesPlayer.wasAttackerCount + minBattlesPlayer.wasDefenderCount)
                {
                    minBattlesPlayer = notOutPlayers[j];
                }
            }
            minBattlesPlayer.wasDefenderCount++;
            defenders.Add(minBattlesPlayer);
            notOutPlayers.Remove(minBattlesPlayer);
        }

        // По такому принципу, люди, которые участвовали в большем количестве битв, будут в конце списков.
        // Если количество людей в списках неравное (одному пары не хватило), то оставшийся человек в конце какого-либо из
        // списков может драться с персонажем вылетевшего игрока или не драться вообще.
        Player noPairPlayer = null;
        if (attackers.Count > defenders.Count)
        {
            noPairPlayer = attackers[attackers.Count - 1];
            noPairPlayer.wasAttackerCount--;
            attackers.Remove(noPairPlayer);
        }
        else if (defenders.Count > attackers.Count)
        {
            noPairPlayer = defenders[defenders.Count - 1];
            noPairPlayer.wasDefenderCount--;
            defenders.Remove(noPairPlayer);
        }

        return (attackers, defenders, noPairPlayer);
    }

    public string GetPlayerNameByNumber(int playerNumber)
    {
        string[] livingCreatures = new string[]
        {
            "Аксолотль",
            "Тихоходка",
            "Осьминог",
            "Хамелеон",
            "Раффлезия",
            "Стеклянница",
            "Нарвал",
            "Киви",
            "Рыба-капля",
            "Кордицепс",
        };
        if (playerNumber >= livingCreatures.Length)
        {
            return "Неопознанное существо " + playerNumber.ToString();
        }
        else
        {
            return livingCreatures[playerNumber];
        }
    }
}
