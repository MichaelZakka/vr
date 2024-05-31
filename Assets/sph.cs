// using UnityEngine;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine.UIElements;
// using System;

// public class SPHFluidSimulation : MonoBehaviour
// {
//     // List<Particle> particles = new List<Particle>();

//     public float gravity = -9.81f;
//     public float gasConstant = 287.05f; // Specific gas constant for dry air (J/(kg·K))
//     public float referenceViscosity = 1.81e-5f; // Reference viscosity (Pa·s)
//     public float referenceTemperature = 293.15f; // Reference temperature (K)
//     public float sutherlandConstant = 110.4f; // Sutherland's constant (K)
//     public float ambientTemperature = 298.15f; // Ambient temperature (K)
//     public float ambientPressure = 101325f; // Ambient pressure (Pa)
//     List<Particle> particles = new List<Particle>();
//     public GameObject sphere;
//     // public float spawnRadius = 5f;
//     public Vector3 pos = new Vector3(0, 1, 0);

//     public float windSpeed = 5f;
//     public Vector3 windDirection = new Vector3(1, 0, 0);
//     public float windStrength = 1f;
//     public float windFrequency = 1f;
//     private float timeCounter = 0f;
//     public float tunnelIntensity = 2f;
//     public float tunnelFrequency = 1f;
//     private Vector3 tunnelOffset;
//     void Start()
//     {

//         for (int i = 0; i < 250; i++)
//         {
//             Vector3 randomVector = new Vector3(
//             UnityEngine.Random.Range(0f, 20f),
//             UnityEngine.Random.Range(0f, 20f),
//             UnityEngine.Random.Range(0f, 20f)
//             );
//             // Debug.Log(randomVector);

//             GameObject oneParticle = Instantiate(sphere, randomVector, Quaternion.identity);

//             Particle p = oneParticle.AddComponent<Particle>();
//             particle = p;

//             p.position = randomVector;
//             p.velocity = Vector3.zero;
//             p.sphere = oneParticle;
//             particles.Add(p);

//             tunnelOffset = Vector3.zero;
//             /*p.velocity = new Vector3(
//                 UnityEngine.Random.Range(-5, 5),
//                 UnityEngine.Random.Range(-5, 5),
//                 UnityEngine.Random.Range(-5, 5));
//             */
//             //    sphereObject.transform.position = particleComponent.position


//         }


//     }
//     void Update()
//     {

//         timeCounter += Time.deltaTime;
//         foreach (var particle in particles)
//         {
//             // Calculate properties
//             particle.density = CalculateDensity(particle);
//             particle.pressure = CalculatePressure(particle);

//             // Calculate forces
//             Vector3 forces = CalculateForces(particle);
//             // Debug.Log(forces);

//             // Integrate motion equations
//             Integrate(particle, forces);
            
//         }

//         // Handle collisions
//         HandleCollisions();


//         foreach (var particle in particles)
//                     {
//                         // Calculate properties
//                         particle.density = CalculateDensity(particle);
//                         particle.pressure = CalculatePressure(particle);

//                         // Calculate forces
//                         Vector3 forces = CalculateForces(particle);

//                         // Integrate motion equations
//                         Integrate(particle, forces);
//                     }
//         foreach (Particle particle in particles)
//         {
//             tunnelOffset += new Vector3(
//             Mathf.PerlinNoise(Time.time * tunnelFrequency, 0),
//             Mathf.PerlinNoise(0, Time.time * tunnelFrequency),
//             Mathf.PerlinNoise(Time.time * tunnelFrequency, Time.time * tunnelFrequency)
//             ) * tunnelIntensity * Time.deltaTime;
//             calculateWindForce(particle);
//             Debug.Log("inside update");

//         ResolveCollision(particle);
//         //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//             particle.velocity += Vector3.down * gravity * Time.deltaTime;
//             particle.sphere.transform.position = particle.position;
//         }
//         pos += particle.velocity * Time.deltaTime;
//         particle.position = pos;
//         ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//         // particle.position.y += 0.005f;
//         // particle.sphere.transform.position = particle.position;
//     }
//     void calculateWindForce(Particle particle)
//     {
//         Vector3 windDirection = new Vector3(
//             Mathf.Cos(timeCounter * windFrequency) * Mathf.Sign(particle.position.x),
//             Mathf.Sin(timeCounter * windFrequency) * Mathf.Sign(particle.position.y),
//             Mathf.Sin(timeCounter * windFrequency) * Mathf.Sign(particle.position.z)
//         );
//         Vector3 windForce = windDirection.normalized * windSpeed * windStrength;
//         particle.position += windForce * Time.deltaTime;
//         particle.sphere.transform.position = particle.position;
//     }
//     void ResolveCollision(Particle particle)
//     {
//         if (particle.position.y < 0)
//         {
//             particle.position.y = 0.5f * Math.Sign(particle.position.y);
//             particle.sphere.transform.position = particle.position;
//             particle.velocity.y *= -1 * 0.01f;
//         }
//     }

//     float CalculateDensity(Particle particle)
//     {
//         return ambientPressure / (gasConstant * ambientTemperature);
//     }

//     float CalculatePressure(Particle particle)
//     {
//         return particle.density * gasConstant * ambientTemperature;
//     }

//     Vector3 CalculateForces(Particle particle)
//     {
//         Vector3 pressureForce =new Vector3(CalculatePressure(particle) ,CalculatePressure(particle) ,CalculatePressure(particle));
//         Vector3 viscousForce = Vector3.zero;
//         Vector3 gravityForce = new Vector3(0, gravity * particle.mass, 0);

