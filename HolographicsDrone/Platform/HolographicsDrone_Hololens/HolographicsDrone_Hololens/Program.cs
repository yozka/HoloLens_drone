using System;
using Windows.ApplicationModel.Core;

using Urho;
using Urho.Actions;
using Urho.SharpReality;
using Urho.Shapes;
using Urho.Resources;
using Urho.Physics;


using Windows.Media.Capture;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;



using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Urho;
using Urho.Actions;
using Urho.SharpReality;
using Urho.Resources;
using Urho.Shapes;
using Urho.Urho2D;

namespace HolographicDrone
{
    using HolographicsDrone.Drone;

    /// <summary>
    /// Windows Holographic application using SharpDX.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        [MTAThread]
        private static void Main()
        {
            var appViewSource = new UrhoAppViewSource<HelloWorldApplication>(new ApplicationOptions("Data"));
            appViewSource.UrhoAppViewCreated += OnViewCreated;
            CoreApplication.Run(appViewSource);
        }


        static void OnViewCreated(UrhoAppView view)
        {
            view.WindowIsSet += View_WindowIsSet;
        }

        static void View_WindowIsSet(Windows.UI.Core.CoreWindow coreWindow)
        {
            // you can subscribe to CoreWindow events here
        }
    }





    //---------------------------


    public class HelloWorldApplication : StereoApplication
    {
        Node mDrone;
        Material spatialMaterial;
        Node environmentNode;

        public HelloWorldApplication(ApplicationOptions opts) : base(opts) { }

        protected override async void Start()
        {
            // Create a basic scene, see StereoApplication
            base.Start();

            environmentNode = Scene.CreateChild();

            // Enable input
            //EnableGestureManipulation = true;
            //EnableGestureTapped = true;

            // Scene has a lot of pre-configured components, such as Cameras (eyes), Lights, etc.
            //DirectionalLight.Brightness = 1f;
            //DirectionalLight.Node.SetDirection(new Vector3(-1, 0, 0.5f));

            



            Node floorNode = Scene.CreateChild("Floor");
            floorNode.Position = new Vector3(0.0f, -1.0f, 0.0f);
            floorNode.Scale = new Vector3(2.0f, 0.2f, 2.0f);
            var floorObject = floorNode.CreateComponent<Box>();
            floorObject.Color = Color.White * 0.5f;

            floorNode.CreateComponent<RigidBody>();
            CollisionShape shape = floorNode.CreateComponent<CollisionShape>();
            shape.SetBox(Vector3.One, Vector3.Zero, Quaternion.Identity);




            // Create a node for the Earth
            mDrone = Scene.CreateChild();
            mDrone.Position = new Vector3(0, 1, 0.0f); //1.5m away
            mDrone.SetScale(0.2f); //D=30cm

            mDrone.CreateComponent<ADrone>(); //модель дрона
            mDrone.CreateComponent<ADroneModel>(); //модель дрона
            mDrone.CreateComponent<AControlGamePad>(); //управление дроном через клаву


            var cache = ResourceCache;

            // Material for spatial surfaces
            //spatialMaterial = new Material();
            //spatialMaterial.SetTechnique(0, CoreAssets.Techniques.NoTextureUnlitVCol, 1, 1);

            //spatialMaterial = cache.GetMaterial("Materials/StoneTiled.xml");
            spatialMaterial = Material.FromColor(Color.Green * 0.5f);

            // make sure 'spatialMapping' capabilaty is enabled in the app manifest.
            var spatialMappingAllowed = await StartSpatialMapping(new Vector3(50, 50, 10), 100);


            int i = 0;
            //UI.roo
        }

        // For HL optical stabilization (optional)
        //public override Vector3 FocusWorldPoint => mDrone.WorldPosition;

        //Handle input:

        Vector3 earthPosBeforeManipulations;
        public override void OnGestureManipulationStarted()
        {
            earthPosBeforeManipulations = mDrone.Position;
        }
        public override void OnGestureManipulationUpdated(Vector3 relativeHandPosition)
        {
            //mDrone.Position = relativeHandPosition + earthPosBeforeManipulations;
        }
        public override void OnGestureTapped()
        {
            int i = 0;
        }
        public override void OnGestureDoubleTapped()
        {
            int i = 0;
        }


        protected override void OnUpdate(float timeStep)
        {
            /*
            if (!look)
            {
                CaptureAndShowResult();
            }*/

        }

        List<string> mm = new List<string>();


        public override void OnSurfaceAddedOrUpdated(SpatialMeshInfo surface, Model generatedModel)
        {
            bool isNew = false;
            StaticModel staticModel = null;
            Node node = environmentNode.GetChild(surface.SurfaceId, false);
            if (node != null)
            {
                isNew = false;
                staticModel = node.GetComponent<StaticModel>();
            }
            else
            {
                isNew = true;
                node = environmentNode.CreateChild(surface.SurfaceId);
                staticModel = node.CreateComponent<StaticModel>();
                mm.Add(surface.SurfaceId);
            }

            node.Position = surface.BoundsCenter;
            //node.Rotation = surface.BoundsRotation;
            staticModel.Model = generatedModel;

            if (isNew)
            {
                staticModel.SetMaterial(spatialMaterial);
                var rigidBody = node.CreateComponent<RigidBody>();
                rigidBody.RollingFriction = 0.5f;
                rigidBody.Friction = 0.5f;
                var collisionShape = node.CreateComponent<CollisionShape>();
                collisionShape.SetTriangleMesh(generatedModel, 0, Vector3.One, Vector3.Zero, Quaternion.Identity);
            }
            else
            {
                //Update Collision shape
                int i = 0;
            }
        }

    }



}