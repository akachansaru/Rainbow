using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// For use in level only
/// </summary>
[RequireComponent(typeof(Text))]
public class ScoreManager : MonoBehaviour {
    public static ScoreManager scoreManager;
    public static int score = 0;

    private Text scoreText;
    private bool savedScore = false;

    void Awake() {
        scoreManager = this;
        scoreText = GetComponent<Text>();
    }

    void Update() {
        if (GameOver() && !savedScore) {
            SavingSystem.savingSystem.AddScore(score);
            SavingSystem.savingSystem.Save();
            savedScore = true;
            print("Game over");
            // Bring up a panel or something with the final score then take them to high score scene
            // Option to play again
            // Option for main menu
        }
    }

    bool GameOver() {
        return (CubeBank.cubeBank.cubesRemaining == 0 && HandManager.handManager.remainingHandSize == 0) 
            || NoPossibleMoves();
    }

    bool NoPossibleMoves() {
        return false;
    }

    public void CalculateScore(GameObject cube, List<GameObject> neighbors, List<Ray> rays) {
        for (int i = 0; i < rays.Count; i++) {
            score += ScoreChain(rays[i], cube, neighbors[i], 1, 1);
            scoreText.text = "Score: " + score;
        }
    }

    int ScoreChain(Ray ray, GameObject firstCube, GameObject secondCube, int chainScore, int chainNumber) {
        ray.origin += ray.direction;
        print("chainNumber: " + chainNumber + " chainScore: " + chainScore);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1) && ColorManager.colorManager.InStrictSequence(firstCube, secondCube, hitInfo.transform.parent.gameObject)) {
            chainNumber++;
            chainScore += chainNumber;
            return ScoreChain(ray, secondCube, hitInfo.transform.parent.gameObject, chainScore, chainNumber);
        } else {
            return chainScore;
        }
    }
}
