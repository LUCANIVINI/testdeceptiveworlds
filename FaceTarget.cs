using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    // Public variable to assign the target object
    public Transform target;

    void Update()
    {
        // Get the direction from this object to the target object
        Vector3 direction = target.position - transform.position;

        // Zero out the y component so we only rotate on the y-axis
        direction.y = 0;

        // If direction vector is not zero, calculate the rotation to face away
        if (direction != Vector3.zero)
        {
            // Get the rotation required to face the opposite direction
            Quaternion targetRotation = Quaternion.LookRotation(-direction);

            // Apply the rotation to this object
            transform.rotation = targetRotation;
        }
    }
}
