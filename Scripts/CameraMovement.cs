using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationDepth;
    private Vector2 screenCenter;

    private void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    private void Update()
    {
        Vector2 relativeToNull = -(screenCenter - (Vector2)Input.mousePosition);

        // rotation movement
        if (Input.GetMouseButton(2))
        {
            // TODO!!
            //transform.Rotate(0, relativeToNull.x * rotationSpeed * Time.deltaTime, 0, Space.World);
            //transform.Rotate(relativeToNull.y * rotationSpeed * Time.deltaTime, 0, 0, Space.World);

            return;
        }

        // position movement
        transform.Translate(Vector3.right * relativeToNull.x * Time.deltaTime * moveSpeed, Space.World);
        transform.Translate(Vector3.forward * relativeToNull.y * Time.deltaTime * moveSpeed, Space.World);

    }

}
