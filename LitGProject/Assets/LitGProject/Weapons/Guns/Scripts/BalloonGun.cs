using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonGun : WeaponBase
{
    #region variables

    #endregion

    #region Monobehavior Methods

    #endregion

    #region Helper Methods

    //in case that we don't need something special, we can just return a 0 vector for the pure virtual method override
    protected override Vector3 CustomBehaviour()
    {
        return Vector3.zero;
    }

    #endregion
}
