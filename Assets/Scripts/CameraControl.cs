
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using ExtensionsMethods;

/// <summary>
/// This is the script we use to control the camera. It is
/// a modified version of the one provided by Unity standard
/// assets.
/// </summary>
public class CameraControl : MonoBehaviour
{
    public enum Type { NORMAL, CINIMA };

    public Type cameraType = Type.NORMAL;
    public float dampTime = 0.2f;                 // Approximate time for the camera to refocus.
    public float screenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
    public float minSize = 6.5f;                  // The smallest orthographic size the camera can be.
    [SerializeField] private List<Transform> targets = new List<Transform> { }; // All the targets the camera needs to encompass.
    public float cameraDistance = 15;
    [SerializeField] private Vector3 desiredRotation;           // The position the camera is moving towards.
    public bool moveEnabled = true;               // Whether or not the camera is moving based on it's targets
    [SerializeField] private Vector3 offsetPosition;           // The position the camera is moving towards.

    private new Camera camera;                        // Used for referencing the camera.
    private float zoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 moveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3 desiredPosition;              // The position the camera is moving towards.

    public DepthOfField depthOfField;

    public List<Transform> Targets
    {
        get
        {
            return targets;
        }

        set
        {
            targets = value;
        }
    }

    private void Start()
    {
        camera = GetComponentInChildren<Camera>();

        desiredRotation = transform.eulerAngles;

        foreach(PlayerController playerController in FindObjectsOfType<PlayerController>())
        {
            targets.Add(playerController.transform);
        }
    }

    private void FixedUpdate()
    {
        if (cameraType == Type.CINIMA)
        {
            desiredRotation = new Vector3(
                desiredRotation.x,
                desiredRotation.y + .3f,
                desiredRotation.z);
        }

        // Move the camera towards a desired position.
        if (moveEnabled)
        {
            Move();
            Rotate();

            if (desiredPosition != Vector3.zero)
            {
                Zoom();
            }
        }
    }

    private void Move()
    {
        // Find the average position of the targets.
        FindAveragePosition();

        if (desiredPosition != Vector3.zero)
        {
            // Smoothly transition to that position.
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
        }
    }

    void Rotate()
    {
        if (desiredRotation != transform.eulerAngles && !desiredRotation.Equals(transform.rotation))
        {
            transform.rotation = transform.rotation.RotateTowardsSnap(Quaternion.Euler(desiredRotation), 1);
        }
    }
    private void FindAveragePosition()
    {
        Vector3 averagePos = Vector3.zero;
        int numTargets = 0;

        // Go through all the targets and add their positions together.
        for (int i = 0; i < Targets.Count; i++)
        {
            Transform target = Targets[i];

            // If the target isn't active, go on to the next one.
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                continue;
            }

            // Add to the average and increment the number of targets in the average.
            averagePos += Targets[i].position;
            numTargets++;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
        {
            averagePos /= numTargets;

            // Keep the same y value.
            //averagePos.y = transform.position.y;

            averagePos += transform.rotation * (Vector3.back * cameraDistance);
        }

        // The desired position is the average position;
        desiredPosition = averagePos + offsetPosition;
    }
    
    private void Zoom()
    {
        // Find the required size based on the desired position and smoothly transition to that size.
        float requiredSize = FindRequiredSize();
        if (camera.orthographic)
        {
            camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
        }
        else
        {
            camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, requiredSize, ref zoomSpeed, dampTime) * 3;

            if (depthOfField != null)
            {
                depthOfField.focalLength = cameraDistance;
            }
        }
    }

    private float FindRequiredSize()
    {
        // Find the position the camera rig is moving towards in its local space.
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);

        // Start the camera's size calculation at zero.
        float size = 0f;

        // Go through all the targets...
        for (int i = 0; i < Targets.Count; i++)
        {
            // ... and if they aren't active continue on to the next target.
            if (!Targets[i].gameObject.activeSelf)
                continue;

            // Otherwise, find the position of the target in the camera's local space.
            Vector3 targetLocalPos = transform.InverseTransformPoint(Targets[i].position);

            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / camera.aspect);
        }

        // Add the edge buffer to the size.
        size += screenEdgeBuffer;

        // Make sure the camera's size isn't below the minimum.
        size = Mathf.Max(size, minSize);

        return size;
    }

    /*public void SetStartPositionAndSize()
    {
        // Find the desired position.
        FindAveragePosition();

        Debug.Log("desiredPosition: " + desiredPosition);

        // Set the camera's position to the desired position without damping.
        transform.position = desiredPosition;

        // Find and set the required size of the camera.
        camera.orthographicSize = FindRequiredSize();
    }*/

    public void SetPositionNoDamp(Vector3 position)
    {
       transform.position = position + (transform.rotation * (Vector3.back * cameraDistance)); // Sets camera position back from given position by camera distance
    }
}