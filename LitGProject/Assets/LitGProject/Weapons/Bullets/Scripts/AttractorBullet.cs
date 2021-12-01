using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorBullet : BulletBase
{
    #region variables

    public float PullRadius = 8;
    public float MaxSeparation = 2; //how far can the objects stay from the projectile once they get pulled by its force
    public float PullForce = 100;
    public int StickingLayerIndex; //index of the physics layer where this bullet can "stick" itself to
    public LayerMask InteractableMask; //layermask of objects that can react to the attraction force

    #endregion

    #region Monobehavior Methods

    void FixedUpdate()
    {
        foreach ( Collider collider in Physics.OverlapSphere(transform.position, PullRadius, InteractableMask) )
        {

            // apply force on target towards the bullet
            Rigidbody ColliderRB = collider.GetComponent<Rigidbody>();
            if (ColliderRB == null)
            {
                continue;
            }
            // calculate direction from target to bullet
            Vector3 ForceDirection = transform.position - ColliderRB.position;
            // calculate force proportional to distance from target to center
            float Distance = ForceDirection.magnitude;
            ForceDirection = ForceDirection.normalized;
            if (Distance < MaxSeparation)
            {
                continue;
            }
            // calculate the force proportional to the distance from target to bullet
            float ForceRate = PullForce / Distance;
            // calculate the resultant vector force and add it to the objects
            ColliderRB.AddForce( ForceDirection * (ForceRate/ColliderRB.mass) );
        }
    }

    //Upon collision with a wall, the bullet will stick to the contact point until it gets deactivated
    void OnTriggerEnter(Collider Other)
    {
        if(Other.gameObject.layer == StickingLayerIndex)
        {
            transform.position = Other.ClosestPoint(transform.position);
            this.BulletRigidBody.Sleep();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, PullRadius);
    }

    #endregion

    #region Helper Methods


    #endregion
}
