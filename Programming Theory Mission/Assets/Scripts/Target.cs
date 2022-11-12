using UnityEngine;

public class Target : MonoBehaviour
{
    private float speed = 100;

    void Update()
    {
        // Simple animation to make the target a little more visible.
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
