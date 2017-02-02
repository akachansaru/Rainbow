using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static bool paused = false;

    public GameObject optionsPanel;

    public void OpenOptions() {
        optionsPanel.SetActive(true);
        paused = true;
    }

    public void CloseOptions() {
        optionsPanel.SetActive(false);
        paused = false;
    }
}
