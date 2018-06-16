using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[ConsoleCommand]
    public void Ping()
    {
        Debug.Log("Pong");
    }

}
