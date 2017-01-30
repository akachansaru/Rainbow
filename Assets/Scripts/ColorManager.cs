using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

public enum Color { Red, Orange, Yellow, Green, Blue, Purple, White };

public class ColorManager : MonoBehaviour {
    public static ColorManager colorManager;
    public static BiDictionaryOneToOne<Color, string> colorToMaterial = new BiDictionaryOneToOne<Color, string>();
    public static CircularDoublyLinkedList<Color> colorSequence = new CircularDoublyLinkedList<Color>();
    public static string materialPath = "Materials/";

    private void Awake() {
        colorManager = this;
        LoadDictionary();
        CreateColorSequence();
    }

    public bool InSequence(GameObject cubeOne, GameObject cubeTwo) {
        Color colorOne = ColorManager.colorToMaterial.GetBySecond(cubeOne.GetComponent<MeshRenderer>().material.name.Split(' ')[0]);
        Color colorTwo = ColorManager.colorToMaterial.GetBySecond(cubeTwo.GetComponent<MeshRenderer>().material.name.Split(' ')[0]);
        return ((colorOne == Color.White || colorTwo == Color.White) ||
            (colorSequence.Find(colorOne).next.Data.Equals(colorTwo)
            || colorSequence.Find(colorOne).prev.Data.Equals(colorTwo)));
    }

    public bool InStrictSequence(GameObject cubeOne, GameObject cubeTwo, GameObject cubeThree) {
        Color colorOne = ColorManager.colorToMaterial.GetBySecond(cubeOne.GetComponent<MeshRenderer>().material.name.Split(' ')[0]);
        Color colorTwo = ColorManager.colorToMaterial.GetBySecond(cubeTwo.GetComponent<MeshRenderer>().material.name.Split(' ')[0]);
        Color colorThree = ColorManager.colorToMaterial.GetBySecond(cubeThree.GetComponent<MeshRenderer>().material.name.Split(' ')[0]);
        return ((colorSequence.Find(colorOne).next.Data.Equals(colorTwo)
            && colorSequence.Find(colorTwo).next.Data.Equals(colorThree))
            || (colorSequence.Find(colorOne).prev.Data.Equals(colorTwo)
            && colorSequence.Find(colorTwo).prev.Data.Equals(colorThree)));
    }

    void LoadDictionary() {
        colorToMaterial.Add(Color.Red, "Red");
        colorToMaterial.Add(Color.Orange, "Orange");
        colorToMaterial.Add(Color.Yellow, "Yellow");
        colorToMaterial.Add(Color.Green, "Green");
        colorToMaterial.Add(Color.Blue, "Blue");
        colorToMaterial.Add(Color.Purple, "Purple");
        colorToMaterial.Add(Color.White, "White");
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
