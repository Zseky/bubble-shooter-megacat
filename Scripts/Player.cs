using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject BubblePrefab;
    public Transform ShootPoint;
    //public float ShootForce = 10f;

    public GameObject GridReference;
    void Start()
    {
        
    }




    void Update()
    {
        _aim();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _shoot();
        }
    }

    private void _aim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0,0,angle - 90));
    }
    private void _shoot()
    {
        GameObject bubble = Instantiate(BubblePrefab, ShootPoint.position, ShootPoint.rotation, GridReference.transform);
        bubble.GetComponent<Bubble>().GridReference = GridReference;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bubble.GetComponent<Bubble>().AimPoint = mousePos;
        bubble.GetComponent<Bubble>().AimPoint.z = 0;
    }
}
