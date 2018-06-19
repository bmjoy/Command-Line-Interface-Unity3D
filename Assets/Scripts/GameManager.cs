using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[ConsoleCommand]
    public void Ping()
    {
        Debug.Log("Pong");
    }

    [ConsoleCommand]
    public void Ping2()
    {
        Debug.Log("Pong2");
    }

    [ConsoleCommand]
    public void SetVariable(int x = 0)
    {
        Debug.Log("Variable set to " + x);
    }

    [ConsoleCommand("Misc.Random")]
    public void PrintRandomNumber()
    {
        Debug.Log(Random.Range(0, 10000));
    }

    [ConsoleCommand("Misc.Random")]
    public int GetRandomNumber()
    {
        return Random.Range(0, 10000);
    }

}
