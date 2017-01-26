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
    public static ArrayList gameCubes;

    private void Awake() {
        cubeBank = this;
        cubePrefab = Resources.Load("Prefabs/Cube") as GameObject;
        cubeSize = cubePrefab.transform.localScale.x;
        gameCubes = RandomCubeColors(maxCubes);
    }

    private void Start() {
        
    }

    ArrayList RandomCubeColors(int numCubes) {
        ArrayList colors = new ArrayList();
        for (int c = 0; c < numCubes; c++) {
            colors.Add((Color)Random.Range(0, 6));
        }
        return colors;
    }

    public Color[] DealCubes(int numCubes) {
        //Color[] cubes = new Color[numCubes];
        //for (int c = 0; c < numCubes; c++) {
        //    //int random = Random.Range(0, gameCubes.Count);
        //    cubes[c] = (Color)gameCubes[random];
        //    gameCubes.RemoveAt(random);
        //}
        Color[] cubes = (Color[])gameCubes.GetRange(0, numCubes).ToArray(typeof(Color));
        gameCubes.RemoveRange(0, numCubes);
        return cubes;
    }
}
