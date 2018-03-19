using UnityEngine;
using System.Collections;
using System;

public class Gun : MonoBehaviour {

	public float damage = 10f;
	public float range = 100f;
    public float fireRate = 15f;

    public static bool auto = true;
    public static int totalAmmo;
    public int ammoReserve = 180;
    public int maxAmmo = 30;
    public static int currentAmmo;
    public float tacReloadTime = 1.6f;
    public float reloadTime = 1.9f;
    private bool isReloading = false;
    public static long lastShot = 0;

    private static bool gunAds = ads.isAds;
    private long getUnixTime()

    {
        return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }


    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impact;

    private float fireLimit = 0f;

    void Start ()
    {
        currentAmmo = maxAmmo;
        totalAmmo = ammoReserve;
    }

    void Update()
    {
        if (gunAds == false)
        {
            Debug.Log("This shit's broke af false");
        }
        if (gunAds == true)
        {
            Debug.Log("This crap's broke af true");
        }

        if (isReloading == true)
            return;

        if (Input.GetButton("FireMode"))
        {
            if (getUnixTime() > lastShot)
            {
                if (auto)
                {
                    Fire1();
                    Debug.Log("Auto");
                }

                else
                {
                    Fire2();
                    Debug.Log("Sinlge");
                }
                lastShot = getUnixTime();
            }
        }

        if (currentAmmo >= 1f) 
        {
            if (auto)
            {
                if (Input.GetButtonDown("Fire1") && Time.time >= fireLimit)
                {
                    fireLimit = Time.time + 1f / fireRate;

                    Shoot();

                    FindObjectOfType<AudioManager>().Play("Shoot");

                    return;
                }
            }

            else
            {
                if (Input.GetButton("Fire1") && Time.time >= fireLimit)
                {
                    fireLimit = Time.time + 1f / fireRate;

                    Shoot();

                    FindObjectOfType<AudioManager>().Play("Shoot");

                    return;
                }
            }
        }

        if (Input.GetButton ("Reload"))
        {
            if (gunAds == true && totalAmmo !=0)
            {    
                if (currentAmmo == 0)
                {
                    StartCoroutine(Reload());
                    return;
                }
                else
                {
                    StartCoroutine(TacticalReload());
                }
            }    
        }     
	}


    IEnumerator Reload()
    {

        isReloading = true;

        if(totalAmmo >= 1)
        {
            if (totalAmmo >= maxAmmo)
            {
                if(currentAmmo == 0)
                {
                    Debug.Log("Reloading...");

                    yield return new WaitForSeconds(reloadTime);

                    currentAmmo = maxAmmo;

                    totalAmmo = totalAmmo - maxAmmo;

                    isReloading = false;
                }
            }

            if (isReloading == true)
            {
                if (totalAmmo < maxAmmo)
                {
                    Debug.Log("Partially Reloading...");

                    yield return new WaitForSeconds(reloadTime);

                    currentAmmo = currentAmmo + totalAmmo;

                    totalAmmo = 0;

                    isReloading = false;
                }
            }
        }
    }

    IEnumerator TacticalReload()
    {
        isReloading = true;

        if (totalAmmo >= 1)
        {

            if (currentAmmo + totalAmmo >= maxAmmo + 1)
            {
                Debug.Log("Tactically Reloading...");

                yield return new WaitForSeconds(tacReloadTime);

                totalAmmo = totalAmmo - ((maxAmmo-currentAmmo) + 1);

                currentAmmo = maxAmmo + 1;

                isReloading = false;
            }

            if (isReloading == true)
            {
                if (currentAmmo + totalAmmo < maxAmmo + 1)
                {
                    Debug.Log("Partially Tactically Reloading...");

                    yield return new WaitForSeconds(tacReloadTime);

                    currentAmmo = currentAmmo + totalAmmo;

                    if (currentAmmo > maxAmmo)
                    {
                        totalAmmo = currentAmmo - maxAmmo;

                        currentAmmo = currentAmmo - totalAmmo;

                        isReloading = false;

                    }

                    else
                    {
                        totalAmmo = 0;
                    }

                    isReloading = false;
                }
            }
        }
    }

    void Fire1()
    {
        auto = false;
    }

    void Fire2()
    {
        auto = true;
    }

    void Shoot()
	{
        muzzleFlash.Play();

        currentAmmo--;

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Hitbox hitbox = hit.transform.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                hitbox.TakeDamage(damage);
            }

            GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 5f);
        }
	}
}
