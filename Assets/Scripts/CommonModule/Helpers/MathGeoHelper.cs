using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MathGeoHelper
{
    //sin Y
    //cos X
    public static float AngleBetweenPoints(Vector3 center, Vector3 lookPoint)
    {
        var radians = Math.Atan2(lookPoint.y - center.y, lookPoint.x - center.x);
        var angle = (float)(radians * (180 / Math.PI));
        angle += 270;
        if (angle > 360) angle -= 360;
        return angle;
    }

    public static Vector3 AngledOffset(float angle, float distance)
    {
        var radians = (angle + 90) * (Math.PI / 180);
        return new Vector3((float)(Math.Cos(radians) * distance), (float)(Math.Sin(radians) * distance));
    }

    public static Vector3 AngledOffset(Quaternion quaternion, float distance)
    {
        return AngledOffset(quaternion.eulerAngles.z, distance);
    }

    public static Quaternion GetByAngle(float angle)
    {
        return new Quaternion(0, 0, angle, 0);
    }
}

