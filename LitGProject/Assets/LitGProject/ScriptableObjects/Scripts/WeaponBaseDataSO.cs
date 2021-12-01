using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponBaseDataSO", menuName = "WeaponBaseDataSO")]
public class WeaponBaseDataSO : ScriptableObject
{
    #region variables

    public GameObject BulletGO;
   
    //bullet force
    public float ShootForce;
    public float UpwardForce;

    //gun data
    public float TimeBetweenShooting;
    public float ReloadTime;
    public float TimeBetweenShots;
    public int MaxAmmo;
    public bool bIsAutomatic;
    int _AmmoLeft;
    public int AmmoLeft
    {
        get { return _AmmoLeft; }
        set
        {
            _AmmoLeft = value;
            AmmoLeftSetted(_AmmoLeft);
        }
    }
    int _AmmoShot;
    public int AmmoShot
    {
        get { return _AmmoShot; }
        set { _AmmoShot = value; }
    }

    public event Action<int> AmmoLeftSetted = delegate { };

    //gun states
    public bool bCanShootAgain = true;
    public bool bUseDefaultForces = true;
    public bool bCanShowTarget = false;

    bool _bIsShooting;
    public bool bIsShooting
    {
        get { return _bIsShooting; }
        set { _bIsShooting = value; }
    }
    bool _bIsReadyToShoot;
    public bool bIsReadyToShoot
    {
        get { return _bIsReadyToShoot; }
        set { _bIsReadyToShoot = value; }
    }
    bool _bIsReloading;
    public bool bIsReloading
    {
        get { return _bIsReloading; }
        set { _bIsReloading = value; }
    }

    //vfx / sfx
    public GameObject MuzzleFlash;

    #endregion

    #region ScriptableObject Methods

    private void OnEnable()
    {
        //make sure ammo is full
        _AmmoLeft = MaxAmmo;
        _bIsReadyToShoot = true;
        _bIsReloading = false;
    }

    #endregion

    #region Helper Methods

    #endregion
}
