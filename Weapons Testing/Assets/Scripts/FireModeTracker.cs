using UnityEngine;
using UnityEngine.UI;

public class FireModeTracker : MonoBehaviour
{
    public Text fire;

    void Update()
    {
        if (Gun.auto)
            fire.text = "i";

        else
            fire.text = "iii";
    }
}