using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //Instances
    [SerializeField]
    GameObject bug;
    [SerializeField]
    Transform center;

    //Values
    [SerializeField]
    float gravityValue;
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    LayerMask layers;
    [SerializeField]
    float raycastLength;

    //Components
    Rigidbody2D myRigidbody;

    //Variables
    Vector2 velocity;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector2.zero;

        ApplyGravity();

        if(IsGrounded() == false)
            AirControl();

        if(Input.GetButton("Ball") == false)
        {
            RaycastHit2D touchedSurface = GetTouchedSurface();

            if (touchedSurface)
            {
                if (touchedSurface.normal.x == 0)
                    bug.transform.position = touchedSurface.point + (new Vector2(touchedSurface.normal.x, Mathf.Round(touchedSurface.normal.y)) * 0.5f);
                else if (touchedSurface.normal.y == 0)
                    bug.transform.position = touchedSurface.point + (new Vector2(Mathf.Round(touchedSurface.normal.x), touchedSurface.normal.y) * 0.5f);
                else
                    bug.transform.position = touchedSurface.point + (touchedSurface.normal * 0.5f);

                bug.transform.rotation = Quaternion.FromToRotation(Vector2.up, touchedSurface.normal);

                bug.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        myRigidbody.velocity = velocity;
    }

    void ApplyGravity()
    {
        velocity += (Vector2.down * gravityValue);
    }

    void AirControl()
    {
        velocity += Vector2.right * (movementSpeed * Input.GetAxis("Horizontal"));
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(center.position, Vector2.down, raycastLength, layers);

        if (hit.collider != null)
            return true;

        return false;
    }

    RaycastHit2D GetTouchedSurface()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(center.position, Vector2.down, raycastLength, layers);
        if (hit.collider != null)
            return hit;

        hit = Physics2D.Raycast(center.position, Vector2.right, raycastLength, layers);
        if (hit.collider != null)
            return hit;

        hit = Physics2D.Raycast(center.position, Vector2.left, raycastLength, layers);
        if (hit.collider != null)
            return hit;

        hit = Physics2D.Raycast(center.position, Vector2.up, raycastLength, layers);
        if (hit.collider != null)
            return hit;

        return hit;
    }
}
