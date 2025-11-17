
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Parts")]
    public Rigidbody2D frontWheel;
    public Rigidbody2D backWheel;
    public Rigidbody2D carBody;

    [Header("Speed Settings")]
    public float baseSpeed = 0f;
    public float maxSpeed = 600f;
    public float accelerationRate = 200f;
    public float decelerationRate = 100f;
    public float brakeRate = 800f;


    public float rotationSpeed = 100f;

    
    public float retryHeightOffset = 2f; 

    private float currentSpeed = 0f;
    private float moveInput;
    private bool isBoosting;
    private bool isBraking;
    private Vector2 lastPosition;
    private float stuckTime = 0f;
    private const float stuckThreshold = 5f;
    private const float moveThreshold = 0.05f;

    private Vector3 startCarPos;
    private Quaternion startCarRot;
    private Vector3 startFrontWheelPos;
    private Quaternion startFrontWheelRot;
    private Vector3 startBackWheelPos;
    private Quaternion startBackWheelRot;

    void Start()
    {
        currentSpeed = baseSpeed;
        lastPosition = carBody.position;

        
        startCarPos = carBody.transform.position;
        startCarRot = carBody.transform.rotation;
        startFrontWheelPos = frontWheel.transform.position;
        startFrontWheelRot = frontWheel.transform.rotation;
        startBackWheelPos = backWheel.transform.position;
        startBackWheelRot = backWheel.transform.rotation;
    }

    void Update()
    {
        if (!GameManager.gameStarted) return;

        moveInput = Input.GetAxisRaw("Horizontal");
        isBoosting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        isBraking = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        
        float movement = (carBody.position - lastPosition).magnitude;
        stuckTime = (movement < moveThreshold) ? stuckTime + Time.deltaTime : 0f;

        if (stuckTime >= stuckThreshold)
        {
            GameManager.instance.GameOver();
        }

        
        if (movement > moveThreshold)
        {
            GameManager.instance.UpdateLastSafePoint(carBody.position, carBody.transform.rotation);
        }

        
        if (Mathf.Abs(carBody.rotation) > 120f)
        {
            GameManager.instance.GameOver();
        }

        lastPosition = carBody.position;
    }

    void FixedUpdate()
    {
        if (!GameManager.gameStarted) return;

        if (isBoosting)
            currentSpeed += accelerationRate * Time.fixedDeltaTime;
        else if (isBraking)
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakeRate * Time.fixedDeltaTime);

        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

       
        float torque = -currentSpeed * moveInput * Time.fixedDeltaTime;
        frontWheel.AddTorque(torque);
        backWheel.AddTorque(torque);
    }

    public void ResetCar()
    {
      
        ResetToPosition(startCarPos, startCarRot, startFrontWheelPos, startFrontWheelRot, startBackWheelPos, startBackWheelRot);
    }

    public void RetryFromLastSafe(Vector3 lastSafePos, Quaternion lastSafeRot)
    {
        
        Vector3 frontWheelOffset = startFrontWheelPos - startCarPos;
        Vector3 backWheelOffset = startBackWheelPos - startCarPos;

     
        Vector3 spawnPos = lastSafePos + new Vector3(0, retryHeightOffset, 0);

       
        Vector3 newFrontWheelPos = spawnPos + frontWheelOffset;
        Vector3 newBackWheelPos = spawnPos + backWheelOffset;

       
        ResetToPosition(spawnPos, Quaternion.identity, newFrontWheelPos, Quaternion.identity, newBackWheelPos, Quaternion.identity);
    }

    private void ResetToPosition(Vector3 carPos, Quaternion carRot, Vector3 frontPos, Quaternion frontRot, Vector3 backPos, Quaternion backRot)
    {
        
        carBody.linearVelocity = Vector2.zero;
        carBody.angularVelocity = 0f;
        frontWheel.linearVelocity = Vector2.zero;
        frontWheel.angularVelocity = 0f;
        backWheel.linearVelocity = Vector2.zero;
        backWheel.angularVelocity = 0f;

       
        carBody.transform.position = carPos;
        carBody.transform.rotation = carRot;
        frontWheel.transform.position = frontPos;
        frontWheel.transform.rotation = frontRot;
        backWheel.transform.position = backPos;
        backWheel.transform.rotation = backRot;

       
        stuckTime = 0;
        currentSpeed = 0;
        lastPosition = carBody.position;
    }
}