using UnityEngine;
using System.Collections;

public class FlagComponent : MonoBehaviour {
    public bool isFlag = false;
    public void OnRightClick() {
        isFlag = !isFlag;
    }
}
