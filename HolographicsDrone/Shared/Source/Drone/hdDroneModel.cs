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
        private readonly Dictionary<EMotor, Node>    mMotors = new Dictionary<EMotor, Node>();
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

            //var mainBox = main.CreateComponent<Box>();
            //mainBox.Color = Color.Blue * 0.3f;
          
            
            //физика
            mRigidBody = main.CreateComponent<RigidBody>();
            

            //mRigidBody.Mass = 0.0f;
            //mRigidBody.Friction = 1.0f;

            mRigidBody.Mass = 1.0f;
            //mRigidBody.Kinematic = false;
            //mRigidBody.SetLinearFactor(new Vector3(0, 1.0f, 0));
            //mRigidBody.SetAngularFactor(Vector3.Up);

            CollisionShape shape = main.CreateComponent<CollisionShape>();
            shape.SetBox(new Vector3(1.0f, 0.1f, 1.0f), Vector3.Zero, Quaternion.Identity);


            //тело
            var nodeBody = main.CreateChild("body");
            var body = nodeBody.CreateComponent<Box>();
            body.Color = Color.Blue;
            nodeBody.Scale = new Vector3(0.4f, 0.1f, 0.4f); //Ширина, Высота, длина

   



            //винты
            const float shift = 0.5f;
            const float top = 0.5f;

            /*
            mMotors[EMotor.frontLeft]   = createMotor(new Vector3(-shift,  top,  shift),    main, Color.Green);
            mMotors[EMotor.frontRight]  = createMotor(new Vector3( shift,  top,  shift),    main, Color.Red);
            mMotors[EMotor.rearLeft]    = createMotor(new Vector3(-shift,  top, -shift),    main, Color.Yellow);
            mMotors[EMotor.rearRight]   = createMotor(new Vector3( shift,  top, -shift),    main, Color.Blue);

            */
            mMotors[EMotor.frontLeft]   = createMotor(new Vector3(-shift, top, shift), main, Color.Green);
            mMotors[EMotor.frontRight]  = createMotor(new Vector3( shift, top, shift), main, Color.Red);
            mMotors[EMotor.rearLeft]    = createMotor(new Vector3(-shift, top, -shift), main, Color.Yellow);
            mMotors[EMotor.rearRight]   = createMotor(new Vector3( shift, top, -shift), main, Color.Blue);


            /*
                        RigidBody rb = r4.CreateComponent<RigidBody>();
                        rb.Mass = 1.0f;
                        rb.Friction = 0.75f;
                        CollisionShape shape = r4.CreateComponent<CollisionShape>();
                        shape.SetBox(Vector3.One, Vector3.Zero, Quaternion.Identity);


                        var c4 = nodeBody.CreateComponent<Constraint>();
                        c4.OtherBody = rb;
                        c4.Position = r4.Position;
                        c4.ConstraintType = ConstraintType.Slider;
                        test = rb;
                        */

            /*
            rb = r1.CreateComponent<RigidBody>();
            rb.Mass = 1.0f;
            rb.Friction = 0.75f;
            shape = r1.CreateComponent<CollisionShape>();
            shape.SetBox(Vector3.One, Vector3.Zero, Quaternion.Identity);

            var c1 = nodeBody.CreateComponent<Constraint>();
            c1.OtherBody = rb;
            c1.Position = r1.Position;
            */

            //var cache = Application.ResourceCache;
            //var nodeBody = Node.CreateChild();
            //var modelBody = nodeBody.CreateComponent<StaticModel>();
            //modelBody.Model = cache.GetModel("Models/Mutant.mdl");



            //Model = cache.GetModel("Models/Mutant.mdl");
            //SetMaterial(cache.GetMaterial("robot.xml"));
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
            

            rotor.Color = color * 0.8f;

            node.Position = pos;
            node.Scale = new Vector3(0.4f, 0.05f, 0.4f); //Ширина, Высота, длина
  
            return node;
        }
        ///--------------------------------------------------------------------











    }
}
