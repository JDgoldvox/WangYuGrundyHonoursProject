using UnityEngine;
using UnityEngine.InputSystem;

public class Actions : MonoBehaviour
{
    public static GameInputActionMap inputActionMap;
    InputAction placePerson;
    LayerMask interactableLayerMask;
    LayerMask peopleLayerMask;

    InputAction cameraUp;
    InputAction cameraLeft;
    InputAction cameraRight;
    InputAction cameraDown;
    InputAction cameraDrag;

    InputAction checkInfo;

    [SerializeField] private GameObject personPrefab;
    [SerializeField] private Transform personSpawnParent;
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private float cameraZoomSpeed;
    [SerializeField] private float cameraRotationSpeed;
    [SerializeField] private float cameraSensitivity;
    private float rotationX = 0f;
    private float rotationY = 0f;

    private bool isCameraMovingForward = false;
    private bool isCameraMovingRight = false;
    private bool isCameraMovingLeft = false;
    private bool isCameraMovingBackward = false;
    private bool isCameraDragging = false;

    [SerializeField] private GameObject infoPanelGameObject;
    private InfoPanel infoPanel;

    private void Awake()
    {
        if (inputActionMap == null)
        {
            inputActionMap = new GameInputActionMap();
        }
        interactableLayerMask = LayerMask.GetMask("Plane");
        peopleLayerMask = LayerMask.GetMask("People");

        infoPanel = infoPanelGameObject.GetComponent<InfoPanel>();
    }

    private void OnEnable()
    {
        placePerson = inputActionMap.Player.PlacePerson;
        placePerson.Enable();
        placePerson.performed += PlacePerson;

        //Camera
        cameraUp = inputActionMap.Camera.Forward;
        cameraUp.Enable();
        cameraUp.performed += CameraForward;
        cameraUp.canceled += CameraForward;

        cameraLeft = inputActionMap.Camera.Left;
        cameraLeft.Enable();
        cameraLeft.performed += CameraLeft;
        cameraLeft.canceled += CameraLeft;

        cameraRight = inputActionMap.Camera.Right;
        cameraRight.Enable();
        cameraRight.performed += CameraRight;
        cameraRight.canceled += CameraRight;

        cameraDown = inputActionMap.Camera.Backward;
        cameraDown.Enable();
        cameraDown.performed += CameraBackward;
        cameraDown.canceled += CameraBackward;

        cameraDrag = inputActionMap.Camera.DragCamera;
        cameraDrag.Enable();
        cameraDrag.performed += CameraDrag;
        cameraDrag.canceled += CameraDrag;

        checkInfo = inputActionMap.Player.CheckInfo;
        checkInfo.Enable();
        checkInfo.performed += CheckPlayerInfo;
    }

    private void OnDisable()
    {
        placePerson.Disable();
        placePerson.performed -= PlacePerson;

        checkInfo.Disable();
        checkInfo.performed -= CheckPlayerInfo;

        cameraUp.Disable();
        cameraLeft.Disable();
        cameraRight.Disable();
        cameraDown.Disable();
        cameraDrag.Disable();
    }

    private void PlacePerson(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, interactableLayerMask))
            {
                Instantiate(personPrefab, hit.point, Quaternion.identity, personSpawnParent);
            }
        }
    }

    private void Update()
    {
        Vector3 camPos = Camera.main.transform.position;

        if (isCameraMovingForward)
        {
            var forwardVector = Camera.main.transform.forward;
            var movement = forwardVector * cameraMoveSpeed * Time.deltaTime;
            Camera.main.transform.position += movement;
        }
        if (isCameraMovingBackward)
        {
            var backwardVector = -Camera.main.transform.forward;
            var movement = backwardVector * cameraMoveSpeed * Time.deltaTime;
            Camera.main.transform.position += movement;
        }
        if (isCameraMovingLeft)
        {
            var leftVector = -Camera.main.transform.right;
            var movement = leftVector * cameraMoveSpeed * Time.deltaTime;
            Camera.main.transform.position += movement;
        }
        if (isCameraMovingRight)
        {
            var rightVector = Camera.main.transform.right;
            var movement = rightVector * cameraMoveSpeed * Time.deltaTime;
            Camera.main.transform.position += movement;
        }

        camPos = Camera.main.transform.position;

        //scroll
        var mouse = Mouse.current;
        Camera.main.transform.position = new Vector3(camPos.x, camPos.y - (cameraZoomSpeed * Input.mouseScrollDelta.y * Time.deltaTime), camPos.z);

        //drag

        if (isCameraDragging)
        {
            float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;

            rotationX -= mouseY;
            rotationY += mouseX; 

            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            Camera.main.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }

    }

    private void CameraForward(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCameraMovingForward = true;
        }
        else if (context.canceled)
        {
            isCameraMovingForward = false;
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

    private void CameraBackward(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCameraMovingBackward = true;
        }
        else if (context.canceled)
        {
            isCameraMovingBackward = false;
        }
    }

    private void CameraDrag(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCameraDragging = !isCameraDragging;

            if(isCameraDragging)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; 
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void CheckPlayerInfo(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000, peopleLayerMask))
            {
                infoPanel.DisplayPersonInfo(hit.transform.gameObject);
            }
        }
    }
}