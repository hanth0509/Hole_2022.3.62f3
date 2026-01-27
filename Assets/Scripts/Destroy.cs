using UnityEngine;
public class Destroy: MonoBehaviour 
{
    private void OnTriggerEnter(Collider other)
    {
        ObjectSize objectSize = other.GetComponent<ObjectSize>();
        if (objectSize != null)
        {
            if(Manager.instance!=null)
            {
                Manager.instance.AddScore(objectSize.score);
            }
        }
        Destroy(other.gameObject);
    }
}