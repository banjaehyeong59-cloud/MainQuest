using UnityEngine;

public class ItemMagnet : MonoBehaviour
{

    private Item parentItem;

    void Awake()
    {
        parentItem = GetComponentInParent<Item>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (parentItem != null)
            {
                parentItem.StartAttraction(other.transform);
            }
        }
    }
}