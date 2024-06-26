using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    //public TouchController touchController;
    public List<Level> levels;
    public TouchController touchController;
    public Level currentLevel;
    public List<Material> materialsNumberOriginal;
    public List<Material> materialHintsNumber;
    public GameObject cubePrefab;
    public Transform levelHierachy;
    public List<GameObject> cubesOfLevel = new List<GameObject>();
    private int numberCurrentSelect = 100;
    public bool isClickButton;
    public bool isPaint;
    public int checkWinNumber = -1;

    //
    public Material blackOriginal;
    public Material darkGreyOriginal;
    public Material lightGreyOrigianl;
    public Material whiteOriginal;
    public Material greyHint;
    public void Start()
    {
        OnLoadLevel(5);
        OnInit();
    }

    //khoi tao trang thai bat dau game
    public void OnInit()
    {
        //player.OnInit();
    }

    //reset trang thai khi ket thuc game
    public void OnReset()
    {
        //player.OnDespawn();
        //for (int i = 0; i < bots.Count; i++)

        //    bots[i].OnDespawn();
        //}

        //bots.Clear();
        //SimplePool.CollectAll();
    }

    //tao prefab level moi
    public void OnLoadLevel(int level)
    {
        //if (currentLevel != null)
        //{
        //    Destroy(currentLevel);
        //}
        currentLevel = levels[level];
        foreach (ObjectData obj in currentLevel.levelData.objects)
        {
            Vector3 newPos = obj.position;

            GameObject cubeSpawn = Instantiate(cubePrefab, newPos, Quaternion.identity, levelHierachy);
            cubeSpawn.GetComponent<Cube>().ID = obj.ID;

            cubesOfLevel.Add(cubeSpawn);

        }
        checkWinNumber = 0;
        FillColorOriginal();

        //SetCubeNeighbors();


        levelHierachy.Rotate(0, 230, 0, Space.Self);
    }
    public void SetNumberCurrentSelect(int number)
    {
        numberCurrentSelect = number;
    }
    public int getNumberCurrentSelect()
    {
        return numberCurrentSelect;
    }
    public void FillColorOriginal()
    {
        if (cubesOfLevel.Count == 0)
        {
            print("level khong co cube");
            return;
        }
    
        foreach (GameObject cube in cubesOfLevel)
        {
            int id = cube.gameObject.GetComponent<Cube>().ID;
            if (cube.gameObject.GetComponent<Cube>().isFill == true)
            {
                continue;
            }
            else if (IsNumberInList(id, currentLevel.cubeIdForBlack) && currentLevel.cubeIdForBlack.Count > 0)
            {
                cube.gameObject.GetComponent<Renderer>().material = blackOriginal;
            }
            else if (IsNumberInList(id, currentLevel.cubeIdForDarkGrey) && currentLevel.cubeIdForDarkGrey.Count > 0)
            {
                cube.gameObject.GetComponent<Renderer>().material = darkGreyOriginal;
            }
            else if (IsNumberInList(id, currentLevel.cubeIdForLightGrey) && currentLevel.cubeIdForLightGrey.Count > 0)
            {
                cube.gameObject.GetComponent<Renderer>().material = lightGreyOrigianl;
            }
            else if (IsNumberInList(id, currentLevel.cubeIdForWhite) && currentLevel.cubeIdForWhite.Count > 0)
            {
                cube.gameObject.GetComponent<Renderer>().material = whiteOriginal;
            }
        }
    }
    public void FillNumberOriginal()
    {
        if (cubesOfLevel.Count == 0)
        {
            print("level khong co cube");
            return;
        }
        foreach (GameObject cube in cubesOfLevel)
        {
            if (cube.gameObject.GetComponent<Cube>().isFill == true)
            {
                continue;
            }
            int id = cube.gameObject.GetComponent<Cube>().ID;
            Renderer cubePrefabRenderer = cube.GetComponent<Renderer>();
            cubePrefabRenderer.material = materialsNumberOriginal[id];

        }
    }
    public void FillHintNumber()
    {
        if (cubesOfLevel.Count == 0)
        {
            print("level khong co cube");
            return;
        }
        foreach(GameObject cube in cubesOfLevel)
        {
            if (cube.gameObject.GetComponent<Cube>().isFill)
            {
                continue;
            }
            else if (cube.gameObject.GetComponent<Cube>().ID == numberCurrentSelect)
            {
                int id = cube.gameObject.GetComponent<Cube>().ID;
                Renderer cubePrefabRenderer = cube.GetComponent<Renderer>();
                cubePrefabRenderer.material = materialHintsNumber[id];
            }
            else if(cube.gameObject.GetComponent<Cube>().ID != numberCurrentSelect)
            {
                int id = cube.gameObject.GetComponent<Cube>().ID;
                Renderer cubePrefabRenderer = cube.GetComponent<Renderer>();
                cubePrefabRenderer.material = materialsNumberOriginal[id];
            }
        }
    }
    bool IsNumberInList(int x, List<int> list)
    {
        return list.Contains(x);
    }
    public void CheckFillHintNumber()
    {
        if (touchController.camera.fieldOfView < 25)
        {
            FillHintNumber();
        }
    }

 
    public void SetStateButtonPaint(bool isPaint)
    {
        this.isPaint = isPaint;
    }

    public void OnNumberButtonClick(int id)
    {
        //Cube targetCube = touchController.FindClosestCubeWithId(id);
        //if (targetCube != null)
        //{
        //    StartCoroutine(touchController.RotateAndZoomToCube(targetCube));
        //}
    }
    public bool CheckWinNumber()
    {
        if(checkWinNumber == cubesOfLevel.Count)
        {
            print("win");
            return true;    
        }
        return false;
    }
    public void CountCheckWin()
    {
        checkWinNumber++;
        print(checkWinNumber);
    }



}