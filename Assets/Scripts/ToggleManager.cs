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


    public void OnClick(BaseEventData data) {
        PointerEventData mouse = data as PointerEventData;
        if (mouse != null) {
            if (mouse.button == PointerEventData.InputButton.Left) {
                if(GameboardManager.currentGameState == GameboardManager.GameState.Playing) {
                    if (gameObject == resetButton) {
                        gameObject.GetComponentInParent<GameboardManager>().Reset();
                        return;
                    }
                    if (gameObject.GetComponent<BombComponent>().isBomb) {
                        buttonDownToggle.sprite = gameObject.GetComponentInParent<GameboardManager>().bombSprite;
                        GameboardManager.currentGameState = GameboardManager.GameState.GameOver; //gets hit, goes to game over but other logic still executes. normal toggle logic
                    }
                    gameObject.GetComponent<Toggle>().interactable = false;
                    //cascade neighbors
                    gameObject.GetComponentInParent<GameboardManager>().CascadeInteractive(gameObject);
                    gameObject.GetComponentInChildren<Text>().enabled = true;
                }
                else if (GameboardManager.currentGameState == GameboardManager.GameState.GameOver) {
                    if (gameObject == resetButton) {
                        gameObject.GetComponentInParent<GameboardManager>().Reset();
                        return;
                    }
                }

            }
            else if (mouse.button == PointerEventData.InputButton.Right) {
                if (GameboardManager.currentGameState == GameboardManager.GameState.Playing) {
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
}
