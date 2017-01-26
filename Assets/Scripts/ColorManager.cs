using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;

public enum Color { Red, Orange, Yellow, Green, Blue, Purple };

public class ColorManager : MonoBehaviour {
    public static Dictionary<Color, Material> colorToMaterial = new Dictionary<Color, Material>();

    private string materialPath = "Materials/";

    private void Awake() {
        LoadDictionary();
    }
    void LoadDictionary() {
        colorToMaterial.Add(Color.Red, Resources.Load(materialPath + "Red") as Material);
        colorToMaterial.Add(Color.Orange, Resources.Load(materialPath + "Orange") as Material);
        colorToMaterial.Add(Color.Yellow, Resources.Load(materialPath + "Yellow") as Material);
        colorToMaterial.Add(Color.Green, Resources.Load(materialPath + "Green") as Material);
        colorToMaterial.Add(Color.Blue, Resources.Load(materialPath + "Blue") as Material);
        colorToMaterial.Add(Color.Purple, Resources.Load(materialPath + "Purple") as Material);
    }
}
