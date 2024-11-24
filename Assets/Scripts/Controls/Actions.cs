using UnityEngine;
using UnityEngine.InputSystem;
public class Actions : MonoBehaviour
{
    public static GameInputActionMap inputActionMap;
    InputAction placePerson;
    LayerMask interactableLayerMask;

    [SerializeField] private GameObject personPrefab;

    private void Awake()
    {
        if(inputActionMap == null)
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
    }

    private void OnDisable()
    {
        placePerson.Disable();
        placePerson.performed -= PlacePerson;
    }

    private void PlacePerson(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, interactableLayerMask))
            {
                Debug.Log(hit.point);
                Debug.Log("hit");

                Instantiate(personPrefab, hit.point, Quaternion.identity);
            }
        }

       
    }
}
