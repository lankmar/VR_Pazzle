using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDemo : MonoBehaviour
{
    void Update()
    {
        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Trigger))
        {
            Debug.Log("GetPressDownEx Trigger");
        }
        if (ViveInput.GetPressDownEx(HandRole.RightHand, ControllerButton.Grip))
        {
            Debug.Log("GetPressDownEx Grip");
        }

        if (ViveInput.GetAxisEx(HandRole.LeftHand, ControllerAxis.Trigger) > 0.5f)
        {
            Debug.Log("GetAxisEx Trigger" + ViveInput.GetAxisEx(HandRole.LeftHand, ControllerAxis.Trigger));
        }

        float dpX = ViveInput.GetPadTouchDelta(HandRole.RightHand).x;
        float dpY = ViveInput.GetPadTouchDelta(HandRole.RightHand).y;

        Debug.Log($"dpX: {dpX}; dpY: {dpY}");
    }
}
