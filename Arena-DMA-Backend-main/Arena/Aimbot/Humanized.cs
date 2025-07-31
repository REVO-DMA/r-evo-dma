using arena_dma_backend.Ballistics;
using arena_dma_backend.Misc;

namespace arena_dma_backend.Arena
{
    public class Humanized
    {
        public static void Garbanzo()
        {
            //Vector2 newViewAngle = FormHardTrajectory(fireportPosition, bonePosition, targetVelocity, simulationResult);

            //// Safety checks
            //if (!float.IsNormal(newViewAngle.X) || !float.IsNormal(newViewAngle.Y) || Math.Abs(newViewAngle.X) > 360 || Math.Abs(newViewAngle.Y) > 90)
            //    throw new Exception(nameof(newViewAngle));

            //Vector2 currentViewAngle = NormalizeVector2(Memory.ReadValue<Vector2>(localPlayer.MovementContext + Offsets.MovementContext.ViewAngle, false));

            //float vaDist = Vector2.Distance(newViewAngle, currentViewAngle);

            //// Skip aimbot if we are within the deadzone
            //if (vaDist <= Program.UserConfig.Deadzone)
            //    return;

            //// A human's vertical aim is slower than their horizontal aim
            //float smoothingAmount = Program.UserConfig.SmoothingAmount;
            //Vector2 smoothFactor = new(smoothingAmount, smoothingAmount + smoothingAmount * 0.3f);

            //// Decrease smoothing the closer we are to the target
            //// This also makes the aimbot much less linear
            //if (vaDist < 0.15f) smoothFactor /= RandomFloatInRange(10f, 11f);
            //else if (vaDist < 0.2f) smoothFactor /= RandomFloatInRange(9f, 10f);
            //else if (vaDist < 0.25f) smoothFactor /= RandomFloatInRange(8f, 9f);
            //else if (vaDist < 0.3f) smoothFactor /= RandomFloatInRange(7f, 8f);
            //else if (vaDist < 0.5f) smoothFactor /= RandomFloatInRange(6f, 7f);
            //else if (vaDist < 1f) smoothFactor /= RandomFloatInRange(5f, 6f);
            //else if (vaDist < 1.5f) smoothFactor /= RandomFloatInRange(4.5f, 5f);
            //else if (vaDist < 2f) smoothFactor /= RandomFloatInRange(4f, 4.5f);
            //else if (vaDist < 2.5f) smoothFactor /= RandomFloatInRange(3.5f, 4f);
            //else if (vaDist < 3f) smoothFactor /= RandomFloatInRange(3f, 3.5f);
            //else if (vaDist < 3.5f) smoothFactor /= RandomFloatInRange(2.5f, 3f);
            //else if (vaDist < 4f) smoothFactor /= RandomFloatInRange(2f, 2.5f);
            //else if (vaDist < 4.5f) smoothFactor /= RandomFloatInRange(1.8f, 2f);
            //else if (vaDist < 5f) smoothFactor /= RandomFloatInRange(1.5f, 1.8f);
            //else if (vaDist < 6f) smoothFactor /= RandomFloatInRange(1.25f, 1.5f);

            //Vector2 smoothedAngle = NormalizeVector2(newViewAngle - currentViewAngle);
            //smoothedAngle = currentViewAngle + smoothedAngle / smoothFactor;

            //Memory.WriteValue(localPlayer.MovementContext + Offsets.MovementContext.ViewAngle, NormalizeVector2(smoothedAngle));
        }

        private static Vector2 FormHardTrajectory(Vector3 fireportPosition, Vector3 targetPosition, Vector3 targetVelocity, BallisticSimulationOutput? simulationResult)
        {
            if (simulationResult != null && simulationResult is BallisticSimulationOutput sim)
            {
                if (Math.Abs(targetVelocity.X) > 25f || Math.Abs(targetVelocity.Y) > 25f || Math.Abs(targetVelocity.Z) > 25f)
                    Logger.WriteLine("[AIMBOT] -> FormHardTrajectory(): Running without prediction.");
                else
                {
                    targetVelocity.X *= sim.TravelTime;
                    targetVelocity.Y *= sim.TravelTime;
                    targetVelocity.Z *= sim.TravelTime;
                    targetPosition.Y += targetVelocity.Y + sim.DropCompensation; // Add drop compensation

                    targetPosition.X += targetVelocity.X;
                    targetPosition.Z += targetVelocity.Z;
                }
            }

            Vector3 difference = fireportPosition - targetPosition;

            float length = difference.Length();

            Vector2 ret;

            ret.Y = (float)Math.Asin(difference.Y / length).ToDegrees(); // Pitch
            ret.X = (float)-Math.Atan2(difference.X, -difference.Z).ToDegrees(); // Yaw

            return NormalizeVector2(ret);
        }

        /// <summary>
        /// Normalizes the given Vector2.
        /// </summary>
        /// <param name="angles">The Vector2 to normalize.</param>
        /// <returns>The normalized Vector2.</returns>
        public static Vector2 NormalizeVector2(Vector2 angles)
        {
            angles.X = NormalizeAngle(angles.X, -180f, 180f);
            angles.Y = NormalizeAngle(angles.Y, -89f, 89f);
            return angles;
        }

        /// <summary>
        /// Normalizes the given angle.
        /// </summary>
        /// <param name="angle">The source angle.</param>
        /// <param name="minAngle">The smallest value the source angle can have.</param>
        /// <param name="maxAngle">The largest value the source angle can have.</param>
        /// <returns>The normalized angle.</returns>
        public static float NormalizeAngle(float angle, float minAngle, float maxAngle)
        {
            while (angle > maxAngle)
            {
                angle -= 360f;
            }
            while (angle < minAngle)
            {
                angle += 360f;
            }

            return Math.Clamp(angle, minAngle, maxAngle);
        }

        private static float RandomFloatInRange(float min, float max)
        {
            return Random.Shared.NextSingle() * (max - min) + min;
        }
    }
}
