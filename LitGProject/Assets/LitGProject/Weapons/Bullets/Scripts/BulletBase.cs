using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    #region Variables

    //references
    public Rigidbody BulletRigidBody;
    //public GameObject CollideVFX;

    //bullet data
    public bool bUseGravity;
    public float BulletLifeTime;

    #endregion

    #region MonoBehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //invoke a hidingbullet method based on lifetime in case that the player shoots the bullet outside of the scene or something
    protected void OnEnable()
    {
        BulletRigidBody.WakeUp();
        Invoke("HideBullet", BulletLifeTime);
    }

    //cancel the invoke if the bullet gets hidded from other circumstances
    protected void OnDisable()
    {
        BulletRigidBody.Sleep();
        CancelInvoke();
    }

    #endregion

    #region Helper Methods

    protected void HideBullet()
    {
        this.gameObject.SetActive(false);
    }

    #endregion
}
