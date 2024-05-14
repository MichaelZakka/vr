using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System;

public class SPHFluidSimulation : MonoBehaviour
{
    // List<Particle> particles = new List<Particle>();
    List<Particle> particles = new List<Particle>();
    public GameObject sphere;

    float gasConstant =8.314f;
    public float spawnRadius = 5f;
    public Vector3 pos = new Vector3(0, 1, 0);
    public float gravity;
    void Start()
    {
        for (int i = 0; i < 250; i++)
        {
            Vector3 randomVector = new Vector3(
            UnityEngine.Random.Range(0f, 20f),
            UnityEngine.Random.Range(0f, 20f),
            UnityEngine.Random.Range(0f, 20f)
            );
            Debug.Log(randomVector);

            GameObject oneParticle = Instantiate(sphere, randomVector, Quaternion.identity);

            Particle p = oneParticle.AddComponent<Particle>();

            // particle = p;

            oneParticle.transform.position = randomVector;
            particles.Add(p);
            p.velocity = new Vector3(
                UnityEngine.Random.Range(-5, 5),
                UnityEngine.Random.Range(-5, 5),
                UnityEngine.Random.Range(-5, 5));

            //    sphereObject.transform.position = particleComponent.position

            p.sphere = oneParticle;
        }


    }
    void Update()
    {
        foreach (Particle particle in particles)
        {

            particle.position += particle.velocity * Time.deltaTime;
            particle.sphere.transform.position = particle.position;
            Debug.Log("inside update");

            ResolveCollision(particle);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // particle.velocity += Vector3.down * gravity * Time.deltaTime;
            // particle.sphere.transform.position = particle.position;
        }
        // pos += particle.velocity * Time.deltaTime;
        // particle.position = pos;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // particle.position.y += 0.005f;
        // particle.sphere.transform.position = particle.position;
    }

    void ResolveCollision(Particle particle)
    {
        if (particle.position.y < 0)
        {
            particle.position.y = 0.5f * Math.Sign(particle.position.y);
            particle.sphere.transform.position = particle.position;
            particle.velocity.y *= -1 * 0.01f;
        }
    }
}
