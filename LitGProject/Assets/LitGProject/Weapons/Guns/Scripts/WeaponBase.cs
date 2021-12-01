using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponBase : MonoBehaviour
{
    #region Variables

    public WeaponBaseDataSO WeaponDataSO;
    
    //references
    public Camera PlayerCamera;
    public Transform ShootPoint;
    public Text AmmoDisplay;
    public Text WeaponTypeDisplay;

    //bullet pool
    public Transform BulletPoolParent;
    protected GameObject[] BulletPool;
    protected Rigidbody[] BulletRigidbodies;

    //weapon target
    public GameObject WeaponTargetGO;
    protected Vector3 TargetPoint;

    //raycast variables
    protected Ray ray;
    protected RaycastHit RayHit;

    #endregion

    #region Monobehaviour Methods

    //subscribe to events from the scriptableobject data for updating the HUD display text
    protected void OnEnable()
    {
        UpdateWeaponNameDisplay(this.name);
        WeaponDataSO.AmmoLeftSetted += UpdateAmmoDisplay;
        UpdateAmmoDisplay(WeaponDataSO.AmmoLeft);
    }

    //unsubscribe the events and setup the HUD display with default values
    protected void OnDisable()
    {
        UpdateWeaponNameDisplay("BareHands");
        WeaponDataSO.AmmoLeftSetted -= UpdateAmmoDisplay;
        ResetAmmoDisplay();
        if (WeaponTargetGO)
        {
            WeaponTargetGO.SetActive(false);
        }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        UpdateWeaponNameDisplay(this.name);
        CreateBulletsPool();
        UpdateAmmoDisplay(WeaponDataSO.AmmoLeft);
    }

    // Update is called once per frame
    protected void Update()
    {
        GetPlayerInput();
    }

    protected void FixedUpdate()
    {
        //shooting
        if (WeaponDataSO.bIsReadyToShoot && WeaponDataSO.bIsShooting && !WeaponDataSO.bIsReloading && WeaponDataSO.AmmoLeft > 0)
        {
            WeaponDataSO.AmmoShot = 0;
            Shoot();
        }
    }

    #endregion

    #region Helper Methods

    protected void CreateBulletsPool()
    {
        //instantiate and save the bullets you will use to avoid being creating and destroying constantly
        BulletPool = new GameObject[WeaponDataSO.MaxAmmo];
        BulletRigidbodies = new Rigidbody[WeaponDataSO.MaxAmmo];
        for (int index = 0; index < BulletPool.Length; index++)
        {
            GameObject TempBullet = Instantiate(WeaponDataSO.BulletGO);
            TempBullet.transform.SetParent(BulletPoolParent);
            TempBullet.SetActive(false);
            BulletPool[index] = TempBullet;
            BulletRigidbodies[index] = TempBullet.GetComponent<Rigidbody>();
        }
    }

    protected virtual void GetPlayerInput()
    {
        if (WeaponDataSO.bIsAutomatic)
        {
            WeaponDataSO.bIsShooting = Input.GetButton("Fire1"); //hold down left click
        }
        else
        {
            WeaponDataSO.bIsShooting = Input.GetButtonUp("Fire1"); //single press left click
        }

        //reloading
        if(Input.GetKeyDown(KeyCode.R) && WeaponDataSO.AmmoLeft < WeaponDataSO.MaxAmmo && !WeaponDataSO.bIsReloading)
        {
            Reload();
        }
        //automatic reload when trying to shoot without ammo
        if(WeaponDataSO.bIsReadyToShoot && !WeaponDataSO.bIsReloading && WeaponDataSO.AmmoLeft <= 0.0f)
        {
            Reload();
        }
    }

    void Shoot()
    {
        WeaponDataSO.bIsReadyToShoot = false;

        //check if ray hits something
        if( CalculateCameraRay() )
        {
            //if something was hitted, set target point to the hit
            TargetPoint = RayHit.point;

            //if desired, a target quad will appear in the projectile's point of collision
            //mostly useful to see where the parabolic shot will land
            if (WeaponDataSO.bCanShowTarget)
            {
                if(!WeaponTargetGO.activeInHierarchy)
                {
                    WeaponTargetGO.SetActive(true);
                }
                WeaponTargetGO.transform.forward = -RayHit.normal;
                WeaponTargetGO.transform.position = TargetPoint - (WeaponTargetGO.transform.forward * 0.1f);
            }
        }
        else
        {
            //if shooting to the air, set a point far from the player
            TargetPoint = ray.GetPoint(75.0f);
        }

        //calculate direction from shooting-point to target-point
        Vector3 ShootDirection = TargetPoint - ShootPoint.position;

        //pure virtual method to override for adding specific shooting behaviour in child classes
        Vector3 LocalVelocity = CustomBehaviour();

        for (int index = 0; index < BulletPool.Length; index++)
        {
            if(!BulletPool[index].activeInHierarchy)
            {
                BulletPool[index].transform.position = ShootPoint.position;
                BulletPool[index].transform.rotation = Quaternion.identity;
                BulletPool[index].transform.forward = ShootDirection.normalized;
                BulletPool[index].SetActive(true);
                //add simple forward and upward RigidBody forces to the bullet
                if(WeaponDataSO.bUseDefaultForces)
                {
                    BulletRigidbodies[index].AddForce(BulletPool[index].transform.forward * WeaponDataSO.ShootForce, ForceMode.Impulse);
                    BulletRigidbodies[index].AddForce(PlayerCamera.transform.up * WeaponDataSO.UpwardForce, ForceMode.Impulse);
                }
                else
                {
                    //add velocity vector to bullet in global space based on CustomBehaviour() method override
                    Vector3 GlobalVelocity = BulletPool[index].transform.TransformDirection(LocalVelocity);
                    BulletRigidbodies[index].velocity = GlobalVelocity;
                }

                break;
            }
        }

        //Instantiate MuzzleFlash or other kind of VFX/SFX if desired
        if(WeaponDataSO.MuzzleFlash != null)
        {
            Instantiate(WeaponDataSO.MuzzleFlash, ShootPoint.position, Quaternion.identity);
        }

        WeaponDataSO.AmmoLeft -= 1;
        WeaponDataSO.AmmoShot += 1;

        //wait a bit before shooting again
        if(WeaponDataSO.bCanShootAgain)
        {
            //Call ResetShooting() function after TimeBetweenShooting passed
            Invoke("ResetShooting", WeaponDataSO.TimeBetweenShooting);
            WeaponDataSO.bCanShootAgain = false; //only invoke once
        }
    }

    void ResetShooting()
    {
        WeaponDataSO.bIsReadyToShoot = true;
        WeaponDataSO.bCanShootAgain = true;
    }

    void Reload()
    {
        WeaponDataSO.bIsReloading = true;
        Invoke("FinishedReloading", WeaponDataSO.ReloadTime);
    }

    void FinishedReloading()
    {
        WeaponDataSO.AmmoLeft = WeaponDataSO.MaxAmmo;
        WeaponDataSO.bIsReloading = false;
    }

    protected bool CalculateCameraRay()
    {
        //find the exact shoot position using a raycast to the middle of the screen
        ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        return Physics.Raycast(ray, out RayHit);
    }

    protected void UpdateAmmoDisplay(int NewAmmoLeft)
    {
        if (AmmoDisplay)
        {
            AmmoDisplay.text = "Ammo: " + NewAmmoLeft + " / " + WeaponDataSO.MaxAmmo;
        }
    }

    protected void ResetAmmoDisplay()
    {
        if (AmmoDisplay)
        {
            AmmoDisplay.text = "Ammo: 0 / 0";
        }
    }

    protected void UpdateWeaponNameDisplay(string NewName)
    {
        if (WeaponTypeDisplay)
        {
            WeaponTypeDisplay.text = NewName;
        }
    }

    //pure virtual method to override for adding specific shooting behaviour in child classes
    protected abstract Vector3 CustomBehaviour();

    #endregion
}
