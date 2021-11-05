using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EyesLookController : MonoBehaviour {

    [SerializeField]
    private Transform leftEye;

    [SerializeField]
    private Transform rightEye;

    [SerializeField]
    private GameObject target;

    private void Update()
    {
        leftEye.transform.SetPositionAndRotation(leftEye.transform.position, LookAt(leftEye.transform.position, target.transform.position));
        rightEye.transform.SetPositionAndRotation(rightEye.transform.position, LookAt(rightEye.transform.position, target.transform.position));
        leftEye.transform.Rotate(new Vector3(90, 0, 0));
        rightEye.transform.Rotate(new Vector3(90, 0, 0));
    }

    public static Quaternion LookAt(Vector3 sourcePoint, Vector3 destPoint)
    {
        Vector3 forwardVector = Vector3.Normalize(destPoint - sourcePoint);

        float dot = Vector3.Dot(Vector3.forward, forwardVector);

        if (Math.Abs(dot - (-1.0f)) < 0.000001f)
        {
            return new Quaternion(Vector3.up.x, Vector3.up.y, Vector3.up.z, 3.1415926535897932f);
        }
        if (Math.Abs(dot - (1.0f)) < 0.000001f)
        {
            return Quaternion.identity;
        }

        float rotAngle = (float)Math.Acos(dot);
        Vector3 rotAxis = Vector3.Cross(Vector3.forward, forwardVector);
        rotAxis = Vector3.Normalize(rotAxis);
        return CreateFromAxisAngle(rotAxis, rotAngle);
    }

    public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
    {
        float halfAngle = angle * .5f;
        float s = (float)System.Math.Sin(halfAngle);
        Quaternion q;
        q.x = axis.x * s;
        q.y = axis.y * s;
        q.z = axis.z * s;
        q.w = (float)System.Math.Cos(halfAngle);
        return q;
    }
}
