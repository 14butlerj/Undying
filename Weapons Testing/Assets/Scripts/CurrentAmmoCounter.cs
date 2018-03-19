using UnityEngine;
using UnityEngine.UI;

public class CurrentAmmoCounter : MonoBehaviour

{
    public Text ammo;

    void Update()
    {
        ammo.text = Gun.currentAmmo.ToString();
    }
}