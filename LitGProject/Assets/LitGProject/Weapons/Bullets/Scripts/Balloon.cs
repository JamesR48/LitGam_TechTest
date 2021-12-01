using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    #region Variables
    
    public float BalloonLifeTime = 10.0f;
    [SerializeField]
    private Transform BalloonBottom; //the point of attatchment of the linerenderer in the balloon

    LineRenderer BalloonLineRenderer;
    Vector3 CollisionPoint; //point of origin of the balloon
    Transform ObjectToAttachTo; //the object that is currently going to be attached with a balloon
    Vector3 CurrentAttachPoint; //current specific point where the balloon is attached
    ConfigurableJoint BalloonJoint;
    bool bDrawRope = false;

    #endregion

    #region Monobehaviour Methods

    void Awake()
    {
        BalloonLineRenderer = GetComponent<LineRenderer>();
        BalloonJoint = GetComponent<ConfigurableJoint>();
        Invoke("DestroyBalloon", BalloonLifeTime);
    }

    void LateUpdate()
    {
        DrawRope();
    }

    #endregion

    #region Helper Methods

    //this data will come from the projectile whenever it enters on collision with interactable objects
    public void SetupBalloon(Vector3 OriginPoint, Rigidbody CollisionBody, Vector3 AnchorPoint, Transform ObjectTransform)
    {
        CollisionPoint = OriginPoint;
        ObjectToAttachTo = ObjectTransform;
        bDrawRope = true;
        //setting up the ConfigurableJoint data
        BalloonJoint.connectedBody = CollisionBody;
        BalloonJoint.autoConfigureConnectedAnchor = false;
        BalloonJoint.connectedAnchor = AnchorPoint;

    }

    //update the linerenderer until the balloon gets destroyed
    void DrawRope()
    {
        if (!bDrawRope) return;

        CurrentAttachPoint = Vector3.Lerp(ObjectToAttachTo.position, CollisionPoint, Time.deltaTime * 8f);

        BalloonLineRenderer.SetPosition(0, BalloonBottom.position);
        BalloonLineRenderer.SetPosition(1, CurrentAttachPoint);
    }

    void DestroyBalloon()
    {
        BalloonLineRenderer.positionCount = 0;
        bDrawRope = false;
        Destroy(this.gameObject);
    }

    //in case we want to create some method for killing a balloon outside of this class
    //(for example with an special type of bullet/spike in the scenery or something)
    public void PopBalloon()
    {
        //cancel the invoke via lifetime ending and directly kill the balloon
        CancelInvoke();
        DestroyBalloon();
    }

    #endregion
}
