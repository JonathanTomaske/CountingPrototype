using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //public GameObject spherePrefab;
    //public GameObject cubePrefab;
    [SerializeField] private ObjectPooler[] objectPools;
    [SerializeField] private GameObject go_LaunchSpawn;
    [SerializeField] private GameObject go_LaunchTarget;
    [SerializeField] private float sphereLaunchForce;
    [SerializeField] private float cubeLaunchForce;

    private Vector3 launchDirection;

    // Start is called before the first frame update
    void Start()
    {
        objectPools = GetComponents<ObjectPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchPooledObj("Sphere", sphereLaunchForce);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            LaunchPooledObj("Cube", cubeLaunchForce);
        }
    }

    void LaunchPooledObj(string objType, float launchForce)
    {
        foreach (ObjectPooler obPool in objectPools)
        {
            if (obPool.objectToPool.tag == objType)
            {
                GameObject pooledProjectile = obPool.GetPooledObject();
                if (pooledProjectile != null)
                {
                    pooledProjectile.SetActive(true); // activate it
                    pooledProjectile.transform.position = go_LaunchSpawn.transform.position;  // position object at end of our launcher object
                    launchDirection = (go_LaunchTarget.transform.position - go_LaunchSpawn.transform.position).normalized;  //set the direction as going down the launcher
                    pooledProjectile.GetComponent<Rigidbody>().AddForce(launchDirection * launchForce, ForceMode.Impulse);
                }
            }
        }
    }

}
