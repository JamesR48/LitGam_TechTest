using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AnimationSelector : MonoBehaviour
{

    #region variables

    public AnimationSO SelectedAnimSO;

    public Button[] AnimationsButtons;
    public Button ContinueButton;
    Button CurrentSelectedButton;
    int CurrentSelectedButtonIndex;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        CurrentSelectedButton = null;
        CurrentSelectedButtonIndex = -1;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Helper Methods

    //dynamically change anaimations and enable/disable buttons based on click events
    public void SelectAnimation(int selection)
    {
        //get the selected animation index and raise an event to notify the character
        SelectedAnimSO.AnimationIndex = selection;
        SelectedAnimSO.RaiseAnimationSelectedEvent(selection);

        //hold the current selected button and animation index to activate/deactivate them
        if (CurrentSelectedButton != null && CurrentSelectedButtonIndex != -1)
        {
            CurrentSelectedButton.interactable = true;
        }
        CurrentSelectedButtonIndex = selection;
        CurrentSelectedButton = AnimationsButtons[CurrentSelectedButtonIndex];
        CurrentSelectedButton.interactable = false;

        //the continue button (change scene and establish animation button) will be active whenever an animation is selected
        if (!ContinueButton.interactable)
        {
            ContinueButton.interactable = true;
        }
    }

    #endregion

}
