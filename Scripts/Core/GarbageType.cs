using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GarbageType
{
    public string Name;
    [Range(0, 100)] public int Chance;
    public List<GameObject> Prefabs;
}