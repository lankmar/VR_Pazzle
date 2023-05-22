using HTC.UnityPlugin.Vive;
using UnityEngine;
using Valve.VR;

public class TrackPadScroller : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _deadZone = 0.1f;

    private SteamVR_RenderModel _vive;
    private CharMagnetic _magnite;

    private void Start()
    {
        _magnite = GetComponent<CharMagnetic>();
    }

    private void Update()
    {
        // if (_vive == null) _vive = GetComponentInChildren<SteamVR_RenderModel>();
        if (_vive == null) return;

        float dp = ViveInput.GetPadTouchDelta(HandRole.RightHand).y;
        if (Mathf.Abs(dp) > _deadZone)
        {
            _magnite.ChangeSpringPower(dp * _speed);
            _vive.controllerModeState.bScrollWheelVisible = true;
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.PadTouch))
            _vive.controllerModeState.bScrollWheelVisible = false;

    }
}