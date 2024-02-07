namespace Carly.TeabagGame.Prototype
{
    public struct SpringModel
    {
        public float Height { get; private set; }
        public float TargetHeight { get; private set; }
        public float Velocity { get; private set; }
        private float Resistance { get; set; }

        public SpringModel(float height, float targetHeight)
        {
            Height = height;
            TargetHeight = targetHeight;
            Velocity = 0;
            Resistance = 40;
        }

        public void UpdateSpring(float stifness, float dampening)
        {
            var x = Height - TargetHeight;
            var loss = -dampening * Velocity;

            var force = -stifness * x + loss;
            Velocity += force;

            Height += Velocity;
        }

        public void ApplyVelocity(float velocity)
        {
            Velocity += velocity / Resistance;
        }
    }
}