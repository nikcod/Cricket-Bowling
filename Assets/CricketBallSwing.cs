using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CricketBallSwing : MonoBehaviour
{
    public Transform pitchMarker;
    public float timeToPitch = 0.6f;

    public float swingForce = 4f;
    public float swingDelay = 0.15f;
    public float bounceForce = 4f;
    public bool swingRight = true;

    public float resetDelay = 3f;
    public Transform resetPoint;

    Rigidbody rb;

    float timer;
    bool hasThrown;
    bool hasPitched;

    Vector3 startPosition;
    Quaternion startRotation;

    BallPathDrawer pathDrawer;
    public ImgSlider swingSlider;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        startPosition = transform.position;
        startRotation = transform.rotation;

        pathDrawer = GetComponent<BallPathDrawer>();
    }

    void FixedUpdate()
    {
        if (!hasThrown) return;

        timer += Time.fixedDeltaTime;

        if (!hasPitched && timer > swingDelay && timer < timeToPitch)
        {
            Vector3 travelDir = rb.velocity.normalized;
            Vector3 swingDir = swingRight
                ? Vector3.Cross(Vector3.up, travelDir)
                : Vector3.Cross(travelDir, Vector3.up);

            rb.AddForce(swingDir * swingForce, ForceMode.Force);
        }

        if (!hasPitched && timer >= timeToPitch)
        {
            PitchBall();
        }
    }
    public void setSwingRight(bool isRight)
    {
        swingRight = isRight;
    }
    public void ThrowBall()
    {
        if (hasThrown || !pitchMarker) return;

        pathDrawer.StartDrawing();
        rb.isKinematic = false;

        timer = 0f;
        hasThrown = true;
        hasPitched = false;

        float swingTime = Mathf.Max(0f, timeToPitch - swingDelay);
        float swingAcceleration = swingForce / rb.mass;
        float lateralDisplacement = 0.5f * swingAcceleration * swingTime * swingTime;

        Vector3 forwardDir = (pitchMarker.position - transform.position).normalized;

        Vector3 swingDir = swingRight
            ? Vector3.Cross(Vector3.up, forwardDir)
            : Vector3.Cross(forwardDir, Vector3.up);

        Vector3 compensatedTarget =
            pitchMarker.position - swingDir.normalized * lateralDisplacement;

        rb.velocity = CalculateLaunchVelocity(
            transform.position,
            compensatedTarget,
            timeToPitch
        );
    }


    void PitchBall()
    {
        hasPitched = true;

        Vector3 v = rb.velocity;
        v.y = 0f;
        rb.velocity = v;

        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

        Invoke(nameof(ResetBall), resetDelay);
    }

    void ResetBall()
    {
        pathDrawer.StopAndClear();
        swingSlider.ResetSlider();
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = resetPoint.position;
        transform.rotation = resetPoint.rotation;

        timer = 0f;
        hasThrown = false;
        hasPitched = false;
    }

    Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float time)
    {
        Vector3 displacement = end - start;
        Vector3 displacementXZ = new Vector3(displacement.x, 0f, displacement.z);

        float y = displacement.y;
        float xz = displacementXZ.magnitude;

        float vxz = xz / time;
        float vy = (y - 0.5f * Physics.gravity.y * time * time) / time;

        Vector3 result = displacementXZ.normalized * vxz;
        result.y = vy;

        return result;
    }
}
