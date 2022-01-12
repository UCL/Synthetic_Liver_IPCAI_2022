using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// from https://www.youtube.com/watch?v=TkVR2Mf_U8Y
public class FollowPath : MonoBehaviour
{
    public float speed = 0.000005f;
    public float moveToNextTol = 0.00001f;
    public Transform pathParent;
    public bool resetCounter = false;
    public int resetAfterNFrames = 275;
    Transform targetPoint;
    int index;

    void OnDrawGizmos()
    {
        Vector3 from;
        Vector3 to;

        for (int a=0; a<pathParent.childCount; a++)
        {
            from = pathParent.GetChild(a).position;
            to = pathParent.GetChild((a+1) % pathParent.childCount).position;
            Gizmos.color = new Color (1, 0, 0);
            Gizmos.DrawLine (from, to);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        targetPoint = pathParent.GetChild(index);
        transform.position = targetPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
                                                 targetPoint.position,
                                                 speed * Time.deltaTime * 0.02f);

        // What happens if we move past/through the tolerance?
        float dist = Vector3.Distance (transform.position, targetPoint.position);
        if (dist < moveToNextTol)
        {
            index++;
            index %= pathParent.childCount;
            targetPoint = pathParent.GetChild(index);
        }

        // Reset camera position
        if (((Time.frameCount % resetAfterNFrames) == 0) && (resetCounter))
        {
            index = 0;
            targetPoint = pathParent.GetChild(index);
            transform.position = targetPoint.position;
        }
    }
}
