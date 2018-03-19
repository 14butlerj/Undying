using UnityEngine;
using UnityEngine.UI;

public class TotalAmmoCounter : MonoBehaviour

{
    public Text ammo;

    void Update()
    {
        ammo.text = Gun.totalAmmo.ToString();
    }
}
