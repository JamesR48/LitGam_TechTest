using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicGun : WeaponBase
{
    #region variables

    public float FiringAngle = 45.0f;

    #endregion

    #region Monobehavior Methods

    #endregion

    #region Helper Methods

    //pure virtual method override for adding custom projectile motion to the bullet
    protected override Vector3 CustomBehaviour()
    {
        //distance to target
        float TargetDistance = Vector3.Distance(ShootPoint.position, TargetPoint);
        //velocity needed to throw the object to the target at specified angle.
        float ProjectileVelocity = TargetDistance / (Mathf.Sin(2 * FiringAngle * Mathf.Deg2Rad) / (-Physics.gravity.y));
        // Extract the Z (Unity's forward) and Y (up) componenent of the velocity
        float Vz = Mathf.Sqrt(ProjectileVelocity) * Mathf.Cos(FiringAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(ProjectileVelocity) * Mathf.Sin(FiringAngle * Mathf.Deg2Rad);
        // create the velocity vector in local space, the base method will make the convertion to global space
        return new Vector3(0f, Vy, Vz);
    }

    #endregion
}
