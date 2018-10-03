using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Shapes;
using Urho.Physics;

namespace HolographicsDrone.Drone
{
    ///-------------------------------------------------------------------
    using Hardware;
    ///-------------------------------------------------------------------









    ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Компанент, визуализация дрона, его модель
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class ADroneModel
            :
                Component
    {
        ///-------------------------------------------------------------------
        private readonly Dictionary<EMotor, Node>   mMotors = new Dictionary<EMotor, Node>();
        private readonly Dictionary<EMotor, float>  mSpeed  = new Dictionary<EMotor, float>(); //скорость пропеллеров
        private RigidBody mRigidBody = null; //физическая модель квадракоптера
        ///-------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ADroneModel()
        {
            ReceiveSceneUpdates = true;

        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Возвращаем физ модель
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public RigidBody rigidBody
        {
            get
            {
                return mRigidBody;
            }
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Возвращаем моторы
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public Node motor(EMotor type)
        {
            return mMotors[type];
        }
        ///--------------------------------------------------------------------



        
         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// устанавливаем скорость вращение винтов
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void setSpeedPropeller(EMotor type, float speed)
        {
            mSpeed[type] = speed;
        }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// инциализация
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public override void OnAttachedToNode(Node node)
        {
            base.OnAttachedToNode(node);
            createModel();
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// система создание модели
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void createModel()
        {
            //var main    = Node.CreateChild();
            Node main = Node;

            /*
            var mainBox = main.CreateComponent<Box>();
            mainBox.Color = Color.Blue;
            */
            
            //физика
            mRigidBody = main.CreateComponent<RigidBody>();
            mRigidBody.RollingFriction = 100.0f;
            mRigidBody.Friction = 100.0f;


            mRigidBody.Mass = 1.0f;
            

            CollisionShape shape = main.CreateComponent<CollisionShape>();
            shape.SetBox(new Vector3(1.0f, 0.5f, 1.0f), new Vector3(0.0f, -0.15f, 0.0f), Quaternion.Identity);


            //тело
            var nodeBody = main.CreateChild("body");
            var model = Application.Current.ResourceCache.GetModel("Models/drone.mdl");
            var bodyStaticModel = nodeBody.CreateComponent<StaticModel>();
            bodyStaticModel.Model = model;
  



            //винты
            const float shift = 0.444f;
            const float top = 0.05f;

    
            mMotors[EMotor.frontLeft]   = createMotor(new Vector3(-shift, top, shift), main, Color.Green);
            mMotors[EMotor.frontRight]  = createMotor(new Vector3( shift, top, shift), main, Color.Red);
            mMotors[EMotor.rearLeft]    = createMotor(new Vector3(-shift, top, -shift), main, Color.Yellow);
            mMotors[EMotor.rearRight]   = createMotor(new Vector3( shift, top, -shift), main, Color.Blue);


          
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// система создание модели
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private Node createMotor(Vector3 pos, Node parent, Color color)
        {
            var node = parent.CreateChild("MotorRotor");

            
            var rotor = node.CreateComponent<Box>();
            rotor.Color = color;

            node.Position = pos;
            node.Scale = new Vector3(0.05f, 0.01f, 0.6f); //Ширина, Высота, длина
  
            return node;
        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// 
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        protected override void OnUpdate(float timeStep)
        {
            foreach (var key in mSpeed.Keys)
            {
                float speed = mSpeed[key];
                var motor = mMotors[key];
                motor.Rotate(new Quaternion(0, speed * 20.0f * timeStep, 0));
            }


        }
        ///--------------------------------------------------------------------



    }
}
