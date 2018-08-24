using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CLI;

public class GameManager : MonoBehaviour {

    [ConsoleCommand]
    public void Ping()
    {
        Debug.Log("Pong");
        CLIManager.Log("Pong");
    }

    [ConsoleCommand]
    public void Ping2()
    {
        Debug.Log("Pong2");
    }

    [ConsoleCommand]
    public void SetInt(int x)
    {
        Debug.Log("Variable set to " + x);
    }

    [ConsoleCommand]
    public void SetFloat(float x)
    {
        Debug.Log("Variable set to " + x);
    }

    [ConsoleCommand]
    public void SetString(string x)
    {
        Debug.Log("Variable set to " + x);
    }

    [ConsoleCommand]
    public void SetMultiple(int x, string s)
    {
        Debug.Log("Variable set to " + x + " and " + s);
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

    [ConsoleCommand("Misc.Random.Numbers")]
    public void PrintRandomInt()
    {
        Debug.Log(Random.Range(0, 10000));
    }

}
