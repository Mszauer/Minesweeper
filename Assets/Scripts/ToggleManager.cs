using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleManager : MonoBehaviour {
    public Image buttonUpToggle;
    public Image buttonDownToggle;
    public Sprite buttonUp;
    public GameObject resetButton;

    protected GameboardManager gameboardManager;

    void Awake() {
        gameboardManager = FindObjectOfType<GameboardManager>();
    }

    public void OnClick(BaseEventData data) {
        PointerEventData mouse = data as PointerEventData;
        if (mouse != null) {
            if (mouse.button == PointerEventData.InputButton.Left) {
                if (gameObject == resetButton) {
                    ResetClicked();
                }

                else if (gameboardManager.currentGameState == GameboardManager.GameState.Interactable) {

                    if (gameObject.GetComponent<BombComponent>().isBomb) {
                        buttonDownToggle.sprite = gameObject.GetComponentInParent<GameboardManager>().bombSprite;
                        gameObject.GetComponentInParent<GameboardManager>().BombClicked();
                        gameboardManager.Victory(false);
                        return;
                    }
                    gameObject.GetComponent<Toggle>().interactable = false;
                    //cascade neighbors
                    gameObject.GetComponentInParent<GameboardManager>().CascadeInteractive(gameObject);
                    gameObject.GetComponentInChildren<Text>().enabled = true;
                    if (gameboardManager.ActiveCellsLeft() == 0) {
                        gameboardManager.Victory(true);
                    }
                }

            }
            else if (mouse.button == PointerEventData.InputButton.Right) {
                if (gameboardManager.currentGameState == GameboardManager.GameState.Interactable) {
                    gameObject.GetComponent<FlagComponent>().OnRightClick();
                    if (gameObject.GetComponent<FlagComponent>().isFlag) {
                        buttonUpToggle.sprite = gameObject.GetComponentInParent<GameboardManager>().flagSprite;
                    }
                    else {
                        //set sprite to normal up state
                        buttonUpToggle.sprite = buttonUp;
                    }
                }
            }
        }
    }

    protected void ResetClicked() {
        Debug.Log("reset button clicked");
        gameObject.GetComponentInParent<GameboardManager>().Reset();
        return;
    }
    public void FlagBombs() {
        GameObject[] allBombs = gameboardManager.GetAllBombs();
        for (int i = 0; i < allBombs.Length; i++) {
            allBombs[i].GetComponent<FlagComponent>().isFlag = true;
            allBombs[i].GetComponent<ToggleManager>().buttonUpToggle.sprite = gameObject.GetComponentInParent<GameboardManager>().flagSprite;
        }
    }
}
