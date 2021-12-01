using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New AnimationSO", menuName = "AnimationSO")]
public class AnimationSO : ScriptableObject
{
    #region variables

    public event Action<int> NewAnimationSelected = delegate { };
    public int AnimationIndex;

    #endregion

    #region ScriptableObject Methods

    private void OnEnable()
    {
        AnimationIndex = -1;
    }

    #endregion

    #region Helper Methods

    public void RaiseAnimationSelectedEvent(int selection)
    {
        NewAnimationSelected.Invoke(selection);
    }

    #endregion
}
