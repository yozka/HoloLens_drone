using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace HolographicsDrone.Drone
{
    ///-------------------------------------------------------------------




     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Управлением дроном через клавиатуру
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AControlKeyboard
            :
                Component
    {
        ///-------------------------------------------------------------------

        ///-------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AControlKeyboard()
        {
            ReceiveSceneUpdates = true;
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Обработка кнопок
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        protected override void OnUpdate(float timeStep)
        {
            Input input = Application.Input;
            const float moveSpeed = 4.0f;


            if (input.GetKeyDown(Key.W))
            {
                //cameraNode.Translate(Vector3.UnitY * moveSpeed * timeStep, TransformSpace.Local);
            }
            if (input.GetKeyDown(Key.S))
            {
                //cameraNode.Translate(new Vector3(0.0f, -1.0f, 0.0f) * moveSpeed * timeStep, TransformSpace.Local);
            }
            if (input.GetKeyDown(Key.A))
            {
                //cameraNode.Translate(new Vector3(-1.0f, 0.0f, 0.0f) * moveSpeed * timeStep, TransformSpace.Local);
            }
            if (input.GetKeyDown(Key.D))
            {
                //cameraNode.Translate(Vector3.UnitX * moveSpeed * timeStep, TransformSpace.Local);
            }
        }
        ///--------------------------------------------------------------------





    }
}
