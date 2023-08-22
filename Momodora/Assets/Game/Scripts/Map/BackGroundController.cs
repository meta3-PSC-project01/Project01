using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    BackGroundAnchorPoint forestLeft;
    BackGroundAnchorPoint forestMiddle;
    BackGroundAnchorPoint forestRight;
    BackGroundAnchorPoint dustLeft;
    BackGroundAnchorPoint dustMiddle;
    BackGroundAnchorPoint dustRight;
    BackGroundAnchorPoint undergroundUp;
    BackGroundAnchorPoint undergroundMiddle;
    BackGroundAnchorPoint undergroundDown;

    Vector3 forestReset = Vector3.zero;
    Vector3 underReset = new Vector3(0, -7, 0);
    Vector3 dustReset = new Vector3(0,-4,0);

    Vector3 forestStart = Vector3.zero;
    Vector3 MixedStart = Vector3.zero;
    Vector3 underStart = new Vector3(0, 9.75f, 0);

    private void Awake()
    {
        forestLeft = transform.Find("ForestLeft").GetComponent<BackGroundAnchorPoint>();
        forestMiddle = transform.Find("ForestMiddle").GetComponent<BackGroundAnchorPoint>();
        forestRight = transform.Find("ForestRight").GetComponent<BackGroundAnchorPoint>();

        dustLeft = transform.Find("DustLeft").GetComponent<BackGroundAnchorPoint>();
        dustMiddle = transform.Find("DustMiddle").GetComponent<BackGroundAnchorPoint>();
        dustRight = transform.Find("DustRight").GetComponent<BackGroundAnchorPoint>();

        undergroundUp = transform.Find("UnderGroundUp").GetComponent<BackGroundAnchorPoint>();
        undergroundMiddle = transform.Find("UnderGroundMiddle").GetComponent<BackGroundAnchorPoint>();
        undergroundDown = transform.Find("UnderGroundDown").GetComponent<BackGroundAnchorPoint>();
    }

    public void SetAnchoredPosition(BackGroundType backGroundType)
    {
        if (backGroundType == BackGroundType.FOREST)
        {
            forestLeft.enabled = true;
            forestRight.enabled = true;
            forestMiddle.enabled = true;

            forestLeft.ResetPosition(forestReset);
            forestRight.ResetPosition(forestReset);
            forestMiddle.ResetPosition(forestReset);

            dustLeft.enabled = true;
            dustRight.enabled = true;
            dustMiddle.enabled = true;

            dustLeft.ResetPosition(dustReset); 
            dustRight.ResetPosition(dustReset);
            dustMiddle.ResetPosition(dustReset);

            undergroundUp.enabled = false;
            undergroundDown.enabled = false;
            undergroundMiddle.enabled = false;

            undergroundUp.ResetPosition(underReset);
            undergroundDown.ResetPosition(underReset);
            undergroundMiddle.ResetPosition(underReset);

            transform.position = forestStart;
        }
        else
        if (backGroundType == BackGroundType.UNDERGROUND)
        {
            forestLeft.enabled = false;
            forestRight.enabled = false;
            forestMiddle.enabled = false;

            forestLeft.ResetPosition(forestReset);
            forestRight.ResetPosition(forestReset);
            forestMiddle.ResetPosition(forestReset);

            dustLeft.enabled = false;
            dustRight.enabled = false;
            dustMiddle.enabled = false;

            dustLeft.ResetPosition(dustReset);
            dustRight.ResetPosition(dustReset);
            dustMiddle.ResetPosition(dustReset);

            undergroundUp.enabled = true;
            undergroundDown.enabled = true;
            undergroundMiddle.enabled = true;

            undergroundUp.ResetPosition(underReset);
            undergroundDown.ResetPosition(underReset);
            undergroundMiddle.ResetPosition(underReset);

            transform.position = underStart;
        }
        else
        if (backGroundType == BackGroundType.MIXED)
        {
            forestLeft.enabled = true;
            forestRight.enabled = true;
            forestMiddle.enabled = true;

            forestLeft.ResetPosition(forestReset);
            forestRight.ResetPosition(forestReset);
            forestMiddle.ResetPosition(forestReset);

            dustLeft.enabled = true;
            dustRight.enabled = true;
            dustMiddle.enabled = true;

            dustLeft.ResetPosition(dustReset);
            dustRight.ResetPosition(dustReset);
            dustMiddle.ResetPosition(dustReset);

            undergroundUp.enabled = true;
            undergroundDown.enabled = true;
            undergroundMiddle.enabled = true;

            undergroundUp.ResetPosition(underReset);
            undergroundDown.ResetPosition(underReset);
            undergroundMiddle.ResetPosition(underReset);

            transform.position = MixedStart;
        }
    }



    public void MoveBackgroundAll(Vector2 position)
    {
        forestLeft.MoveBackground(position);
        forestMiddle.MoveBackground(position);
        forestRight.MoveBackground(position);
        dustLeft.MoveBackground(position);
        dustMiddle.MoveBackground(position);
        dustRight.MoveBackground(position);
        undergroundUp.MoveBackground(position);
        undergroundMiddle.MoveBackground(position);
        undergroundDown.MoveBackground(position);
    }
}

public enum BackGroundType
{
    FOREST=0,
    UNDERGROUND,
    MIXED
}
