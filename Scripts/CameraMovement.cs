using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationDepth;
    [SerializeField] private float smoothSpeed;

    private Vector2 screenCenter;

    private void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    private void FixedUpdate()
    {
        var relativeToNull = -(screenCenter - (Vector2) Input.mousePosition);

        // rotation movement
        if (Input.GetMouseButton(2))
        {
            // TODO!!
            //transform.Rotate(0, relativeToNull.x * rotationSpeed * Time.deltaTime, 0, Space.World);
            //transform.Rotate(relativeToNull.y * rotationSpeed * Time.deltaTime, 0, 0, Space.World);

            return;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * Time.deltaTime * verticalSpeed, Space.World);
        } else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * Time.deltaTime * verticalSpeed, Space.World);
        }

        // position movement
        var desiredPos = new Vector3();

        if (Mathf.Abs(relativeToNull.x) > Screen.width / 2 -5)
            transform.Translate(Vector3.right * relativeToNull.x * Time.deltaTime * moveSpeed, Space.World);
        if (Mathf.Abs(relativeToNull.y) > Screen.height / 2 -5)
            transform.Translate(desiredPos += Vector3.forward * relativeToNull.y * Time.deltaTime * moveSpeed, Space.World);

    }

}
