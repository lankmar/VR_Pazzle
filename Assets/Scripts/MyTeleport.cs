using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyTeleport : Teleportable
{
    [SerializeField] private float Speed;
    [SerializeField] private float CoolDown;

    public override IEnumerator StartTeleport(RaycastResult hitResult, Vector3 position, Quaternion rotation, float delay)
    {
        while (true)
        {
            target.position = Vector3.MoveTowards(target.position, position, Speed * Time.deltaTime);

            Vector3 v = position;
            v.y = target.position.y;

            if (Vector3.Distance(target.position, v) < 0.1f)
            {
                yield return new WaitForSeconds(CoolDown);
                teleportCoroutine = null;
                yield break;
            }

            yield return new WaitForFixedUpdate();

        }
    }
} 
