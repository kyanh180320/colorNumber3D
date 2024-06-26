using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{
    // Start is called before the first frame update
    public ObjectCollection levelData;
    public List<Material> materialsColorFill;
    public List<int> cubeIdForDarkGrey;
    public List<int> cubeIdForLightGrey;
    public List<int> cubeIdForBlack;
    public List<int> cubeIdForWhite;
    public List<List<GameObject>> listOfListHintNumber;
    

}
