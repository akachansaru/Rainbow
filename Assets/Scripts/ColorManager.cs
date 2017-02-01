using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

public enum Color { Red, Orange, Yellow, Green, Blue, Purple, White, Gray };

public class ColorManager : MonoBehaviour {
    public static ColorManager colorManager;
    public static BiDictionaryOneToOne<Color, string> colorToMaterial = new BiDictionaryOneToOne<Color, string>();
    public static CircularDoublyLinkedList<Color> colorSequence = new CircularDoublyLinkedList<Color>();
    public static string materialPath = "Materials/";

    private void Awake() {
        colorManager = this;
        if (colorSequence.size == 0) {
            LoadDictionary();
            colorSequence = CreateColorSequence();
        }
    }

    public bool InSequence(GameObject cubeOne, GameObject cubeTwo) {
        Color colorOne = colorToMaterial.GetBySecond(cubeOne.GetComponent<MeshRenderer>().material.name.Split(' ')[0]);
        Color colorTwo = colorToMaterial.GetBySecond(cubeTwo.GetComponent<MeshRenderer>().material.name.Split(' ')[0]);
        return (colorOne != Color.Gray && colorTwo != Color.Gray) && ((colorOne == Color.White || colorTwo == Color.White) ||
            (colorSequence.Find(colorOne).next.Data.Equals(colorTwo)
            || colorSequence.Find(colorOne).prev.Data.Equals(colorTwo)));
    }

    public bool InStrictSequence(GameObject cubeOne, GameObject cubeTwo, GameObject cubeThree) {
        Color colorOne = colorToMaterial.GetBySecond(cubeOne.GetComponent<MeshRenderer>().material.name.Split(' ')[0]);
        Color colorTwo = colorToMaterial.GetBySecond(cubeTwo.GetComponent<MeshRenderer>().material.name.Split(' ')[0]);
        Color colorThree = colorToMaterial.GetBySecond(cubeThree.GetComponent<MeshRenderer>().material.name.Split(' ')[0]);
        return ((colorSequence.Find(colorOne).next.Data.Equals(colorTwo)
            && colorSequence.Find(colorTwo).next.Data.Equals(colorThree))
            || (colorSequence.Find(colorOne).prev.Data.Equals(colorTwo)
            && colorSequence.Find(colorTwo).prev.Data.Equals(colorThree)));
    }

    void LoadDictionary() {
        colorToMaterial.TryAdd(Color.Red, "Red");
        colorToMaterial.TryAdd(Color.Orange, "Orange");
        colorToMaterial.TryAdd(Color.Yellow, "Yellow");
        colorToMaterial.TryAdd(Color.Green, "Green");
        colorToMaterial.TryAdd(Color.Blue, "Blue");
        colorToMaterial.TryAdd(Color.Purple, "Purple");
        colorToMaterial.TryAdd(Color.White, "White");
        colorToMaterial.TryAdd(Color.Gray, "Gray");
    }

    CircularDoublyLinkedList<Color> CreateColorSequence() {
        CircularDoublyLinkedList<Color> sequence = new CircularDoublyLinkedList<Color>();
        sequence.Add(Color.Red);
        sequence.Add(Color.Orange);
        sequence.Add(Color.Yellow);
        sequence.Add(Color.Green);
        sequence.Add(Color.Blue);
        sequence.Add(Color.Purple);
        return sequence;
    }
}
