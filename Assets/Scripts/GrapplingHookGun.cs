using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RopeState
{
    Disabled,
    Fly,
    Active,
}


public class GrapplingHookGun : MonoBehaviour
{

    [SerializeField] private GrapplingHook hook;
    [SerializeField] private Transform spawnHook;
    [SerializeField] private KeyBinding keyBinding;
    [SerializeField] private Rigidbody playerRigetbody;
    [SerializeField] private RopeRenderer ropeRenderer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float ropeMaxLength;
    [SerializeField] private float hookSpeed;

    private SpringJoint springJoint;
    private RopeState currentRopeState = RopeState.Disabled;
    private float ropeLength;


    private void Update()
    {
        if (currentRopeState != RopeState.Disabled)
        {
            ropeRenderer.Draw(spawnHook.position, hook.transform.position, ropeLength);
        }

        if (currentRopeState == RopeState.Fly)
        {
            ropeLength = Vector3.Distance(spawnHook.position, hook.transform.position);

            if (ropeLength >= ropeMaxLength)
            {
                RemoveHook();
            }

            // Макс длина верёвки
            if (ropeLength > 5)
                ropeLength = 5;
        }


        if (Input.GetKeyDown(keyBinding.GrapplingHook))
        {
            Shot();
        }

        if (Input.GetKeyUp(keyBinding.GrapplingHook))
        {
            RemoveHook();
        }

        if (Input.GetKeyDown(keyBinding.Jump))
        {
            JumpFromRope();
        }
    }


    private void JumpFromRope()
    {
        if (currentRopeState == RopeState.Active && playerController.IsGraundet == false)
        {
            float up = playerRigetbody.velocity.y;
            if (up < 5)
            {
                up = 5;
            }
            else if(up > 8)
            {
                up = 8;
            }

            playerRigetbody.velocity = new Vector3(playerRigetbody.velocity.x * 2, up * 2, playerRigetbody.velocity.z * 2);
            playerController.isSpinning = true;
            RemoveHook();
        }
    }


    private void Shot()
    {
        RemoveHook();
        currentRopeState = RopeState.Fly;

        hook.gameObject.SetActive(true);
        hook.transform.position = spawnHook.position;
        hook.transform.rotation = spawnHook.rotation;
        hook.rigidbody.velocity = spawnHook.forward * hookSpeed;
    }

    public void CreateSpringJoint()
    {
        currentRopeState = RopeState.Active;
        if (springJoint == null)
        {
            springJoint = gameObject.AddComponent<SpringJoint>();
            springJoint.connectedBody = hook.rigidbody;
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.anchor = new Vector3(0, 0.5f, 0);
            springJoint.connectedAnchor = Vector3.zero;
            springJoint.spring = 2000;
            springJoint.damper = 10;
            springJoint.minDistance = 2;
            springJoint.maxDistance = ropeLength / 1.25f;
        }
    }

    public void RemoveHook()
    {
        currentRopeState = RopeState.Disabled;
        ropeLength = 0;
        ropeRenderer.Hide();
        hook.UnHook();
        hook.gameObject.SetActive(false);

        if (springJoint)
        {
            Destroy(springJoint);
        }
    }
}