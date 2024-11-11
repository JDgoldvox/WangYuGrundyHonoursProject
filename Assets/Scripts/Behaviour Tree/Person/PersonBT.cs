using BehaviourTree;
public class PersonBT : Tree
{
    public UnityEngine.Transform[] wayPoints;
    
    public static float speed = 30f;
    protected override Node InitTree()
    {
        Patrol newNode = new Patrol(transform, wayPoints);

        if (newNode != null)
        {
            UnityEngine.Debug.Log("new node created, its not null");
        }

        return newNode;
    }
}
