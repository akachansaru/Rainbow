using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
    public GameObject scoreContent;

    private GameObject highScorePrefab;

	void Start () {
        highScorePrefab = Resources.Load("Prefabs/HighScoreText") as GameObject;
        List<int> highScores = SavingSystem.savingSystem.HighScores;
        for (int i = 0; i < highScores.Count; i++) {
            GameObject scoreObject = Instantiate(highScorePrefab, scoreContent.transform);
            if (i == 0) {
                scoreObject.GetComponent<Text>().text = "1st Place: " + highScores[i].ToString();
            } else if (i == 1) {
                scoreObject.GetComponent<Text>().text = "2nd Place: " + highScores[i].ToString();
            } else if (i == 2) {
                scoreObject.GetComponent<Text>().text = "3rd Place: " + highScores[i].ToString();
            } else {
                scoreObject.GetComponent<Text>().text = (i + 1).ToString() + ": " + highScores[i].ToString();
            }
        }
	}
}
