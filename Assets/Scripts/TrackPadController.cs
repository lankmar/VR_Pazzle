using HTC.UnityPlugin.Vive;
using UnityEngine;

public class TrackPadController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    private void Update()
    {
        float x = ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.PadX);
        float y = ViveInput.GetAxis(HandRole.RightHand, ControllerAxis.PadY);

        Vector3 movement = (transform.forward * y + transform.right * x).normalized;

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
