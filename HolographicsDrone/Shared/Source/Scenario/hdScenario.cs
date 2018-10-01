﻿using System;
using System.Collections.Generic;
using System.Text;
using Urho;

namespace HolographicsDrone.Scenario
{
    ///-------------------------------------------------------------------
    using HolographicsDrone.Drone;
    using Utils;
    ///-------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Сценарий взаимодействия квадракоптера
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AScenario
            :
                Component
    {
        ///-------------------------------------------------------------------
        private Node mDrone = null;



        ///-------------------------------------------------------------------
        ///-------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AScenario()
        {
            ReceiveSceneUpdates = true;
        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Обработка
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        protected override void OnUpdate(float timeStep)
        {
            

        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// создание и получение квадрика
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void createDrone()
        {
            if (mDrone != null)
            {
                return;
            }

            mDrone = Scene.CreateChild("drone");
            mDrone.SetScale(0.2f); //D=30cm

            mDrone.CreateComponent<ADrone>(); //модель дрона
            mDrone.CreateComponent<ADroneModel>(); //модель дрона
            mDrone.CreateComponent<AControlBluetoothJoy>(); //управление дроном через клаву
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// переход в домашнию точку
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void home()
        {
            createDrone();
            mDrone.Position = new Vector3(0.0f, 1.0f, 0.0f);

        }
        ///--------------------------------------------------------------------




        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// присоеденям дрона к управлению
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void attachDrone()
        {
            
        }
        ///--------------------------------------------------------------------




    }
}