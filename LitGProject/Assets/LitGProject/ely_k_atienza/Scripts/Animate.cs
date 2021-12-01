using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    #region variables

    public AnimationSO SelectedAnimSO;

    Animator CharacterAnimator;

    #endregion

    #region Monobehaviour Methods

    void Awake()
    {
        CharacterAnimator = GetComponentInChildren<Animator>();    
    }

    //subscribe to the event for easily stablish animations
    private void OnEnable()
    {
        SelectedAnimSO.NewAnimationSelected += OnNewAnimationSelected;
    }

    //unsubscribe from the event when needed
    private void OnDisable()
    {
        SelectedAnimSO.NewAnimationSelected -= OnNewAnimationSelected;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SelectedAnimSO.AnimationIndex != -1) //default state is -1, TPose
        {
            PlayAnimation(SelectedAnimSO.AnimationIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Helper Methods

    void OnNewAnimationSelected(int NewAnimationIndex)
    {
        PlayAnimation(NewAnimationIndex);
    }

    public void PlayAnimation(int StateIndex)
    {
        switch (StateIndex)
        {
            case 0:
                CharacterAnimator.Play(AnimationTags.HOUSEDANCE);
                break;
            case 1:
                CharacterAnimator.Play(AnimationTags.MACARENADANCE);
                break;
            case 2:
                CharacterAnimator.Play(AnimationTags.HIPHOPDANCE);
                break;
        }
    }

    #endregion
}
