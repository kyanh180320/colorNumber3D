using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class ObjectData
{
    public Vector3 position;
    public Material material;
    public int ID;
}
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class ObjectCollection : ScriptableObject
{
    public List<ObjectData> objects = new List<ObjectData>();

}
