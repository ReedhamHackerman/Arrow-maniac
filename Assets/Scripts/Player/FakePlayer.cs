using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    
    public GameObject gameob;
    public float arrowForce;
   

    // Update is called once per frame
    void Update()
    {
        Vector3 rotateAround = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
        rotateAround.z = 0;
        transform.right = -rotateAround;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        GameObject arrowIns = Instantiate(gameob, transform.position, transform.rotation);
        arrowIns.GetComponent<Rigidbody2D>().velocity = transform.right * arrowForce;
    }
}
