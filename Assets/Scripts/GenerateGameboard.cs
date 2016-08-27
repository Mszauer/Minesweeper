using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GenerateGameboard : MonoBehaviour {
    public GameObject parent = null;
    public GameObject buttonRef = null;

    public Sprite bombSprite;
    public Sprite flagSprite;

    public List<GameObject> buttons = null;

    public int numBombs = 1;

    protected Vector2 screenSize;

    public void Awake() {
        buttons = new List<GameObject>();

        screenSize = new Vector2(Screen.width, Screen.height);
        int activeBombs = 0;
        Vector2 numButtons = new Vector2(8, 8);
        for (int col = 0; col < numButtons.x; col++) {//36x36 button size
            int xOffset = col * 35;
            for (int row = 0; row < numButtons.y; row++) {
                int yOffset = row * -35;
                GameObject button = Instantiate(buttonRef, new Vector3(), Quaternion.identity) as GameObject;
                button.name = "Button_" + col + "_" + row;
                buttons.Add(button);
                button.transform.SetParent(parent.transform);
                Vector2 spawnPos = new Vector2(-parent.GetComponent<RectTransform>().rect.width / 2, parent.GetComponent<RectTransform>().rect.height / 2);


                button.GetComponent<RectTransform>().anchoredPosition = new Vector3(spawnPos.x + xOffset, spawnPos.y + yOffset);
                button.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                //button.GetComponent<Text>().text = " ";

                Random r = new Random();
                
                while (activeBombs < numBombs) {
                    if (Random.Range(0,8) == 1) {
                        button.GetComponent<BombComponent>().isBomb = true;
                        activeBombs++;
                    }
                    Debug.Log("Bomb located at col:" + col+" row: "+row);//8columns. Row*numCol+col
                }



            }
        }
        
    }
   
}
