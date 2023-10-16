using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateRoomManager : MonoBehaviour
{
    public CodeMatchmakingLobbyController cmlc;

    public void Share()
    {
        new NativeShare().SetText("Hello! This game is Shoot pong. Link - https://cutt.ly/vycDJc6 Use " + cmlc.roomCodeToShare + " code to join my room and let's play a match. It's gonna be fun!").Share();
    }
}
