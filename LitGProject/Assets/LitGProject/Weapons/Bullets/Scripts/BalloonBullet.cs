using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBullet : BulletBase
{
    #region Variables

    public GameObject BalloonGO;
    public LayerMask InteractableMask;
    Balloon BalloonRef;


    #endregion

    #region Monobehaviour Methods

    void OnCollisionEnter(Collision collision)
    {
        //this comparison is made since layermask gives a bit value while layer gives an int
        if( ( (1 << collision.gameObject.layer) & InteractableMask) != 0 )
        {
            //get the bullet's point of contact with an object and instantiate/setup a balloon there
            Vector3 CollisionPoint = collision.GetContact(0).point;
            GameObject TempBalloon = Instantiate(BalloonGO, CollisionPoint, Quaternion.identity, this.transform.parent);
            TempBalloon.transform.localScale *= 0.4f;
            Rigidbody CollisionBody = collision.gameObject.GetComponent<Rigidbody>();
            BalloonRef = TempBalloon.GetComponent<Balloon>();
            BalloonRef.SetupBalloon ( CollisionPoint, CollisionBody, collision.transform.InverseTransformPoint(CollisionPoint), collision.transform );
        }
        this.gameObject.SetActive(false); //hide the bullet when finished and send it back to the pool
    }

    #endregion

    #region Helper Methods

    #endregion
}
