using UnityEngine;

// Controller for the player character.
public class FarmerController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    // The prefab to use for our target reticle.
    [SerializeField]
    private GameObject targetPrefab;

    [SerializeField]
    private float panSpeed = 20.0f;

    [SerializeField]
    private float speed = 10;

    private GameObject target;

    // This defines the distance radius around our player.
    private float range = 0.5f;

    private Animator animator;

    // A plane to use for translating mouse coords to world coords.
    private Plane groundPlane = new Plane(Vector3.up, 0);


    void Start()
    {
        target = Instantiate(targetPrefab, gameObject.transform.position + new Vector3(0, 0.1f, 0), targetPrefab.transform.rotation);
        animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        // Move Camera
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mainCamera.transform.position =
            mainCamera.transform.position + new Vector3(move.x, 0, move.y) * panSpeed * Time.deltaTime;

        // When the player clicks the left mouse button, we set the target the farmer will move towards.
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.nearClipPlane;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

            float distance;
            Ray ray = mainCamera.ScreenPointToRay(mousePos);

            if (groundPlane.Raycast(ray, out distance))
            {
                worldPos = ray.GetPoint(distance);

                // Ignore targets that are already within our range.
                if (Vector3.Distance(worldPos, transform.position) > range)
                {
                    // Make sure target is always shown above the ground.
                    worldPos.y = 0.1f;
                    target.transform.position = worldPos;
                    gameObject.transform.LookAt(worldPos);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // If we're not standing near the target, move the farmer toward it.
        float distanceFromTarget = Vector3.Distance(target.transform.position, gameObject.transform.position);

        if (distanceFromTarget > range)
        {
            // We're running toward the target.
            animator.SetFloat("Speed_f", 1);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            // We're stopped.
            animator.SetFloat("Speed_f", 0);
        }
    }


    // Detect collisions only with resources.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == null || collision.gameObject.tag != "Resource")
        {
            return;
        }

        Destroy(collision.gameObject);
    }
}