//         return pressureForce + viscousForce + gravityForce;
//     }

//     void Integrate(Particle particle, Vector3 forces)
//     {
//         // Semi-implicit Euler integration
//         particle.velocity += forces / particle.mass * Time.deltaTime;
//         particle.position += particle.velocity * Time.deltaTime;
//         // particle.sphere.transform.position =particle.position;
//     }

//     void HandleCollisions()
//     {
//         // Simple collision detection and response
//         for (int i = 0; i < particles.Count; i++)
//         {
//             for (int j = i + 1; j < particles.Count; j++)
//             {
//                 Particle particle1 = particles[i];
//                 Particle particle2 = particles[j];
//                 Vector3 displacement = particle1.position - particle2.position;
//                 float distance = displacement.magnitude;
//                 float collisionRadius = 0.1f; // Example collision radius

//                 if (distance < collisionRadius)
//                 {
//                     Vector3 collisionNormal = displacement / distance;
//                     Vector3 relativeVelocity = particle1.velocity - particle2.velocity;
//                     float velocityAlongNormal = Vector3.Dot(relativeVelocity, collisionNormal);

//                     if (velocityAlongNormal > 0) continue; // Particles are moving apart

//                     float restitutionCoefficient = 0.5f; // Adjust for elasticity
//                     float impulseMagnitude = -(1 + restitutionCoefficient) * velocityAlongNormal;
//                     impulseMagnitude /= (1 / particle1.mass + 1 / particle2.mass);

//                     Vector3 impulse = impulseMagnitude * collisionNormal;
//                     particle1.velocity += impulse / particle1.mass;
//                     particle2.velocity -= impulse / particle2.mass;
//                 }
//             }
//         }
//     }
// }







// // // using System.Collections.Generic;
// // // using UnityEngine;



// // // public class AirSimulation : MonoBehaviour
// // // {
// // //     public List<AirParticle> particles;
// // //     public float gravity = -9.81f;
// // //     public float gasConstant = 287.05f; // Specific gas constant for dry air (J/(kg·K))
// // //     public float referenceViscosity = 1.81e-5f; // Reference viscosity (Pa·s)
// // //     public float referenceTemperature = 293.15f; // Reference temperature (K)
// // //     public float sutherlandConstant = 110.4f; // Sutherland's constant (K)
// // //     public float ambientTemperature = 298.15f; // Ambient temperature (K)
// // //     public float ambientPressure = 101325f; // Ambient pressure (Pa)

// // //     void Start()
// // //     {
// // //         // Initialize particles
// // //     }

// // //     void Update()
// // //     {
// // // foreach (var particle in particles)
// // // {
// // //     // Calculate properties
// // //     particle.density = CalculateDensity(particle);
// // //     particle.pressure = CalculatePressure(particle);

// // //     // Calculate forces
// // //     Vector3 forces = CalculateForces(particle);

// // //     // Integrate motion equations
// // //     Integrate(particle, forces);
// // // }

// // // // Handle collisions
// // // HandleCollisions();
// // //     }

// // // float CalculateDensity(AirParticle particle)
// // // {
// // //     return ambientPressure / (gasConstant * ambientTemperature);
// // // }

// // // float CalculatePressure(AirParticle particle)
// // // {
// // //     return particle.density * gasConstant * ambientTemperature;
// // // }

// // // Vector3 CalculateForces(AirParticle particle)
// // // {
// // //     Vector3 pressureForce = Vector3.zero; // Placeholder for pressure force calculation
// // //     Vector3 viscousForce = Vector3.zero; // Placeholder for viscous force calculation
// // //     Vector3 gravityForce = new Vector3(0, gravity * particle.mass, 0);

// // //     return pressureForce + viscousForce + gravityForce;
// // // }

// // // void Integrate(AirParticle particle, Vector3 forces)
// // // {
// // //     // Semi-implicit Euler integration
// // //     particle.velocity += forces / particle.mass * Time.deltaTime;
// // //     particle.position += particle.velocity * Time.deltaTime;
// // // }

// // // void HandleCollisions()
// // // {
// // //     // Simple collision detection and response
// // //     for (int i = 0; i < particles.Count; i++)
// // //     {
// // //         for (int j = i + 1; j < particles.Count; j++)
// // //         {
// // //             AirParticle particle1 = particles[i];
// // //             AirParticle particle2 = particles[j];
// // //             Vector3 displacement = particle1.position - particle2.position;
// // //             float distance = displacement.magnitude;
// // //             float collisionRadius = 0.1f; // Example collision radius

// // //             if (distance < collisionRadius)
// // //             {
// // //                 Vector3 collisionNormal = displacement / distance;
// // //                 Vector3 relativeVelocity = particle1.velocity - particle2.velocity;
// // //                 float velocityAlongNormal = Vector3.Dot(relativeVelocity, collisionNormal);

// // //                 if (velocityAlongNormal > 0) continue; // Particles are moving apart

// // //                 float restitutionCoefficient = 0.5f; // Adjust for elasticity
// // //                 float impulseMagnitude = -(1 + restitutionCoefficient) * velocityAlongNormal;
// // //                 impulseMagnitude /= (1 / particle1.mass + 1 / particle2.mass);

// // //                 Vector3 impulse = impulseMagnitude * collisionNormal;
// // //                 particle1.velocity += impulse / particle1.mass;
// // //                 particle2.velocity -= impulse / particle2.mass;
// // //             }
// // //         }
// // //     }
// // // }
// // // }