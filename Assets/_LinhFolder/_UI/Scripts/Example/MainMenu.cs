using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UIExample

{
    public class MainMenu : UICanvas
    {
        public GameObject gamePlayUI;
        public List<ButtonBase> buttons;
        public GameObject ListButton;
        public ButtonBase buttonPrefab;
        public void PlayButton()
        {
            UIManager.Ins.OpenUI<GamePlay>();
            CloseDirectly();
        }
        public void GenLevel(int level)
        {
           LevelManager.Ins.OnLoadLevel(level);
            this.gameObject.SetActive(false);
            gamePlayUI.SetActive(true);
            //GenButton();

        }
        //public void GenButton()
        //{
        //    //LevelManager.Ins.currentLevel = LevelManager.Ins.levels[level];
        //    for(int i = 0; i < LevelManager.Ins.currentLevel.materialsColorFill.Count; i++)
        //    {
        //        ButtonBase buttonSpawn = Instantiate(buttonPrefab);
        //        buttonSpawn.transform.SetParent(ListButton.transform, false);
        //        buttonSpawn.textIDNumerButton.text = (i+1).ToString();
        //        ////buttonSpawn.image.material = LevelManager.Ins.currentLevel.materialsColorFill[i];
        //        //buttonSpawn.gameObject.transform.SetParent(ListButton.transform);


        //    }
        //}
    }
}