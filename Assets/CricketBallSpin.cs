using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CricketBallSpin : MonoBehaviour
{
    public Transform pitchMarker;
    public float timeToPitch = 0.6f;

    public float spinStrength = 20f;
    public float bounceDeflectionAngle = 12f;
    public float bounceForce = 6f;
    public bool spinRight = true;

    public float resetDelay = 3f;
    public Transform resetPoint;

    Rigidbody rb;

    float timer;
    bool hasThrown;
    bool hasBounced;

    Vector3 startPosition;
    Quaternion startRotation;

    BallPathDrawer pathDrawer;
    public ImgSlider spinSlider;

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

        if (!hasBounced && timer >= timeToPitch)
        {
            BounceBall();
        }
    }
    public void SetSpinDirection(bool toRight)
    {
        spinRight = toRight;
    }
    public void ThrowBall()
    {
        if (hasThrown || !pitchMarker) return;

        pathDrawer.StartDrawing();

        rb.isKinematic = false;

        timer = 0f;
        hasThrown = true;
        hasBounced = false;

        rb.velocity = CalculateLaunchVelocity(
            transform.position,
            pitchMarker.position,
            timeToPitch
        );

        ApplySpin();
    }

    void ApplySpin()
    {
        Vector3 spinAxis = spinRight ? Vector3.forward : Vector3.back;
        rb.angularVelocity = spinAxis * spinStrength;
    }

    void BounceBall()
    {
        hasBounced = true;

        Vector3 v = rb.velocity;
        v.y = 0f;
        rb.velocity = v;

        DeflectVelocity();

        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

        Invoke(nameof(ResetBall), resetDelay);
    }

    void DeflectVelocity()
    {
        float angle = spinRight ? bounceDeflectionAngle : -bounceDeflectionAngle;
        Quaternion deflection = Quaternion.AngleAxis(angle, Vector3.up);
        rb.velocity = deflection * rb.velocity;
    }

    void ResetBall()
    {
        pathDrawer.StopAndClear();
        spinSlider.ResetSlider();
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = resetPoint.position;
        transform.rotation = resetPoint.rotation;

        timer = 0f;
        hasThrown = false;
        hasBounced = false;
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
