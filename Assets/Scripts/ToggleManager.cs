using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleManager : MonoBehaviour {
    public Image buttonUpToggle;
    public Image buttonDownToggle;
    public Sprite buttonUp;

    public void OnClick(BaseEventData data) {
        PointerEventData mouse = data as PointerEventData;
        if (mouse != null) {
            if (mouse.button == PointerEventData.InputButton.Left) {
                if (gameObject.GetComponent<BombComponent>().isBomb) {
                    buttonDownToggle.sprite = gameObject.GetComponentInParent<GenerateGameboard>().bombSprite;
                }
                gameObject.GetComponent<Toggle>().interactable = false;
            }
            else if (mouse.button == PointerEventData.InputButton.Right) {
                gameObject.GetComponent<FlagComponent>().OnRightClick();
                if (gameObject.GetComponent<FlagComponent>().isFlag) {
                    buttonUpToggle.sprite = gameObject.GetComponentInParent<GenerateGameboard>().flagSprite;
                }
                else {
                    //set sprite to normal up state
                    buttonUpToggle.sprite = buttonUp;
                }
            }
        }
        
    }
}
