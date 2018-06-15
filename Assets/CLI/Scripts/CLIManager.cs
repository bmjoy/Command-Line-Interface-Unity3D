using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CLIManager : MonoBehaviour {

    private void Start()
    {
        var eventSystem = FindObjectOfType<EventSystem>();

        if (eventSystem == null)
            Debug.LogError("No UI EventSystem is present in the current scene!");
    }
}
