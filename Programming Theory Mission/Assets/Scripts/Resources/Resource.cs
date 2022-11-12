using System.Collections;
using UnityEngine;

public abstract class Resource : MonoBehaviour
{
    // Number of seconds until a resource spoils. 
    protected abstract int spoilageRate { get; }

    // Current amount of spoilage.
    private int currentSpoilage = 0;

    private Vector3 originalScale;


    private void Awake()
    {
        originalScale = gameObject.transform.localScale;

        // Set the resource's tag so we don't have to do this in the editor.
        gameObject.tag = "Resource";
        StartCoroutine(lifeCycle());
    }

    private void Update()
    {
        // If our resource performs animation, call its animate method().
        if (doesAnimate())
        {
            animate();
        }
    }

    // Subclasses should return true if they want they perform animation during Update().
    protected virtual bool doesAnimate()
    {
        return false;
    }

    // Subclasses should override this to perform animation during Update().
    protected virtual void animate() { }

    IEnumerator lifeCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (currentSpoilage == spoilageRate)
            {
                // Destroy resources that have completely spoiled
                Destroy(gameObject);
            }
            else
            {
                // Shrink the resource object as it spoils
                float percentage = (float)(spoilageRate - currentSpoilage) / (float)spoilageRate;
                var newScale = new Vector3(originalScale.x * percentage, originalScale.y, originalScale.z * percentage);
                gameObject.transform.localScale = newScale;

                currentSpoilage++;
            }
        }
    }
}
