using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

public enum Color { Red, Orange, Yellow, Green, Blue, Purple, White };

public class ColorManager : MonoBehaviour {
    public static ColorManager colorManager;
    public static BiDictionaryOneToOne<Color, UnityEngine.Color> colorToMaterial = new BiDictionaryOneToOne<Color, UnityEngine.Color>();

    private static CircularDoublyLinkedList<Color> colorSequence = new CircularDoublyLinkedList<Color>();

    private string materialPath = "Materials/";

    private void Awake() {
        colorManager = this;
        LoadDictionary();
        Debug.Log("Dict = " + colorToMaterial.GetByFirst(Color.Yellow));
        Debug.Log("Dict = " + colorToMaterial.GetBySecond((Resources.Load(materialPath + "Yellow") as Material).color));
        CreateColorSequence();
    }

    public bool InSequence(Color colorOne, Color colorTwo) {
        return ((colorOne == Color.White || colorTwo == Color.White) || 
            (colorSequence.Find(colorOne).next.Data.Equals(colorTwo) 
            || colorSequence.Find(colorOne).prev.Data.Equals(colorTwo)));
    }

    void LoadDictionary() {
        colorToMaterial.Add(Color.Red, (Resources.Load(materialPath + "Red") as Material).color);
        colorToMaterial.Add(Color.Orange, (Resources.Load(materialPath + "Orange") as Material).color);
        colorToMaterial.Add(Color.Yellow, (Resources.Load(materialPath + "Yellow") as Material).color);
        colorToMaterial.Add(Color.Green, (Resources.Load(materialPath + "Green") as Material).color);
        colorToMaterial.Add(Color.Blue, (Resources.Load(materialPath + "Blue") as Material).color);
        colorToMaterial.Add(Color.Purple, (Resources.Load(materialPath + "Purple") as Material).color);
        colorToMaterial.Add(Color.White, (Resources.Load(materialPath + "White") as Material).color);
    }

    void CreateColorSequence() {
        colorSequence.Add(Color.Red);
        colorSequence.Add(Color.Orange);
        colorSequence.Add(Color.Yellow);
        colorSequence.Add(Color.Green);
        colorSequence.Add(Color.Blue);
        colorSequence.Add(Color.Purple);
    }
}
