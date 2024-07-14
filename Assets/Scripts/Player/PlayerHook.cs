using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    [SerializeField] private float grappleLength;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private Camera cam;


    private Vector3 grapplePoint;
    private DistanceJoint2D joint;

    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        rope.enabled = false;
    }

    void Update()
    {
        HookActivation();
    }


    void HookActivation()
    {
        if (Input.GetMouseButtonDown(1))
        {

            RaycastHit2D hit = Physics2D.Raycast(
               origin: cam.ScreenToWorldPoint(Input.mousePosition),
               direction: Vector2.zero,
               distance: Mathf.Infinity,
               layerMask: grappleLayer
                );


            if (hit.collider != null)
            {

                grapplePoint = hit.point;
                grapplePoint.z = 0;
                joint.connectedAnchor = grapplePoint;
                joint.enabled = true;
                joint.distance = grappleLength;
                rope.enabled = true;
                rope.SetPosition(0, grapplePoint);
                rope.SetPosition(1, transform.position);


            }
        }

        if (Input.GetMouseButtonUp(1))
        {

            joint.enabled = false;
            rope.enabled = false;

        }

        if (rope.enabled == true)
        {

            rope.SetPosition(1, transform.position);
        }




    }
}