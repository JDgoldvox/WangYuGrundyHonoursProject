using BehaviourTree;
public class PersonBT : Tree
{
    public UnityEngine.Transform[] wayPoints;
    public UnityEngine.Animator animator;
    
    public static float speed = 5f;
    protected override Node InitTree()
    {
        Patrol newNode = new Patrol(transform, wayPoints, animator);

        if (newNode != null)
        {
            UnityEngine.Debug.Log("new node created, its not null");
        }

        return newNode;
    }
}
