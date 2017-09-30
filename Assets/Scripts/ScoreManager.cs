using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// For use in level only
/// </summary>
public class ScoreManager : MonoBehaviour {
    public static ScoreManager scoreManager;
    public static int score = 0;

    public Text scoreText;
    public GameObject gameOverPanel;
    public Text finalScoreText;

    private bool outOfMoves = false;
    private bool savedScore = false;

    public bool OutOfMoves {
        set { outOfMoves = value; }
    }

    void Awake() {
        scoreManager = this;
    }

    void Update() {
        if (GameOver() && !savedScore) {
            SavingSystem.savingSystem.AddScore(score);
            SavingSystem.savingSystem.Save();
            savedScore = true;
            if (score > SavingSystem.savingSystem.HighScores[3]) {
                // The new score is either 1st, 2nd, or 3rd
                finalScoreText.text = "New High Score! " + score;
            } else {
                finalScoreText.text = "Score: " + score;
            }
            gameOverPanel.SetActive(true);
            print("Game over");
        }
    }

    bool GameOver() {
        return (CubeBank.cubeBank.cubesRemaining == 0 && HandManager.handManager.remainingHandSize == 0) 
            || outOfMoves;
    }

    public int CalculateScore(GameObject cube, List<GameObject> neighbors, List<Ray> rays) {
        int cubeScore = 0;
        for (int i = 0; i < rays.Count; i++) {
            cubeScore += ScoreChain(rays[i], cube, neighbors[i], 1, 1);
        }
        score += cubeScore;
        scoreText.text = "Score: " + score;
        return cubeScore;
    }

    // TODO: Light up each cube that scores, in order, and play a sound
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
