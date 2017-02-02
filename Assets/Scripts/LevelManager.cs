using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static bool paused = false;

    public GameObject optionsCanvas;

    public void OpenOptions() {
        optionsCanvas.SetActive(true);
        paused = true;
    }

    public void CloseOptions() {
        optionsCanvas.SetActive(false);
        paused = false;
    }
}
