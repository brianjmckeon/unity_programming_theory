using UnityEngine;

public class Apple : Food
{
    protected override int spoilageRate => 5;

    private float speed = 20;
    private float timer;

    protected override bool doesAnimate()
    {
        return true;
    }

    protected override void animate()
    {
        // Bounce the apple up and down.
        timer += Time.deltaTime;
        var delta = Mathf.Sin(timer * speed * Mathf.PI / 4) * 0.005f;
        transform.position += new Vector3(0, delta, 0);
    }
}
