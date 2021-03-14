using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugController : MonoBehaviour
{
    //Values
    [SerializeField]
    float movementSpeed;

    //Components
    Rigidbody2D myRigidbody;

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
        Move();
    }

    void Move()
    {
        Debug.Log("here");
        myRigidbody.velocity = transform.right * (movementSpeed * Input.GetAxis("Horizontal"));
    }
}
