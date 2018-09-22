using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Shapes;
using Urho.Physics;

namespace HolographicsDrone.Drone
{
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
                StaticModelGroup
    {
        ///-------------------------------------------------------------------
        private RigidBody test;
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
            // createModel();
            ReceiveSceneUpdates = true;
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
            var nodeBody = Node.CreateChild();
            var body = nodeBody.CreateComponent<Box>();
            body.Color = Color.Blue;
            nodeBody.Scale = new Vector3(0.3f, 0.1f, 0.3f); //Ширина, Высота, длина
            AddInstanceNode(nodeBody);

            RigidBody rigidMain = nodeBody.CreateComponent<RigidBody>();
            rigidMain.Mass = 1.15f;
            rigidMain.Friction = 0.75f;
            CollisionShape shape = nodeBody.CreateComponent<CollisionShape>();
            shape.SetBox(Vector3.One, Vector3.Zero, Quaternion.Identity);


            //винты
            const float shift = 1.31f;
            const float top = 0.05f;
            var r1 = createRototor(new Vector3(shift,    top, shift), nodeBody);
            createRototor(new Vector3(-shift,   top, shift), nodeBody);
            var r3 = createRototor(new Vector3(shift,    top, -shift), nodeBody);




            var r4 = createRototor(new Vector3(-shift,   top, -shift), nodeBody);


            test = rigidMain;
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
        private Node createRototor(Vector3 pos, Node parent)
        {
            var node = parent.CreateChild();
            var rotor = node.CreateComponent<Box>();
            rotor.Color = Color.Green;

            node.Position = pos;
            //node.Scale = new Vector3(0.3f, 0.05f, 0.3f); //Ширина, Высота, длина

            AddInstanceNode(node);
            return node;
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
            //test.ApplyImpulse(new Vector3(0.1f * timeStep, 1.4f * timeStep, 0));
            //test.ApplyTorqueImpulse(new Vector3(0, 100.6f * timeStep,0));

            test.ApplyForce(new Vector3(0.0f, 12.0f, 0.1f), new Vector3(0.0f, 1.0f, 0.0f));

            
        }
        ///--------------------------------------------------------------------




    }
}
