using UnityEngine;

public static class Rigidbody2DHelper
{
    public static void MoveRight(Rigidbody2D body, float speed)
    {
        body.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);
    }

    public static void MoveLeft(Rigidbody2D body, float speed)
    {
        body.AddForce(new Vector2(speed * -1, 0), ForceMode2D.Impulse);
    }

    public static void MoveTop(Rigidbody2D body, float speed)
    {
        body.AddForce(new Vector2(0, speed), ForceMode2D.Impulse);
    }

    public static void MoveBottom(Rigidbody2D body, float speed)
    {
        body.AddForce(new Vector2(0, speed * -1), ForceMode2D.Impulse);
    }

    public static void LimitSpeed(Rigidbody2D body, float maxHorizontal, float maxVertical)
    {
        body.velocity = new Vector2(
            MathHelper.Bound(body.velocity.x, maxHorizontal * -1, maxHorizontal),
            MathHelper.Bound(body.velocity.y, maxVertical * -1, maxVertical));
    }
}
