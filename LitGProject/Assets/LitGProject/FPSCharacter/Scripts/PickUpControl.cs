using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpControl : MonoBehaviour
{
    #region variables

    //references
    public WeaponBase WeaponClassRef;
    public Rigidbody WeaponRigidBody;
    public Collider WeaponCollider;
    public Transform PlayerTransform;
    public Transform PlayerCameraTransform;
    public Transform WeaponContainer;

    //pickup motion data
    public float PickUpRange;
    public float DropForwardForce;
    public float DropUpwardForce;

    //bools
    public bool bIsWeaponEquipped;
    public static bool bPlayerEquipmentFull;
    bool bIsDropping = false;

    #endregion

    #region MonoBehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        //setup weapon
        if(!bIsWeaponEquipped)
        {
            WeaponClassRef.enabled = false;
            WeaponRigidBody.isKinematic = false;
            WeaponCollider.isTrigger = false;
        }
        else
        {
            WeaponClassRef.enabled = true;
            WeaponRigidBody.isKinematic = true;
            WeaponCollider.isTrigger = true;
            bPlayerEquipmentFull = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check if player is in range, "E" key is pressed, the player hasn't picked up anything yet
        // and this weapon is not picked up already
        Vector3 DistanceToPlayer = PlayerTransform.position - this.transform.position;
        if(!bIsWeaponEquipped && (DistanceToPlayer.magnitude <= PickUpRange) && Input.GetKeyUp(KeyCode.E) && !bPlayerEquipmentFull)
        {
            PickUp();
        }
        //if weapon equipped, or "Q" is pressed, drop the weapon
        if(bIsWeaponEquipped && Input.GetKeyUp(KeyCode.Q))
        {
            bIsDropping = true;
        }
    }

    void FixedUpdate()
    {
        if(bIsDropping)
        {
            Drop();
            bIsDropping = false;
        }
    }

    #endregion

    #region Helper Methods

    void PickUp()
    {
        bIsWeaponEquipped = true;
        bPlayerEquipmentFull = true;

        //parent the weapon to the gun container and reset its transform
        this.transform.parent = WeaponContainer;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = Vector3.one;

        //deactivate collider and rigidbody
        WeaponRigidBody.isKinematic = true;
        WeaponCollider.isTrigger = true;

        WeaponClassRef.enabled = true;
    }

    void Drop()
    {
        bIsWeaponEquipped = false;
        bPlayerEquipmentFull = false;

        //null the parent of the weapon
        this.transform.parent = null;

        //activate collider and rigidbody
        WeaponRigidBody.isKinematic = false;
        WeaponCollider.isTrigger = false;

        //throwing force for the dropped weapon
        WeaponRigidBody.AddForce(PlayerCameraTransform.forward * DropForwardForce, ForceMode.Impulse);
        WeaponRigidBody.AddForce(PlayerCameraTransform.up * DropUpwardForce, ForceMode.Impulse);
        //random rotation for the throwing
        float RandomRotation = Random.Range(-1.0f, 1.0f);
        WeaponRigidBody.AddTorque(new Vector3(RandomRotation, RandomRotation, RandomRotation) * 10.0f, ForceMode.Impulse);

        WeaponClassRef.enabled = false;
    }

    #endregion
}
