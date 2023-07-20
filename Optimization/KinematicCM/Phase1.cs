using KinematicCharacterController;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Optimization.KinematicCM
{
    public static class Phase1
    {
        private static void UpdatePhase1Baseder(Action<KinematicCharacterMotor, float> orig, KinematicCharacterMotor self, float deltaTime)
        {
            if (float.IsNaN(self._baseVelocity.x) || float.IsNaN(self._baseVelocity.y) || float.IsNaN(self._baseVelocity.z))
            {
                self._baseVelocity = Vector3.zero;
            }
            if (float.IsNaN(self._attachedRigidbodyVelocity.x) || float.IsNaN(self._attachedRigidbodyVelocity.y) || float.IsNaN(self._attachedRigidbodyVelocity.z))
            {
                self._attachedRigidbodyVelocity = Vector3.zero;
            }
            if (self._baseVelocity.x > 1000f || self._baseVelocity.y > 1000f || self._baseVelocity.z > 1000f)
            {
                // Main.logger.LogError("base velocity too high, setting to 0");
                self._baseVelocity = Vector3.zero;
            }

            self.CharacterController.BeforeCharacterUpdate(deltaTime);
            self.TransientPosition = self.Transform.position;
            self.TransientRotation = self.Transform.rotation;
            self.InitialSimulationPosition = self.TransientPosition;
            self.InitialSimulationRotation = self.TransientRotation;
            self._rigidbodyProjectionHitCount = 0;
            self.OverlapsCount = 0;
            self._lastSolvedOverlapNormalDirty = false;
            if (self._movePositionDirty)
            {
                if (self._solveMovementCollisions)
                {
                    if (self.InternalCharacterMove(self._movePositionTarget - self.TransientPosition, deltaTime, out self._internalResultingMovementMagnitude, out self._internalResultingMovementDirection) && self.InteractiveRigidbodyHandling)
                    {
                        Vector3 zero = Vector3.zero;
                        self.ProcessVelocityForRigidbodyHits(ref zero, deltaTime);
                    }
                }
                else
                {
                    self.TransientPosition = self._movePositionTarget;
                }
                self._movePositionDirty = false;
            }
            self.LastGroundingStatus.CopyFrom(self.GroundingStatus);
            self.GroundingStatus = default(CharacterGroundingReport);
            self.GroundingStatus.GroundNormal = self.CharacterUp;

            // edited from here

            if (self._solveMovementCollisions)
            {
                Vector3 vector = self._cachedWorldUp;
                float num = 0f;
                int num2 = 0;
                bool flag = false;

                object lockObject = new();

                Parallel.For(0, 3, (i, state) =>
                {
                    if (!flag)
                    {
                        int num3 = self.CharacterCollisionsOverlap(self.TransientPosition, self.TransientRotation, self._internalProbedColliders, 0f);
                        if (num3 > 0)
                        {
                            for (int j = 0; j < num3; j++)
                            {
                                if (self._internalProbedColliders[j] != null)
                                {
                                    Rigidbody attachedRigidbody = self._internalProbedColliders[j].attachedRigidbody;
                                    if (!attachedRigidbody || attachedRigidbody.isKinematic && !attachedRigidbody.GetComponent<PhysicsMover>())
                                    {
                                        Transform component = self._internalProbedColliders[j].GetComponent<Transform>();
                                        if (component != null && Physics.ComputePenetration(self.Capsule, self.TransientPosition, self.TransientRotation, self._internalProbedColliders[j], component.position, component.rotation, out vector, out num))
                                        {
                                            HitStabilityReport hitStabilityReport = new HitStabilityReport
                                            {
                                                IsStable = self.IsStableOnNormal(vector)
                                            };
                                            vector = self.GetObstructionNormal(vector, hitStabilityReport);
                                            Vector3 vector2 = vector * (num + 0.001f);

                                            // lock (lockObject) // stupid mf
                                            {
                                                self.TransientPosition += vector2;
                                                if (self.OverlapsCount < self._overlaps.Length)
                                                {
                                                    self._overlaps[self.OverlapsCount] = new OverlapResult(vector, self._internalProbedColliders[j]);
                                                    self.OverlapsCount++;
                                                    state.Break();
                                                }
                                                else
                                                {
                                                    state.Break();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                });
            }

            // to here

            if (self._solveGrounding)
            {
                if (self.MustUnground)
                {
                    self.TransientPosition += self.CharacterUp * 0.0075f;
                }
                else
                {
                    float num4 = 0.005f;
                    if (!self.LastGroundingStatus.SnappingPrevented && (self.LastGroundingStatus.IsStableOnGround || self.LastMovementIterationFoundAnyGround))
                    {
                        if (self.StepHandling != StepHandlingMethod.None)
                        {
                            num4 = Mathf.Max(self.CapsuleRadius, self.MaxStepHeight);
                        }
                        else
                        {
                            num4 = self.CapsuleRadius;
                        }
                        num4 += self.GroundDetectionExtraDistance;
                    }
                    self.ProbeGround(ref self._internalTransientPosition, self.TransientRotation, num4, ref self.GroundingStatus);
                }
            }
            self.LastMovementIterationFoundAnyGround = false;
            self.MustUnground = false;
            if (self._solveGrounding)
            {
                self.CharacterController.PostGroundingUpdate(deltaTime);
            }
            if (self.InteractiveRigidbodyHandling)
            {
                self._lastAttachedRigidbody = self.AttachedRigidbody;
                if (self.AttachedRigidbodyOverride)
                {
                    self.AttachedRigidbody = self.AttachedRigidbodyOverride;
                }
                else if (self.GroundingStatus.IsStableOnGround && self.GroundingStatus.GroundCollider.attachedRigidbody)
                {
                    Rigidbody interactiveRigidbody = self.GetInteractiveRigidbody(self.GroundingStatus.GroundCollider);
                    if (interactiveRigidbody)
                    {
                        self.AttachedRigidbody = interactiveRigidbody;
                    }
                }
                else
                {
                    self.AttachedRigidbody = null;
                }
                Vector3 vector3 = Vector3.zero;
                if (self.AttachedRigidbody)
                {
                    vector3 = self.GetVelocityFromRigidbodyMovement(self.AttachedRigidbody, self.TransientPosition, deltaTime);
                }
                if (self.PreserveAttachedRigidbodyMomentum && self._lastAttachedRigidbody != null && self.AttachedRigidbody != self._lastAttachedRigidbody)
                {
                    self._baseVelocity += self._attachedRigidbodyVelocity;
                    self._baseVelocity -= vector3;
                }
                self._attachedRigidbodyVelocity = self._cachedZeroVector;
                if (self.AttachedRigidbody)
                {
                    self._attachedRigidbodyVelocity = vector3;
                    Vector3 normalized = Vector3.ProjectOnPlane(Quaternion.Euler(57.29578f * self.AttachedRigidbody.angularVelocity * deltaTime) * self.CharacterForward, self.CharacterUp).normalized;
                    self.TransientRotation = Quaternion.LookRotation(normalized, self.CharacterUp);
                }
                if (self.GroundingStatus.GroundCollider && self.GroundingStatus.GroundCollider.attachedRigidbody && self.GroundingStatus.GroundCollider.attachedRigidbody == self.AttachedRigidbody && self.AttachedRigidbody != null && self._lastAttachedRigidbody == null)
                {
                    self._baseVelocity -= Vector3.ProjectOnPlane(self._attachedRigidbodyVelocity, self.CharacterUp);
                }
                if (self._attachedRigidbodyVelocity.sqrMagnitude > 0f)
                {
                    self._isMovingFromAttachedRigidbody = true;
                    if (self._solveMovementCollisions)
                    {
                        if (self.InternalCharacterMove(self._attachedRigidbodyVelocity * deltaTime, deltaTime, out self._internalResultingMovementMagnitude, out self._internalResultingMovementDirection))
                        {
                            self._attachedRigidbodyVelocity = self._internalResultingMovementDirection * self._internalResultingMovementMagnitude / deltaTime;
                        }
                        else
                        {
                            self._attachedRigidbodyVelocity = Vector3.zero;
                        }
                    }
                    else
                    {
                        self.TransientPosition += self._attachedRigidbodyVelocity * deltaTime;
                    }
                    self._isMovingFromAttachedRigidbody = false;
                }
            }
        }
    }
}