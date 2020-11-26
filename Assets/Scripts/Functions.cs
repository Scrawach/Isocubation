using System.Collections;
using UnityEngine;

public static class Functions
{
    public delegate void SmoothChangeFunction<T>(T start, T end, float t);
    
    public static float Bezier(float start, float middle, float end, float t)
    {
        if (t <= 0) 
            return start;

        if (t >= 1)
            return end;

        return start * (1 - t) * (1 - t) + middle * 2 * t * (1 - t) + end * (t * t);
    }

    public static IEnumerator SmoothChange<T>(T start, T end, float speed, SmoothChangeFunction<T> func)
    {
        var init = start;
        var final = end;

        var step = speed * Time.fixedDeltaTime;
        var t = 0f;

        while (t <= 1.0f)
        {
            t += step;
            func(init, final, t);
            yield return new WaitForFixedUpdate();
        }

        yield return null;
    }
    
    public static Quaternion GetRotationAfterFlip(Quaternion start, Vector3 direction)
    {
        var extraRot = Quaternion.Euler(0, 90, 0);

        if (direction.magnitude > 0.5f)
        {
            var dirNorm = direction.normalized;
            extraRot = Quaternion.Euler(90 * dirNorm.z, 0, -90 * dirNorm.x);
        }

        var result = extraRot * start;
        return result;
    }
}