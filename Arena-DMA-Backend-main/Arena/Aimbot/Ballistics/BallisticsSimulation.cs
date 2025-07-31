using static arena_dma_backend.Arena.AimbotPrewarmer;

namespace arena_dma_backend.Ballistics
{
    public static class BallisticsSimulation
    {
        // Maximum simulation iterations (in-game max is 1300 or 13 seconds of travel time)
        private const int _maxIterations = 1300;
        // The time step between each simulation iteration
        private const float _simTimeStep = 0.01f;
        // The target distance tolerance from the lerped Z component.
        private const float _optimalLerpTolerance = 0.001f;
        // Fake Unity Physics.gravity
        private static readonly Vector3 _gravity = new(0f, -9.81f, 0f);
        // Fake Unity Vector3.forward
        private static readonly Vector3 _forwardVector = new(0f, 0f, 1f);
        // Cached trajectory calculation
        private static readonly TrajectoryDataPoint[] _trajectoryDataPoints = new TrajectoryDataPoint[_maxIterations];
        // Ballistic data cache
        private static DataCache _cache = new();
        // The last ballistic context
        private static BallisticContext _lastContext = new();

        public static BallisticSimulationOutput Run(Vector3 startPos, Vector3 endPos)
        {
            BallisticContext context = Context;

            // Only re-simulate when the bullet properties change
            if (_lastContext.IsDifferent(context))
            {
                float BW1 = context.Mass * 2f;
                float BDW1 = context.Mass * 0.0014223f / (context.Diameter * context.Diameter * context.Coefficient);
                float BDW2 = context.Diameter * context.Diameter * 3.1415927f / 4f;
                float BDW3 = 1.2f * BDW2;
                float simVelocity = context.Velocity;

                _cache = new(BW1, BDW1, BDW2, BDW3, simVelocity);

                DoSimulation(context);

                _lastContext = context;
            }

            // Find the closest data point to the target
            float targetDistance = Vector3.Distance(startPos, endPos);
            int optimalIndex = 0;

            for (int i = 0; i < _trajectoryDataPoints.Length; i++)
            {
                var dp = _trajectoryDataPoints[i];

                float distance = Vector3.Distance(dp.Position, Vector3.Zero);

                // Exit if we passed the target
                if (distance > targetDistance)
                    break;

                optimalIndex = i;
            }

            // Get the optimal lerp factor between the nearest and next data point
            var nearestDP = _trajectoryDataPoints[optimalIndex];
            var nextDP = _trajectoryDataPoints[optimalIndex + 1];

            float lf = FindOptimalLerp(nearestDP.Position, nextDP.Position, targetDistance);

            float bulletDrop = Vector3.Lerp(nearestDP.Position, nextDP.Position, lf).Y * -1;
            float travelTime = float.Lerp(nearestDP.Time, nextDP.Time, lf);

            return new BallisticSimulationOutput(bulletDrop, travelTime);
        }

        private static void DoSimulation(BallisticContext context)
        {
            Vector3 startVelocity = _forwardVector * context.Velocity;

            _trajectoryDataPoints[0] = new(0, 0f, Vector3.Zero, startVelocity);

            for (int i = 1; i < _maxIterations; i++)
            {
                var lastData = _trajectoryDataPoints[i - 1];

                Vector3 lastVelocity = lastData.Velocity;
                Vector3 lastPosition = lastData.Position;
                float lastMagnitude = lastVelocity.Length();

                float dragCoefficient = G1.CalculateDragCoefficient(lastMagnitude) * _cache.BDW1;

                // Create position offset
                Vector3 translationOffset = _gravity + _cache.BDW3 * -dragCoefficient * lastMagnitude * lastMagnitude / _cache.BW1 * Vector3.Normalize(lastVelocity);

                // Set current position
                Vector3 currentPosition = lastPosition + lastVelocity * _simTimeStep + 5E-05f * translationOffset;
                Vector3 currentVelocity = lastVelocity + translationOffset * _simTimeStep;

                _trajectoryDataPoints[i] = new(i, lastData.Time + _simTimeStep, currentPosition, currentVelocity);
            }

            Logger.WriteLine("[BALLISTICS SIMULATION] Cache updated!");
        }

        private static float FindOptimalLerp(Vector3 posBefore, Vector3 posAfter, float shotDistance)
        {
            float lerpMin = 0f;
            float lerpMax = 1f;
            float lerpMid = 0.5f;

            while (lerpMax - lerpMin > _optimalLerpTolerance)
            {
                Vector3 lerped = Vector3.Lerp(posBefore, posAfter, lerpMid);

                if (lerped.Z < shotDistance)
                    lerpMin = lerpMid;
                else
                    lerpMax = lerpMid;

                lerpMid = (lerpMin + lerpMax) / 2f;
            }

            return lerpMid;
        }

        private readonly struct DataCache(float bw1, float bdw1, float bdw2, float bdw3, float simVelocity)
        {
            public readonly float BW1 = bw1;
            public readonly float BDW1 = bdw1;
            public readonly float BDW2 = bdw2;
            public readonly float BDW3 = bdw3;
            public readonly float SimVelocity = simVelocity;
        }
    }
}