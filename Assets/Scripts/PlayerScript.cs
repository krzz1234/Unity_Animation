using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private CharacterController mController;
    private Animator mAnimator;
    public Vector3 mVelocity;
    public float WalkSpeed   = 1.894965f;
    public float RunSpeed    = 3.603041f;
    public float SprintSpeed = 4.922f;

    private Transform mCameraPivot;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        mController = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();

        // Lock/Hide the Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set Velocity to 0
        mVelocity = Vector3.zero;

        // Get Camera Pivot
        mCameraPivot = transform.Find("CameraPivot");
    }

    private void Jump() {
        // Set Velocity Up
        mVelocity.y = 10.0f;
        mAnimator.SetTrigger("Jump");
    }

    // Update is called once per frame
    void Update() {
        // Player Speed
        float targetSpeed = 0.0f;
        if(Input.GetKey(KeyCode.W)) {
            if(Input.GetKey(KeyCode.LeftControl)) {
                // Player Walking
                targetSpeed = WalkSpeed;
            } else if(Input.GetKey(KeyCode.LeftShift)) {
                // Player Sprinting
                targetSpeed = SprintSpeed;
            } else {
                // Player Running
                targetSpeed = RunSpeed;
            }
        }

        // Look left/right
        mCameraPivot.Rotate(Vector3.up, Input.GetAxis("Mouse X"), Space.World);
        mCameraPivot.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"), Space.Self);

        // Update Velocity
        mVelocity.z = Mathf.MoveTowards(mVelocity.z, targetSpeed, 2.0f * Time.deltaTime);

        if(mController.isGrounded == false) {
            mVelocity.y -= 9.8f * Time.deltaTime;
        } else {
            mVelocity.y = -2f;
        }
        mAnimator.SetFloat("Speed", mVelocity.z);
        mAnimator.SetBool("isGrounded", mController.isGrounded);

        // User Pressed Space
        if(Input.GetKeyDown(KeyCode.Space)) {
            // Jump
            Jump();
        }

        // Move Character
        mController.Move(mVelocity * Time.deltaTime);
    }
}
