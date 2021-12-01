using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    #region variables


    #endregion

    #region MonoBehaviour Methods

    void Awake()
    {
        
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

    public void ChangeScene(int NextScene)
    {
        SceneManager.LoadScene(NextScene);
    }

    #endregion

}
