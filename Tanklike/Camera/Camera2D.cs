using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanklike.Camera
{
    public class Camera2D
    {
        private float _zoom;
        private float _rotation;
        private Vector2 _position;
        private Matrix _transform = Matrix.Identity;
        private bool _isViewTransformationDirty = true;
        private Matrix _camTranslationMatrix = Matrix.Identity;
        private Matrix _camRotationMatrix = Matrix.Identity;
        private Matrix _camScaleMatrix = Matrix.Identity;
        private Matrix _resTranslationMatrix = Matrix.Identity;

        private Vector3 _camTranslationVector = Vector3.Zero;
        private Vector3 _camScaleVector = Vector3.Zero;
        private Vector3 _resTranslationVector = Vector3.Zero;

        protected ResolutionIndependentRenderer ResolutionIndependentRenderer { get; set; }

        /// <summary>
        /// Current position of the camera.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _isViewTransformationDirty = true;
            }
        }

        /// <summary>
        /// Current zoom level of the camera.
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < 0.1f)
                {
                    _zoom = 0.1f;
                }
                _isViewTransformationDirty = true;
            }
        }

        /// <summary>
        /// Current rotation angle of the camera.
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _isViewTransformationDirty = true;
            }
        }


        public Camera2D(ResolutionIndependentRenderer resolutionIndependence)
        {
            ResolutionIndependentRenderer = resolutionIndependence;

            _zoom = 0.1f;
            _rotation = 0.0f;
            _position = Vector2.Zero;
        }

        /// <summary>
        /// Moves the camera
        /// </summary>
        /// <param name="amount">Vector that determines the amount and direction of movement.</param>
        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        /// <summary>
        /// Sets the camera to a specific position.
        /// </summary>
        /// <param name="position">Vector that determines the new position of the camera.</param>
        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        /// <summary>
        /// Retrieves the current view transformation matrix for the camera.
        /// </summary>
        /// <returns></returns>
        public Matrix GetViewTransformationMatrix()
        {
            if (_isViewTransformationDirty)
            {
                _camTranslationVector.X = -_position.X;
                _camTranslationVector.Y = -_position.Y;

                Matrix.CreateTranslation(ref _camTranslationVector, out _camTranslationMatrix);
                Matrix.CreateRotationZ(_rotation, out _camRotationMatrix);

                _camScaleVector.X = _zoom;
                _camScaleVector.Y = _zoom;
                _camScaleVector.Z = 1;

                Matrix.CreateScale(ref _camScaleVector, out _camScaleMatrix);

                _resTranslationVector.X = ResolutionIndependentRenderer.VirtualWidth * 0.5f;
                _resTranslationVector.Y = ResolutionIndependentRenderer.VirtualHeight * 0.5f;
                _resTranslationVector.Z = 0;

                Matrix.CreateTranslation(ref _resTranslationVector, out _resTranslationMatrix);

                _transform = _camTranslationMatrix *
                             _camRotationMatrix *
                             _camScaleMatrix *
                             _resTranslationMatrix *
                             ResolutionIndependentRenderer.GetTransformationMatrix();

                _isViewTransformationDirty = false;
            }

            return _transform;
        }

        /// <summary>
        /// Sets the transformation matrices to be recalculated.
        /// </summary>
        public void RecalculateTransformationMatrices()
        {
            _isViewTransformationDirty = true;
        }


    }
}
