using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicBullet : BulletBase
{
    #region Variables

    #endregion

    #region Monobehaviour Methods

    private void OnCollisionEnter(Collision collision)
    {
        //send it back to the pool
        this.gameObject.SetActive(false);
    }

    #endregion

    #region Helper Methods

    #endregion
}
