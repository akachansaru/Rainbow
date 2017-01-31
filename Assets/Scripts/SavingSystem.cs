using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class SavingSystem : MonoBehaviour {

    public static SavingSystem savingSystem;
    public static SaveValues savedData;

    private string saveFilePath;

    void Awake() {
        savingSystem = this;
        saveFilePath = Application.persistentDataPath + "/RainbowSaveValues.sheep";
        Load();
    }

    public void AddScore(int score) {
        if (savedData.highScores.Count == SaveValues.maxScoreCapacity - 1) {
            savedData.highScores.Add(score);
            savedData.highScores.Sort();
            savedData.highScores.RemoveAt(0);
            savedData.highScores.Reverse();
            print("Added score 1. Count: " + savedData.highScores.Count);
            print("Capacity: " + savedData.highScores.Capacity);
        } else if (savedData.highScores.Count < SaveValues.maxScoreCapacity) {
            savedData.highScores.Add(score);
            print("Added score 2. Count: " + savedData.highScores.Count);
            print("Capacity: " + savedData.highScores.Capacity);
        } else {
            print("Didn't add score. Count: " + savedData.highScores.Count);
            print("Capacity: " + savedData.highScores.Capacity);
        }
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveFilePath);
        bf.Serialize(file, savedData);
        file.Close();
        Debug.Log("Saved to " + saveFilePath);
    }

    void Load() {
        if (File.Exists(saveFilePath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveFilePath, FileMode.Open);
            savedData = (SaveValues)bf.Deserialize(file);
            file.Close();
            savedData.highScores.Sort();
            savedData.highScores.Reverse();
            print("Loaded from " + saveFilePath);
        } else {
            NewGame();
        }
        savedData.highScores.ForEach(score => { print(score + " "); });
    }

    void NewGame() {
        savedData = new SaveValues();
        savedData.highScores = new List<int>();
        savedData.highScores.Add(0);
        savedData.highScores.Add(2);
        savedData.highScores.Add(1);
        savedData.highScores.Sort();
        savedData.highScores.Reverse();
        print("New game");
        print("capacity: " + savedData.highScores.Capacity);
        print("count: " + savedData.highScores.Count);
    }
}
