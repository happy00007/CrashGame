using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope2DCreater : MonoBehaviour
{
    [Range(3, 60)] public int segmentsCount = 5;
    public Transform pointA;
    public Transform pointB;

    public HingeJoint2D hingePrefab;
    public Rigidbody2D target;

    [HideInInspector] public Transform[] segments;

    private void Start()
    {
        GenerateRope();
    }
    Vector2 GetSegmantPosition(int segmanetIndex)
    {
        Vector2 posA = pointA.position;
        Vector2 posB = pointB.position;

        float fraction = 1f / (float)segmentsCount;
        return Vector2.Lerp(posA, posB, fraction * segmanetIndex);
    }

    void GenerateRope()
    {
        segments = new Transform[segmentsCount];

        for (int i = 0; i < segmentsCount; i++)
        {
            var currJoint = Instantiate(hingePrefab, GetSegmantPosition(i), Quaternion.identity, transform);
            segments[i] = currJoint.transform;
            currJoint.gameObject.SetActive(true);
            if (i > 0)
            {
                currJoint.GetComponent<HingeJoint2D>().connectedBody = segments[i - 1].GetComponent<Rigidbody2D>();
            }
            if (i >= segmentsCount - 1)
            {
                //currJoint.GetComponent<HingeJoint2D>().connectedBody = target;
                target.transform.position = currJoint.transform.position;
                target.GetComponent<HingeJoint2D>().connectedBody = currJoint.GetComponent<Rigidbody2D>();

            }
        }

    }

    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null)
            return;
        Gizmos.color = Color.green;
        for (int i = 0; i < segmentsCount; i++)
        {
            Vector2 posAtIndex = GetSegmantPosition(i);
            Gizmos.DrawSphere(posAtIndex, 0.1f);
        }
    }
}
