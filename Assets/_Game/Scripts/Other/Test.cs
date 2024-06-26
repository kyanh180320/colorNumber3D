using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    int count = 0;
    public GameObject cubePrefab;
    public Transform levelHi;
    public void Start()
    {
        LoadLevel();
    }
    
    
    

    // Update is called once per frame
    public void LoadLevel()
    {
        foreach(ObjectData obj in LevelManager.Ins.currentLevel.levelData.objects)
        {
            count++;
            Vector3 newPos = obj.position;
            GameObject cubeSpawn = Instantiate(cubePrefab, newPos, Quaternion.identity, levelHi.transform);
            cubeSpawn.GetComponent<Cube>().ID = obj.ID;
            LevelManager.Ins.cubesOfLevel.Add(cubeSpawn);
            
        }
        //transform.Rotate(0, 230, 0, Space.Self);

    }
    public void SetParent()
    {
        foreach(GameObject cube in LevelManager.Ins.cubesOfLevel)
        {
            cube.transform.SetParent(this.transform);
        }
    }
    
}
