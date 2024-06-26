
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
public class ObjectCollectorWindow : EditorWindow
{
    private GameObject selectedModel;
    private List<Material> availableMaterials = new List<Material>();

    [MenuItem("Tools/Object Collector")]
    public static void ShowWindow()
    {
        GetWindow<ObjectCollectorWindow>("Object Collector");
    }

    void OnGUI()
    {
        GUILayout.Label("Select a model to collect cubes", EditorStyles.boldLabel);

        selectedModel = (GameObject)EditorGUILayout.ObjectField("Model", selectedModel, typeof(GameObject), true);

        GUILayout.Space(10);
        GUILayout.Label("Available Materials", EditorStyles.boldLabel);

        // Display list of materials
        for (int i = 0; i < availableMaterials.Count; i++)
        {
            availableMaterials[i] = (Material)EditorGUILayout.ObjectField($"Material {i + 1}", availableMaterials[i], typeof(Material), false);
        }

        if (GUILayout.Button("Add Material"))
        {
            availableMaterials.Add(null);
        }

        if (GUILayout.Button("Collect Cubes"))
        {
            CollectCubes();
        }
    }

    void CollectCubes()
    {
        if (selectedModel == null)
        {
            Debug.LogError("No model selected.");
            return;
        }

        List<Transform> objectTransforms = new List<Transform>();

        foreach (Transform child in selectedModel.GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<Renderer>() != null && child.name.ToLower().Contains("cube"))
            {
                objectTransforms.Add(child);
            }
        }

        ObjectCollection objectCollection = ScriptableObject.CreateInstance<ObjectCollection>();

        foreach (Transform objectTransform in objectTransforms)
        {
            Material materialObject = objectTransform.GetComponent<Renderer>().sharedMaterial;
            int materialID = availableMaterials.IndexOf(materialObject);

            ObjectData objectData = new ObjectData
            {
                position = objectTransform.localPosition,
                ID = materialID,
                //material = materialObject

            };
            objectCollection.objects.Add(objectData);
        }

        string path = "Assets/_Game/LevelData";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        int nextLevelNumber = GetNextLevelNumber(path);
        string levelFolderPath = $"{path}/Level_{nextLevelNumber}";
        if (!Directory.Exists(levelFolderPath))
        {
            Directory.CreateDirectory(levelFolderPath);
        }

        string objectCollectionPath = AssetDatabase.GenerateUniqueAssetPath($"{levelFolderPath}/{selectedModel.name}_level_{nextLevelNumber}.asset");

        AssetDatabase.CreateAsset(objectCollection, objectCollectionPath);
        AssetDatabase.SaveAssets();

        CreateLevelScriptableObject(objectCollection, levelFolderPath, selectedModel.name, nextLevelNumber);

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = objectCollection;

        Debug.Log($"Cubes collected and saved to {objectCollectionPath}");
    }

    void CreateLevelScriptableObject(ObjectCollection objectCollection, string levelFolderPath, string modelName, int levelNumber)
    {
        Level level = ScriptableObject.CreateInstance<Level>();
        level.levelData = objectCollection;
        level.materialsColorFill = new List<Material>(availableMaterials);
        // Initialize other lists if needed
        level.cubeIdForDarkGrey = new List<int>();
        level.cubeIdForLightGrey = new List<int>();
        level.cubeIdForBlack = new List<int>();
        level.cubeIdForWhite = new List<int>();

        //string levelPath = AssetDatabase.GenerateUniqueAssetPath($"{levelFolderPath}/{modelName}_level_{levelNumber}_SO.asset");
        string levelPath = AssetDatabase.GenerateUniqueAssetPath($"{levelFolderPath}/level_{levelNumber}.asset");

        AssetDatabase.CreateAsset(level, levelPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"Level ScriptableObject created and saved to {levelPath}");
    }

    int GetNextLevelNumber(string path)
    {
        string[] existingFiles = Directory.GetDirectories(path, "Level_*");
        int maxLevel = 0;

        foreach (string dir in existingFiles)
        {
            string dirName = Path.GetFileName(dir);
            string[] parts = dirName.Split('_');
            if (parts.Length > 1 && int.TryParse(parts[1], out int levelNumber))
            {
                if (levelNumber > maxLevel)
                {
                    maxLevel = levelNumber;
                }
            }
        }

        return maxLevel + 1;
    }
}
#endif

