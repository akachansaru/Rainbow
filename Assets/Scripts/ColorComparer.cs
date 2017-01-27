using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ColorComparer : IComparer<GameObject> {

     public int Compare(GameObject x, GameObject y) {
        Color a = ColorManager.colorToMaterial.GetBySecond(x.GetComponent<MeshRenderer>().material.color);
        Color b = ColorManager.colorToMaterial.GetBySecond(y.GetComponent<MeshRenderer>().material.color);
        return (int)a - (int)b;
    }
}
