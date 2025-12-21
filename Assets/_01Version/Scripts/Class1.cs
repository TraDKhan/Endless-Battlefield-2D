using UnityEngine;
public class Class1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("AAA");
        gameObject.SetActive(false);
    }
}
    

