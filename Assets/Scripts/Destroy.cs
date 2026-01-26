using UnityEngine;
public class Destroy: MonoBehaviour 
{
    private void OnTriggerEnter(Collider other)
    {
        ObjectSize objectSize = other.GetComponent<ObjectSize>();
        if (objectSize != null)
        {
            // objectSize.score += objectSize.score;
        }
        Destroy(other.gameObject);
    }
}