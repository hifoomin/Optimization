using KinematicCharacterController;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Optimization.KinematicCM
{
    public static class Phase2
    {
        private static void UpdatePhase2Baseder(Action<KinematicCharacterMotor, float> orig, KinematicCharacterMotor self, float deltaTime)
        {
            // Main.logger.LogError("updating phase 2");
            self.CharacterController.UpdateRotation(ref self._internalTransientRotation, deltaTime);
            self.TransientRotation = self._internalTransientRotation;
            if (self._moveRotationDirty)
            {
                self.TransientRotation = self._moveRotationTarget;
                self._moveRotationDirty = false;
            }
            if (self._solveMovementCollisions && self.InteractiveRigidbodyHandling)
            {
                if (self.InteractiveRigidbodyHandling && self.AttachedRigidbody)
                {
                    float radius = self.Capsule.radius;
                    RaycastHit raycastHit;
                    if (self.CharacterGroundSweep(self.TransientPosition + self.CharacterUp * radius, self.TransientRotation, -self.CharacterUp, radius, out raycastHit) && raycastHit.collider.attachedRigidbody == self.AttachedRigidbody && self.IsStableOnNormal(raycastHit.normal))
                    {
                        float num = radius - raycastHit.distance;
                        self.TransientPosition = self.TransientPosition + self.CharacterUp * num + self.CharacterUp * 0.001f;
                    }
                }

                // edited from here

                if (self.SafeMovement || self.InteractiveRigidbodyHandling)
                {
                    Vector3 vector = self._cachedWorldUp;
                    float num2 = 0f;
                    int num3 = 0;
                    bool flag = false;

                    object lockObject = new object(); // Create a lock object

                    while (num3 < 3 && !flag)
                    {
                        int num4 = self.CharacterCollisionsOverlap(self.TransientPosition, self.TransientRotation, self._internalProbedColliders, 0f);
                        if (num4 > 0)
                        {
                            Parallel.For(0, num4, (i, state) =>
                            {
                                if (!flag)
                                {
                                    Transform component = self._internalProbedColliders[i].GetComponent<Transform>();
                                    if (component != null && Physics.ComputePenetration(self.Capsule, self.TransientPosition, self.TransientRotation, self._internalProbedColliders[i], component.position, component.rotation, out vector, out num2))
                                    {
                                        HitStabilityReport hitStabilityReport = new HitStabilityReport
                                        {
                                            IsStable = self.IsStableOnNormal(vector)
                                        };
                                        vector = self.GetObstructionNormal(vector, hitStabilityReport);
                                        Vector3 vector2 = vector * (num2 + 0.001f);

                                        lock (lockObject) // mf changes transient pos
                                        {
                                            self.TransientPosition += vector2;

                                            if (self.InteractiveRigidbodyHandling)
                                            {
                                                Rigidbody attachedRigidbody = self._internalProbedColliders[i].attachedRigidbody;
                                                if (attachedRigidbody)
                                                {
                                                    PhysicsMover component2 = attachedRigidbody.GetComponent<PhysicsMover>();
                                                    if (component2 && (attachedRigidbody && (!attachedRigidbody.isKinematic || component2)))
                                                    {
                                                        HitStabilityReport hitStabilityReport2 = new HitStabilityReport
                                                        {
                                                            IsStable = self.IsStableOnNormal(vector)
                                                        };
                                                        if (hitStabilityReport2.IsStable)
                                                        {
                                                            self.LastMovementIterationFoundAnyGround = hitStabilityReport2.IsStable;
                                                        }
                                                        if (component2.Rigidbody && component2.Rigidbody != self.AttachedRigidbody)
                                                        {
                                                            Vector3 vector3 = self.TransientPosition + self.TransientRotation * self.CharacterTransformToCapsuleCenter;
                                                            Vector3 transientPosition = self.TransientPosition;
                                                            MeshCollider meshCollider = self._internalProbedColliders[i] as MeshCollider;
                                                            if (!meshCollider || meshCollider.convex)
                                                            {
                                                                Physics.ClosestPoint(vector3, self._internalProbedColliders[i], component.position, component.rotation);
                                                            }
                                                            self.StoreRigidbodyHit(component2.Rigidbody, self.Velocity, transientPosition, vector, hitStabilityReport2);
                                                        }
                                                    }
                                                }
                                            }

                                            if (self.OverlapsCount < self._overlaps.Length)
                                            {
                                                self._overlaps[self.OverlapsCount] = new OverlapResult(vector, self._internalProbedColliders[i]);
                                                self.OverlapsCount++;
                                                state.Break(); // Exit the loop
                                            }
                                            else
                                            {
                                                state.Break(); // Exit the loop
                                            }
                                        }
                                    }
                                }
                            });
                        }
                        else
                        {
                            flag = true;
                        }
                        num3++;
                    }
                }

                // to here

                self.CharacterController.UpdateVelocity(ref self._baseVelocity, deltaTime);
                if (self._baseVelocity.magnitude < 0.01f)
                {
                    self._baseVelocity = Vector3.zero;
                }
                if (self._baseVelocity.sqrMagnitude > 0f)
                {
                    if (self._solveMovementCollisions)
                    {
                        if (self.InternalCharacterMove(self._baseVelocity * deltaTime, deltaTime, out self._internalResultingMovementMagnitude, out self._internalResultingMovementDirection))
                        {
                            self._baseVelocity = self._internalResultingMovementDirection * self._internalResultingMovementMagnitude / deltaTime;
                        }
                        else
                        {
                            self._baseVelocity = Vector3.zero;
                        }
                    }
                    else
                    {
                        self.TransientPosition += self._baseVelocity * deltaTime;
                    }
                }
                if (self.InteractiveRigidbodyHandling)
                {
                    self.ProcessVelocityForRigidbodyHits(ref self._baseVelocity, deltaTime);
                }
                if (self.HasPlanarConstraint)
                {
                    self.TransientPosition = self.InitialSimulationPosition + Vector3.ProjectOnPlane(self.TransientPosition - self.InitialSimulationPosition, self.PlanarConstraintAxis.normalized);
                }

                // from here

                if (self.DetectDiscreteCollisions)
                {
                    int num5 = self.CharacterCollisionsOverlap(self.TransientPosition, self.TransientRotation, self._internalProbedColliders, 0.002f);

                    object lockObject = new object();

                    Parallel.For(0, num5, j =>
                    {
                        Collider collider = self._internalProbedColliders[j];

                        lock (lockObject)
                        {
                            self.CharacterController.OnDiscreteCollisionDetected(collider);
                        }
                    });
                }

                // to here

                self.CharacterController.AfterCharacterUpdate(deltaTime);
            }
        }
    }
}