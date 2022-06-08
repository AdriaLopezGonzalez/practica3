using System;
using SFML.Window;
using SFML.System;

namespace TCEngine
{
    public static class MathUtil
    {
        public static float DEG2RAD = (float)(Math.PI / 180.0f);
        public static float RAD2DEG = (float)(180.0f / Math.PI);

        /// <summary>
        /// Vector magnitude
        /// </summary>
        public static float Size(this Vector2f _vector)
        {
            return (float)Math.Sqrt(_vector.X * _vector.X + _vector.Y * _vector.Y);
        }

        /// <summary>
        /// Squared vector magnitude
        /// </summary>
        public static float SizeSquared(this Vector2f _vector)
        {
            return (_vector.X * _vector.X + _vector.Y * _vector.Y);
        }

        /// <summary>
        /// Returns the normalized vector (unit vector)
        /// </summary>
        public static Vector2f Normal(this Vector2f _vector)
        {
            Vector2f result = _vector;

            float size = _vector.Size();
            if (size > 0.0f)
            {
                result.X /= size;
                result.Y /= size;
            }

            return result;
        }

        /// <summary>
        /// Dot product in range [-1, 1]
        /// </summary>
        public static float Dot(Vector2f _lhs, Vector2f _rhs)
        {
            return _lhs.X * _rhs.X + _lhs.Y * _rhs.Y;
        }

        /// <summary>
        /// Returns the angle in degrees between _lhs and _rhs (unsigned)
        /// </summary>
        public static float Angle(Vector2f _lhs, Vector2f _rhs)
        {
            float dotProduct = Clamp(Dot(_lhs.Normal(), _rhs.Normal()), -1.0f, 1.0f);
            return (float)Math.Acos(dotProduct) * RAD2DEG;
        }

        /// <summary>
        /// Returns the angle in degrees between _lhs and _rhs (signed)
        /// </summary>
        public static float AngleWithSign(Vector2f _lhs, Vector2f _rhs)
        {
            return Angle(_lhs, _rhs) * Sign(_lhs, _rhs);
        }

        /// <summary>
        /// Returns the cross product -1 or 1 with normalize Vectors
        /// </summary>
        public static float Cross(Vector2f _lhs, Vector2f _rhs)
        {
            return (_lhs.X * _rhs.Y) - (_lhs.Y * _rhs.X);
        }

        // <summary>
        /// Returns the sign [-1 or 1] with normalize Vectors
        /// </summary>
        public static float Sign(Vector2f _lhs, Vector2f _rhs)
        {
            return (Cross(_lhs, _rhs) <= 0.0f) ? 1.0f : -1.0f;
        }

        /// <summary>
        /// Rotate v angle degrees
        /// </summary>
        public static Vector2f Rotate(this Vector2f _v, float _angle)
        {
            float sin0 = (float)Math.Sin(_angle * DEG2RAD);
            float cos0 = (float)Math.Cos(_angle * DEG2RAD);

            if (cos0 * cos0 < 0.001f * 0.001f)
                cos0 = 0.0f;

            Vector2f result = new Vector2f();
            result.X = cos0 * _v.X - sin0 * _v.Y;
            result.Y = sin0 * _v.X + cos0 * _v.Y;
            return result;
        }

        public static float Clamp(float _value, float _max, float _min)
        {
            return (float)Math.Max(Math.Min(_value, _min), _max);
        }

        /// <summary>
        /// Linear interpolation
        /// </summary>
        /// <param name="_source">Initial pos</param>
        /// <param name="_target">Target pos</param>
        /// <param name="_delta">Percentage in range [0.0,1.0]</param>
        public static Vector2f Lerp(Vector2f _source, Vector2f _target, float _delta)
        {
            _delta = Clamp(_delta, 0.0f, 1.0f);
            return _target * _delta + _source * (1.0f - _delta);
        }

        /// <summary>
        /// Linear interpolation
        /// </summary>
        /// <param name="_source">Initial value</param>
        /// <param name="_target">Target value</param>
        /// <param name="_delta">Percentage in range [0.0,1.0]</param>
        public static float Lerp(float _source, float _target, float _delta)
        {
            _delta = Clamp(_delta, 0.0f, 1.0f);
            return _target * _delta + _source * (1.0f - _delta);
        }
    }
}
