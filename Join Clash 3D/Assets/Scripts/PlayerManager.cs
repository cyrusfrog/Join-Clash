using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool MoveByTouch;
    private Vector3 Direction;
    public List<Rigidbody> Rblist = new List<Rigidbody>();
    [SerializeField] private float runSpeed, velocity, swipeSpeed, roadSpeed;
    [SerializeField] private Transform road;
    public static PlayerManager PlayerManagerCls;
    void Start()
    {
        PlayerManagerCls = this;
        Rblist.Add(transform.GetChild(0).GetComponent<Rigidbody>());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MoveByTouch = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            MoveByTouch= false;
        }
        if(MoveByTouch)
        {
            Direction = new Vector3(Mathf.Lerp(Direction.x, Input.GetAxis("Mouse X"), runSpeed * Time.deltaTime), 0f);
            Direction = Vector3.ClampMagnitude(Direction, 1f);
            road.position = new Vector3(0f , 0f , Mathf.SmoothStep(road.position.z, -100f , Time.deltaTime * roadSpeed));
            
            foreach(var stickman_Anim in Rblist)
                stickman_Anim.GetComponent<Animator>().SetFloat("run", 1f);
        }
        else
        {
            foreach(var stickman_Anim in Rblist)
                stickman_Anim.GetComponent<Animator>().SetFloat("run", 0f);
        }

        foreach (var stickman_rb in Rblist)
        {
            if (stickman_rb.velocity.magnitude > 0.5f)
            {
                stickman_rb.rotation = Quaternion.Slerp(stickman_rb.rotation, Quaternion.LookRotation(stickman_rb.velocity, Vector3.up), Time.deltaTime * velocity);

            }
            else
            {
                stickman_rb.rotation = Quaternion.Slerp(stickman_rb.rotation, Quaternion.identity, Time.deltaTime * velocity);
            }
        }
    }

    private void FixedUpdate()
    {
        if(MoveByTouch)
        {
            Vector3 displacement = new Vector3(Direction.x, 0f, 0f) * Time.fixedDeltaTime;

            foreach (var stickman_rb in Rblist)
                stickman_rb.velocity = new Vector3(Direction.x *Time.fixedDeltaTime*swipeSpeed, 0f, 0f);
        }
        else
        {
            foreach (var stickman_rb in Rblist)
                stickman_rb.velocity = Vector3.zero;
        }
    }
    
}
