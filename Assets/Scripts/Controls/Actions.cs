using UnityEngine;
using UnityEngine.InputSystem;
public class Actions : MonoBehaviour
{
    public static GameInputActionMap inputActionMap;
    InputAction placePerson;
    LayerMask interactableLayerMask;

    InputAction cameraUp;
    InputAction cameraLeft;
    InputAction cameraRight;
    InputAction cameraDown;

    [SerializeField] private GameObject personPrefab;
    [SerializeField] private float cameraMoveSpeed;

    private bool isCameraMovingUp = false;
    private bool isCameraMovingRight = false;
    private bool isCameraMovingLeft = false;
    private bool isCameraMovingDown = false;

    private void Awake()
    {
        if (inputActionMap == null)
        {
            inputActionMap = new GameInputActionMap();
        }
        interactableLayerMask = LayerMask.GetMask("interactable");
    }

    private void OnEnable()
    {
        placePerson = inputActionMap.Player.PlacePerson;
        placePerson.Enable();
        placePerson.performed += PlacePerson;

        //Camera
        cameraUp = inputActionMap.Camera.Up;
        cameraUp.Enable();
        cameraUp.performed += CameraUp;
        cameraUp.canceled += CameraUp;

        cameraLeft = inputActionMap.Camera.Left;
        cameraLeft.Enable();
        cameraLeft.performed += CameraLeft;
        cameraLeft.canceled += CameraLeft;

        cameraRight = inputActionMap.Camera.Right;
        cameraRight.Enable();
        cameraRight.performed += CameraRight;
        cameraRight.canceled += CameraRight;

        cameraDown = inputActionMap.Camera.Down;
        cameraDown.Enable();
        cameraDown.performed += CameraDown;
        cameraDown.canceled += CameraDown;
    }

    private void OnDisable()
    {
        placePerson.Disable();
        placePerson.performed -= PlacePerson;

        cameraUp.Disable();
        cameraLeft.Disable();
        cameraRight.Disable();
        cameraDown.Disable();
    }

    private void PlacePerson(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, interactableLayerMask))
            {
                //Debug.Log(hit.point);
                //Debug.Log("hit");

                Instantiate(personPrefab, hit.point, Quaternion.identity);
            }
        }
    }

    private void Update()
    {
        if (isCameraMovingUp)
        {
            Vector3 camPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(camPos.x, camPos.y, camPos.z + (cameraMoveSpeed * Time.deltaTime));
        }
        if (isCameraMovingDown)
        {
            Vector3 camPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(camPos.x, camPos.y, camPos.z - (cameraMoveSpeed * Time.deltaTime));
        }
        if (isCameraMovingLeft)
        {
            Vector3 camPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(camPos.x - (cameraMoveSpeed * Time.deltaTime), camPos.y, camPos.z);
        }
        if (isCameraMovingRight)
        {
            Vector3 camPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(camPos.x + (cameraMoveSpeed * Time.deltaTime), camPos.y, camPos.z);
        }
    }

    private void CameraUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCameraMovingUp = true;
        }
        else if (context.canceled)
        {
            isCameraMovingUp = false;
        }
    }

    private void CameraLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCameraMovingLeft = true;
        }
        else if (context.canceled)
        {
            isCameraMovingLeft = false;
        }
    }

    private void CameraRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCameraMovingRight = true;
        }
        else if (context.canceled)
        {
            isCameraMovingRight = false;
        }
    }

    private void CameraDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCameraMovingDown = true;
        }
        else if (context.canceled)
        {
            isCameraMovingDown = false;
        }
    }
}