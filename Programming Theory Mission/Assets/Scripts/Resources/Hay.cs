using UnityEngine;

public class Hay : NonFood
{
    protected override int spoilageRate => 60;

    private float speed = 10;

    protected override bool doesAnimate()
    {
        return true;
    }

    protected override void animate()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
