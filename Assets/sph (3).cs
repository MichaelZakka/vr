using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System;
using System.Xml.Serialization;

public class SPHFluidSimulation : MonoBehaviour
{
    // List<Particle> particles = new List<Particle>();
    List<Particle> particles = new List<Particle>();
    public GameObject sphere;
    public float particleRadius = 0.5f;
    public float gasConstant = 287.05f; // Specific gas constant for dry air (J/(kg·K))
    public float referenceViscosity = 1.81e-5f; // Reference viscosity (Pa·s)
    public float referenceTemperature = 293.15f; // Reference temperature (K)
    public float sutherlandConstant = 110.4f; // Sutherland's constant (K)
    public float ambientTemperature = 298.15f; // Ambient temperature (K)
    public float ambientPressure = 101325f; // Ambient pressure (Pa)

    public float spawnRadius = 5f;
    public float windSpeed = 5f;
    public float windStrength = 1f;
    public float windFrequency = 1f;
    private float timeCounter = 0f;
    public float gravity = 9.81f;


    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomVector = new Vector3(
            UnityEngine.Random.Range(-spawnRadius, spawnRadius),
            UnityEngine.Random.Range(-spawnRadius, spawnRadius),
            UnityEngine.Random.Range(-spawnRadius, spawnRadius)
            );
            Debug.Log(randomVector);

            GameObject oneParticle = Instantiate(sphere, randomVector, Quaternion.identity);

            Particle p = oneParticle.AddComponent<Particle>();


            p.position = randomVector;
            p.velocity = Vector3.zero;
            p.sphere = oneParticle;
            particles.Add(p);
            p.density = CalculateDensity(p);
            p.mass = calculateMass(p);


            /*p.velocity = new Vector3(
                UnityEngine.Random.Range(-5, 5),
                UnityEngine.Random.Range(-5, 5),
                UnityEngine.Random.Range(-5, 5));
            */
            //    sphereObject.transform.position = particleComponent.position


        }


    }
    void Update()
    {
        timeCounter += Time.deltaTime;
        for (int i = 0; i < particles.Count; i++)
        {
            Particle particle = particles[i];
            // calculateWindForce(particle);
            Vector3 forces = CalculateForces(particle);
            calculateGravity(particle);
            ResolveCollision(particle);
            Debug.Log(particle.mass);
            integrate(particle, forces);

        }

        for (int i = 0; i < particles.Count; i++)
        {
            for (int j = i + 1; j < particles.Count; j++)
            {
                ResolveCollisionForParticles(particles[i], particles[j]);
            }
        }
    }

    float CalculateDensity(Particle particle)
    {
        return ambientPressure / (gasConstant * ambientTemperature);
    }

    float CalculatePressure(Particle particle)
    {
        return particle.density * gasConstant * ambientTemperature;
    }

    Vector3 CalculateForces(Particle particle)
    {
        Vector3 pressureForce = new Vector3(
            Mathf.Cos(timeCounter * CalculatePressure(particle) * Mathf.Sign(particle.position.x)),
            Mathf.Sin(timeCounter * CalculatePressure(particle) * Mathf.Sign(particle.position.x)),
            Mathf.Cos(timeCounter * CalculatePressure(particle) * Mathf.Sign(particle.position.x)));
        // Vector3 viscousForce = new Vector3();

        return pressureForce;
    }



    float calculateMass(Particle particle){
        return 1.2f;
    }
    void integrate(Particle particle, Vector3 forces)
    {
        particle.velocity += forces / particle.mass * Time.deltaTime;
        particle.position += particle.velocity * Time.deltaTime;
        particle.sphere.transform.position = particle.position;
    }
    // void calculateWindForce(Particle particle)
    // {
    //     Vector3 windDirection = new Vector3(
    //         Mathf.Cos(timeCounter * windFrequency) * Mathf.Sign(particle.position.x),
    //         Mathf.Sin(timeCounter * windFrequency) * Mathf.Sign(particle.position.y),
    //         Mathf.Sin(timeCounter * windFrequency) * Mathf.Sign(particle.position.z)
    //     );
    //     Vector3 windForce = windDirection.normalized * windSpeed * windStrength;
    //     particle.velocity += windForce * Time.deltaTime;
    //     particle.position += windForce * Time.deltaTime;
    //     particle.sphere.transform.position = particle.position;
    // }
    void ResolveCollision(Particle particle)
    {
        if (particle.position.y < 0)
        {
            particle.position.y = 0;
            particle.velocity.y *= -0.5f;
            particle.sphere.transform.position = particle.position;

        }
    }
    void calculateGravity(Particle particle)
    {
        particle.velocity += Vector3.down * gravity * Time.deltaTime * particle.mass;
        particle.position += particle.velocity * Time.deltaTime;
        particle.sphere.transform.position = particle.position;
    }
    void ResolveCollisionForParticles(Particle p1, Particle p2)
    {
        Vector3 delta = p1.position - p2.position;
        float distance = delta.magnitude;
        float minDistance = 2 * particleRadius;

        if (distance < minDistance)
        {
            Vector3 normal = delta.normalized;
            float penetrationDepth = minDistance - distance;
            Vector3 correction = normal * (penetrationDepth / 2);

            p1.position += correction;
            p2.position -= correction;

            p1.velocity = Vector3.Reflect(p1.velocity, normal);
            p2.velocity = Vector3.Reflect(p2.velocity, normal);

            p1.sphere.transform.position = p1.position;
            p2.sphere.transform.position = p2.position;
        }
    }
}
