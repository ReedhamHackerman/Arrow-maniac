using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class UI_PlayerManager : MonoBehaviour
{
    private int connectedPlayers;
    public int GetConnectedPlayers => connectedPlayers;

    private Dictionary<int, UIInputManager> playerInputs = new Dictionary<int, UIInputManager>();
    public Dictionary<int, UIInputManager> GetAllPlayersInput => playerInputs;

    public void InitializeAllConnectedPlayers()
    {
        connectedPlayers = ReInput.controllers.joystickCount;

        if (connectedPlayers > 0)
        {
            for (int i = 0; i < connectedPlayers; i++)
            {
                Player p = ReInput.players.GetPlayer(i);
                UIInputManager input = new UIInputManager(p);

                playerInputs.Add(i, input);
            }
        }
    }
}
