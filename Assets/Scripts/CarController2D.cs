using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class CarController2D : MonoBehaviour
{
    public float acceleration;
    public float steering;

    private Rigidbody2D body;
    private float h;
    private float v;
    private float driftForce;
    private float topGear;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), string.Format("H: {0}\nV: {1}\nDrift Force: {2}\nVelMag: {3}", h, v, driftForce, topGear));
    }

    void FixedUpdate()
    {
        //using these feels more natural & sort of simulates jerk (velocity, acceleration, jerk, snap, crackle, pop)
        h = -Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //move forward & back
        Vector2 speed = transform.up * (v * acceleration);
        body.AddForce(speed);

        //turn left & right
        float direction = Vector2.Dot(body.velocity, body.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            body.rotation += h * steering * (body.velocity.magnitude / 5.0f);
            //body.AddTorque((h * steering) * (body.velocity.magnitude / 10.0f));
        }
        else
        {
            body.rotation -= h * steering * (body.velocity.magnitude / 5.0f);
            //body.AddTorque((-h * steering) * (body.velocity.magnitude / 10.0f));
        }


        //calculate drag
        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (body.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }
        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        driftForce = Vector2.Dot(body.velocity, body.GetRelativeVector(rightAngleFromForward.normalized));
        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);
        body.AddForce(body.GetRelativeVector(relativeForce));


        //drawing debug lines
        topGear = body.velocity.magnitude;
        Debug.DrawLine((Vector3)body.position, (Vector3)body.GetRelativePoint(rightAngleFromForward), Color.green);
        Debug.DrawLine((Vector3)body.position, (Vector3)body.GetRelativePoint(relativeForce), Color.red);
    }
}