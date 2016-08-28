using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GenerateGameboard : MonoBehaviour {
    public GameObject parent = null;
    public GameObject buttonRef = null;

    public Sprite bombSprite;
    public Sprite flagSprite;

    public GameObject[][] buttons = null;

    public int numBombs = 2;
    public int numButtons = 8;

    protected Vector2 screenSize;

    public void Awake() {
        buttons = new GameObject[numButtons][];
        for (int i = 0;i < buttons.Length ; i++) {
            buttons[i] = new GameObject[numButtons];
        }
        GenerateBoard(new Vector2(Screen.width, Screen.height), buttons);//screenSize+buttons array
        GenerateBomb(0);//current bombs = argument

        for(int col = 0; col < buttons.Length; col++) {
            for(int row = 0; row < buttons[col].Length; row++) {
                int neighborsTouching = BombNeighbors(col, row).Count;
                if (neighborsTouching > 0) {
                    buttons[col][row].GetComponentInChildren<Text>().text = neighborsTouching.ToString();
                }
            }
        }
    }

    protected void GenerateBoard(Vector2 screenSize,GameObject[][] buttons) {
        for (int col = 0; col < buttons.Length; col++) {//36x36 button size
            int xOffset = col * 35;//visual offset
            for (int row = 0; row < buttons[col].Length; row++) {
                int yOffset = row * -35; // visual offset

                GameObject button = Instantiate(buttonRef, new Vector3(), Quaternion.identity) as GameObject; //spawn button from prefab
                button.name = "Button_" + col + "_" + row;//easier unity access
                buttons[col][row] = button;//add buttons to list for references
                button.transform.SetParent(parent.transform);//set transform to gameboard
                Vector2 spawnPos = new Vector2(-(parent.GetComponent<RectTransform>().rect.width / 2) - (parent.GetComponent<RectTransform>().rect.width / 4), (parent.GetComponent<RectTransform>().rect.height / 2) + (parent.GetComponent<RectTransform>().rect.width / 4));//spawn upper left corner at -75,75

                button.GetComponent<RectTransform>().anchoredPosition = new Vector3(spawnPos.x + xOffset, spawnPos.y + yOffset);//spawn at location w/ visual offset applied
                button.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);//set scale to 1 from 0 (parent scale = 0)

                button.GetComponentInChildren<Text>().text = "";

            }
        }
    }

    protected void GenerateBomb(int activeBombs) {
        Random r = new Random();
        for (int col = 0; col < buttons.Length; col++) {
            for(int row = 0; row < buttons[col].Length; row++) {
                if (!buttons[col][row].GetComponent<BombComponent>().isBomb) {//recursion check
                    if (activeBombs < numBombs) {
                        if (Random.Range(0, 8) == 1) {
                            buttons[col][row].GetComponent<BombComponent>().isBomb = true;
                            activeBombs++;
                            Debug.Log("Bomb located at col:" + col + " row: " + row);
                        }
                    }
                }
            }
        }
        //recursively call if not enough bombs spawned
        if (activeBombs < numBombs) {
            GenerateBomb(activeBombs);
        }
    }
   
    public List<GameObject> BombNeighbors(int col, int row) {
        List<GameObject> neighbors = new List<GameObject>();
        //above left
        if (row > 0 && col > 0) {
            if (buttons[col-1][row - 1].GetComponent<BombComponent>().isBomb) {
                neighbors.Add(buttons[col - 1][row - 1]);
            }
        }

        //above
        if (row > 0) {
            if (buttons[col][row-1].GetComponent<BombComponent>().isBomb) {
                neighbors.Add(buttons[col][row - 1]);
            }
        }

        //above right
        if (row > 0 && col < buttons.Length-1) {
            if (buttons[col+1][row - 1].GetComponent<BombComponent>().isBomb) {
                neighbors.Add(buttons[col + 1][row - 1]);
            }
        }

        //left
        if (col > 0) {
            if (buttons[col-1][row].GetComponent<BombComponent>().isBomb) {
                neighbors.Add(buttons[col - 1][row]);
            }
        }

        //right
        if (col < buttons.Length-1) {
            if (buttons[col+1][row].GetComponent<BombComponent>().isBomb) {
                neighbors.Add(buttons[col + 1][row]);
            }
        }

        //under left
        if (row < buttons[col].Length-1 && col > 0) {
            if (buttons[col-1][row + 1].GetComponent<BombComponent>().isBomb) {
                neighbors.Add(buttons[col - 1][row + 1]);
            }
        }

        //under
        if (row < buttons[col].Length-1) {
            if (buttons[col][row+1].GetComponent<BombComponent>().isBomb) {
                neighbors.Add(buttons[col][row + 1]);
            }
        }

        //under right
        if (row < buttons[col].Length - 1 && col < buttons.Length - 1) {
            if (buttons[col + 1][row + 1].GetComponent<BombComponent>().isBomb) {
                neighbors.Add(buttons[col + 1][row + 1]);
            }
        }

        return neighbors;
    }
    public void CascadeInteractive(GameObject buttonPressed) {
        int col = 0;
        int row = 0;
        bool found = false;
        for (; col < buttons.Length; col++) {//removing -1 leads to out of bounds error
            for(; row < buttons[col].Length; row++) {
                if (buttons[col][row] == buttonPressed) {
                    found = true;
                    break;
                }
            }
            if (found) {
                break;
            }
            else{
                row = 0;

            }
        }

        int[][] indices = new int[][] {
            new int[]{-1,-1 }, new int[]{0, -1}, new int[] {1, -1},
            new int[] {-1,0},  new int[] {0,0 }, new int[] {1,0 },
            new int[] {-1,1 },new int[] {0,1 },new int[] {1,1 }
        };

        for(int i = 0; i < indices.Length; i++) {
            int newRow = row + indices[i][1];
            int newCol = col + indices[i][0];

            if (newRow >= 0 && newCol >= 0 &&
                newCol < buttons.Length && newRow < buttons[newCol].Length && !buttons[newCol][newRow].GetComponent<BombComponent>().isBomb && buttons[newCol][newRow].GetComponent<Toggle>().interactable) {
                buttons[newCol][newRow].GetComponent<Toggle>().interactable = false;
                buttons[newCol][newRow].GetComponent<Toggle>().isOn = false;
                CascadeInteractive(buttons[newCol][newRow]);
            }
        }
    }
}