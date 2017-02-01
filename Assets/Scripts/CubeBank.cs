using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeBank : MonoBehaviour {
    public int maxCubes;
    public Text cubesRemainingText;

    public static CubeBank cubeBank;
    public static GameObject cubePrefab;
    public static float cubeSize;
    public static Material grayMaterial;

    /// <summary>
    /// All available cubes for the game. When they are gone, the game is over.
    /// </summary>
    private List<Color> gameColors;

    public int cubesRemaining {
        get { return gameColors.Count; }
    }

    public bool DealColors(int numCubes, out List<Color> dealtColors) {
        if (gameColors.Count >= numCubes) {
            dealtColors = gameColors.GetRange(0, numCubes);
            gameColors.RemoveRange(0, numCubes);
            cubesRemainingText.text = "Cubes Remaining: " + gameColors.Count;
            return true;
        } else {
            dealtColors = null;
            return false;
        }
    }

    void Awake() {
        cubeBank = this;
        cubePrefab = Resources.Load("Prefabs/Cube") as GameObject;
        grayMaterial = Resources.Load(ColorManager.materialPath + "Gray") as Material;
        cubeSize = cubePrefab.transform.localScale.x;
        gameColors = RandomCubeColors(maxCubes);
    }

    List<Color> RandomCubeColors(int numCubes) {
        List<Color> colors = new List<Color>();
        for (int c = 0; c < numCubes; c++) {
            colors.Add((Color)Random.Range(0, 6));
        }
        return colors;
    }
}
