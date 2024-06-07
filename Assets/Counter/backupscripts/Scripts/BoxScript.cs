using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using UnityEngine.Jobs;

using math = Unity.Mathematics.math;
using random = Unity.Mathematics.Random;

public class BoxScript : MonoBehaviour
{

    // 1
    private NativeArray<Vector3> velocities;

    // 2
    private TransformAccessArray transformAccessArray;
    private NativeArray<Vector3> boxBoundsArray;


    [Header("References")]
    //object we'll use to define movement area for the boxes
    public Transform moveArea;
    //object that we'll have a bunch of and move around with the job system
    public Transform objectPrefab;

    [Header("Spawn Settings")]
    public int amountOfBoxes;
    public Vector3 spawnBounds; //total area
    public float spawnHeight;
    public int directionChangeFrequency;

    [Header("Settings")]
    public float moveSpeed;
    public float turnSpeed;

    private PositionUpdateJob positionUpdateJob; //Struct made in this class

    private JobHandle positionUpdateJobHandle;

    // Start is called before the first frame update
    void Start()
    {
        int boxCount = 0;

        // 1
        velocities = new NativeArray<Vector3>(amountOfBoxes, Allocator.Persistent);
        boxBoundsArray = new NativeArray<Vector3>(amountOfBoxes, Allocator.Persistent);
        

        // 2 TransformAccessArray holds transforms for objects and is optimized for the job system over using an Array of Transform objects
        transformAccessArray = new TransformAccessArray(amountOfBoxes);

        //for (int i = 0; i < amountOfBoxes; i++)
        //{
            float distanceX;
            float distanceZ;
            float xBound = -spawnBounds.x;
            float zBound = -spawnBounds.z;
            float boxDivisions = (spawnBounds.x / math.sqrt(amountOfBoxes) * 2);
            
            //for (float xBound = spawnBounds.x; xBound > -spawnBounds.x; xBound-boxDivisions)
            while(xBound < spawnBounds.x)
            {
                distanceX = xBound; //+ (boxDivisions / 2);
                //Debug.Log("xBound: " + xBound);
                //for (float zBound = spawnBounds.z; zBound > -spawnBounds.z; zBound-boxDivisions)
                while (zBound < spawnBounds.z)
                {
                    distanceZ = zBound;// + (boxDivisions / 2);

                    Debug.Log("spawning at X:" + distanceX + ", Z:" + distanceZ);

                    // 3
                    Vector3 spawnPoint = (transform.position + Vector3.up * spawnHeight) + new Vector3(distanceX, 0, distanceZ);

                    // 4
                    Transform t = (Transform)Instantiate(objectPrefab, spawnPoint, Quaternion.identity);

                    // 5
                    transformAccessArray.Add(t);
                    boxBoundsArray[boxCount] = new Vector3(xBound, 0, zBound);

                    zBound += boxDivisions;

                    
                    boxCount++;
                }
                zBound = -spawnBounds.z;
                xBound += boxDivisions;
            }
            //Debug.Log("Final xBound: " + xBound);


        //float distanceX =
        //Random.Range(-spawnBounds.x / 2, spawnBounds.x / 2);

        //float distanceZ =
        //Random.Range(-spawnBounds.z / 2, spawnBounds.z / 2);


        //}
    }

    // Update is called once per frame
    void Update()
    {

        // 1
        positionUpdateJob = new PositionUpdateJob()
        {
            objectVelocities = velocities,
            objectBounds = boxBoundsArray,
            jobDeltaTime = Time.deltaTime,
            moveSpeed = this.moveSpeed,
            turnSpeed = this.turnSpeed,
            time = Time.time,
            directionChangeFrequency = this.directionChangeFrequency,
            center = moveArea.position,
            bounds = spawnBounds,
            direction = new Vector3(0, 0, 1),
            seed = System.DateTimeOffset.Now.Millisecond,
            boundCheckTimer = 1f,
            randomize = false
        };

        //trying to set a random speed per box
        //float seed = System.DateTimeOffset.Now.Millisecond;
        //random randomGen = new random((uint)( * Time.deltaTime + 1 + seed));
        //moveSpeed = randomGen.NextFloat(1, 10);

        // 2
        positionUpdateJobHandle = positionUpdateJob.Schedule(transformAccessArray);
    }

    private void LateUpdate()
    {
        positionUpdateJobHandle.Complete();
    }

    private void OnDestroy()
    {
        transformAccessArray.Dispose();
        velocities.Dispose();
        boxBoundsArray.Dispose();
    }


    [BurstCompile]
    struct PositionUpdateJob : IJobParallelForTransform
    {

        public NativeArray<Vector3> objectVelocities;
        public NativeArray<Vector3> objectBounds;

        public Vector3 bounds;
        public Vector3 center;
        public Vector3 direction;

        public float jobDeltaTime;
        public float time;
        public float moveSpeed;
        public float turnSpeed;
        public int directionChangeFrequency;

        public float seed;
        private float turnDegrees;
        public float boundCheckTimer;

        public bool randomize;

        public void Execute(int i, TransformAccess transform)
        {
            //random randomGen = new random((uint)(i + time + 1 + seed));

            //float nextIntSave = randomGen.NextInt(0, directionChangeFrequency);
            //if (nextIntSave <= 2f)
            //{
            //    int randomDirection = randomGen.NextInt(0, 4);
            //    if (randomDirection == 0)
            //    {
            //        direction = new Vector3(0, 0, 1);
            //    }
            //    else if (randomDirection == 1)
            //    {
            //        direction = new Vector3(1, 0, 0);
            //    }
            //    else if (randomDirection == 2)
            //    {
            //        direction = new Vector3(0, 0, -1);
            //    }
            //    else if (randomDirection == 3)
            //    {
            //        direction = new Vector3(-1, 0, 0);
            //    }
            //}

            transform.position += transform.localToWorldMatrix.MultiplyVector(direction * jobDeltaTime * moveSpeed);

            if (transform.position.x > center.x + bounds.x)
            {
                direction = new Vector3(-1, 0, 0);
            }
            else if (transform.position.x < center.x - bounds.x)
            {
                direction = new Vector3(1, 0, 0);
            }
            else if (transform.position.z > center.z + bounds.z)
            {
                direction = new Vector3(0, 0, -1);
            }
            else if (transform.position.z < center.z - bounds.z)
            {
                direction = new Vector3(0, 0, 1);
            }
        }
    }

}
