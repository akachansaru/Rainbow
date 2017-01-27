using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBank : MonoBehaviour {
    public int maxCubes;

    public static CubeBank cubeBank;
    public static GameObject cubePrefab;
    public static float cubeSize;

    /// <summary>
    /// All available cubes for the game. When they are gone, the game is over.
    /// </summary>
    private List<Color> gameColors;

    private void Awake() {
        cubeBank = this;
        cubePrefab = Resources.Load("Prefabs/Cube") as GameObject;
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

    public List<Color> DealColors(int numCubes) {
        List<Color> colors = gameColors.GetRange(0, numCubes);
        gameColors.RemoveRange(0, numCubes);
        return colors;
    }
}
