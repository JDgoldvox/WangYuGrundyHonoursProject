using UnityEngine;

public class PersonDebug : MonoBehaviour
{
    PersonBT S_PersonBT;
    Transform modelTransform;

    private void Awake()
    {
        S_PersonBT = GetComponent<PersonBT>();
        modelTransform = S_PersonBT.transform.GetChild(0).transform;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (S_PersonBT != null)
        {
            //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
            Gizmos.DrawWireSphere(modelTransform.position, S_PersonBT.visionRange);
        }


        Gizmos.color = Color.green;
        if (S_PersonBT != null)
        {
            //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
            Gizmos.DrawWireSphere(S_PersonBT.transform.position, S_PersonBT.visionRange);
        }

    }
}
