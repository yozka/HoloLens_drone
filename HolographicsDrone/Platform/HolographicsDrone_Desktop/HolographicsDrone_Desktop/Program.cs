using System;
using Urho;
using Urho.Desktop;
using Urho.Physics;
using Urho.Shapes;

namespace HolographicsDrone
{
    using HolographicsDrone.Drone;
    using HolographicsDrone.GUI;


    class AProgram
    {
        static void Main(string[] args)
        {
            DesktopUrhoInitializer.AssetsDirectory = "Data";
            new AHolographicsDrone().Run();

        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Точка входа, инциализация для десктопа
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public class AHolographicsDrone : Application
        {
            ///--------------------------------------------------------------------
            private Scene mScene = null; //сцена игры
            private Node mCameraNode = null;
            ///--------------------------------------------------------------------






            ///-------------------------------------------------------------------
            ///
            /// <summary>
            /// Constructor
            /// </summary>
            ///
            ///--------------------------------------------------------------------
            public AHolographicsDrone(ApplicationOptions options = null) 
                : 
                    base(options)
            {
                
            }
            ///--------------------------------------------------------------------






             ///-------------------------------------------------------------------
            ///
            /// <summary>
            /// Start game
            /// </summary>
            ///
            ///--------------------------------------------------------------------
            protected override void Start()
            {
                Input.SetMouseVisible(true);
                Graphics.WindowTitle = "Drone";
                Graphics.SetMode(1024, 768);



                createScene();
                createLevel();


                /*
                var cache = ResourceCache;
                var helloText = new Text();
                helloText.Value = "Hello world!!";

                helloText.SetColor(new Color(0f, 1f, 0f));
                helloText.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 30);


                helloText.SetPosition(100, 100);

                UI.Root.AddChild(helloText);
                */



                Engine.SubscribeToPostRenderUpdate(args => { render(); });
  



                mScene.GetOrCreateComponent<PhysicsWorld>();
                mScene.GetOrCreateComponent<DebugRenderer>();

            }
            ///--------------------------------------------------------------------


            
            protected void render ()
            {
                // Default debug rendering.
                Renderer.DrawDebugGeometry(false);

                // Use debug renderer to output physics world debug.
                
                var debugRenderer = mScene.GetComponent<DebugRenderer>();
                var physicsComp = mScene.GetComponent<PhysicsWorld>();
                if (physicsComp != null)
                {
                    physicsComp.DrawDebugGeometry(debugRenderer, false);
                }

                //отрисуем точки

                var list = mScene.GetChildrenWithComponent<Urho.Shapes.Box>(true);
                foreach (var node in list)
                {
                    Vector3 thrust = node.WorldUp;
                    thrust.Normalize();
                    var pos = node.WorldPosition;

                    Vector3 start = pos;
                    Vector3 end = pos + thrust;
                    debugRenderer.AddLine(start, end, Color.White, false);
                    
                }



                /*
                var upperBound = new Vector3(-4.0f, 2.0f, 0.0f);
                var lowerBound = new Vector3(4.0f, -2.0f, 0.0f);
                debugRenderer.AddBoundingBox(
                    new BoundingBox(upperBound, lowerBound),
                    Color.White,
                    false);*/
            }



             ///-------------------------------------------------------------------
            ///
            /// <summary>
            /// Создание сцены
            /// </summary>
            ///
            ///--------------------------------------------------------------------
            private void createScene()
            {
                var cache = ResourceCache;
                mScene = new Scene();

                // Create octree, use default volume (-1000, -1000, -1000) to (1000, 1000, 1000)
                // Create a physics simulation world with default parameters, which will update at 60fps. Like the Octree must
                // exist before creating drawable components, the PhysicsWorld must exist before creating physics components.
                // Finally, create a DebugRenderer component so that we can draw physics debug geometry
                mScene.CreateComponent<Octree>();
                var physics = mScene.CreateComponent<PhysicsWorld>();
                //physics.SetGravity(Vector3.Zero);
               // mScene.CreateComponent<DebugRenderer>();

                // Create a Zone component for ambient lighting & fog control
                Node zoneNode = mScene.CreateChild("Zone");
                Zone zone = zoneNode.CreateComponent<Zone>();
                zone.SetBoundingBox(new BoundingBox(-1000.0f, 1000.0f));
                zone.AmbientColor = new Color(0.15f, 0.15f, 0.15f);
                zone.FogColor = new Color(1.0f, 1.0f, 1.0f);
                zone.FogStart = 300.0f;
                zone.FogEnd = 500.0f;

                // Create a directional light to the world. Enable cascaded shadows on it
                Node lightNode = mScene.CreateChild("DirectionalLight");
                lightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f));
                Light light = lightNode.CreateComponent<Light>();
                light.LightType = LightType.Directional;
                light.CastShadows = true;
                light.ShadowBias = new BiasParameters(0.00025f, 0.5f);
                // Set cascade splits at 10, 50 and 200 world units, fade shadows out at 80% of maximum shadow distance
                light.ShadowCascade = new CascadeParameters(10.0f, 50.0f, 200.0f, 0.0f, 0.8f);

                // Create skybox. The Skybox component is used like StaticModel, but it will be always located at the camera, giving the
                // illusion of the box planes being far away. Use just the ordinary Box model and a suitable material, whose shader will
                // generate the necessary 3D texture coordinates for cube mapping
                /*
                Node skyNode = mScene.CreateChild("Sky");
                skyNode.SetScale(500.0f); // The scale actually does not matter
                Skybox skybox = skyNode.CreateComponent<Skybox>();
                skybox.Model = cache.GetModel("Models/Box.mdl");
                skybox.SetMaterial(cache.GetMaterial("Materials/Skybox.xml"));*/

                {
                    // Create a floor object, 1000 x 1000 world units. Adjust position so that the ground is at zero Y
                    Node floorNode = mScene.CreateChild("Floor");
                    floorNode.Position = new Vector3(0.0f, -0.5f, 0.0f);
                    floorNode.Scale = new Vector3(1000.0f, 1.0f, 1000.0f);
                    var floorObject = floorNode.CreateComponent<Box>();
                    floorObject.Color = Color.Gray;
                    
                    //floorObject.Model = cache.GetModel("Models/Box.mdl");
                    //floorObject.SetMaterial(cache.GetMaterial("Materials/StoneTiled.xml"));

                    // Make the floor physical by adding RigidBody and CollisionShape components. The RigidBody's default
                    // parameters make the object static (zero mass.) Note that a CollisionShape by itself will not participate
                    // in the physics simulation

                    floorNode.CreateComponent<RigidBody>();
                    CollisionShape shape = floorNode.CreateComponent<CollisionShape>();
                    // Set a box shape of size 1 x 1 x 1 for collision. The shape will be scaled with the scene node scale, so the
                    // rendering and physics representation sizes should match (the box model is also 1 x 1 x 1.)
                    shape.SetBox(Vector3.One, Vector3.Zero, Quaternion.Identity);
                }

     
                {
                    // Create a pyramid of movable physics objects
                    for (int y = 0; y < 8; ++y)
                    {
                        for (int x = -y; x <= y; ++x)
                        {
                            Node boxNode = mScene.CreateChild("Box");
                            boxNode.Position = new Vector3((float)x, -(float)y + 8.0f, 0.0f);
                            var boxObject = boxNode.CreateComponent<Box>();
                            boxObject.Color = Color.Blue;
                            //boxObject.Model = cache.GetModel("Models/Box.mdl");
                            //boxObject.SetMaterial(cache.GetMaterial("Materials/StoneEnvMapSmall.xml"));
                            boxObject.CastShadows = true;

                            // Create RigidBody and CollisionShape components like above. Give the RigidBody mass to make it movable
                            // and also adjust friction. The actual mass is not important; only the mass ratios between colliding 
                            // objects are significant
                            RigidBody body = boxNode.CreateComponent<RigidBody>();
                            body.Mass = 0.06f;
                            body.Friction = 0.75f;
                            CollisionShape shape = boxNode.CreateComponent<CollisionShape>();
                            shape.SetBox(Vector3.One, Vector3.Zero, Quaternion.Identity);
                        }
                    }
                }

                // Create the camera. Limit far clip distance to match the fog. Note: now we actually create the camera node outside
                // the scene, because we want it to be unaffected by scene load / save
                mCameraNode = mScene.CreateChild();
                Camera camera = mCameraNode.CreateComponent<Camera>();
                camera.FarClip = 500.0f;

                // Set an initial position for the camera scene node above the floor
                mCameraNode.Position = (new Vector3(0.0f, 2.0f, -12.0f));

                mCameraNode.Position = (new Vector3(0.0f, 8.0f, -15.0f));
                mCameraNode.Rotation = new Quaternion(30, 0, 0);

                Renderer.SetViewport(0, new Viewport(Context, mScene, camera, null));


                mScene.GetOrCreateComponent<AMainHUD>();
            }
            ///--------------------------------------------------------------------






             ///-------------------------------------------------------------------
            ///
            /// <summary>
            /// Создание уровня
            /// </summary>
            ///
            ///--------------------------------------------------------------------
            private void createLevel()
            {
                var drone = mScene.CreateChild("drone");
                drone.CreateComponent<ADrone>(); //сам дрон, мозги
                drone.CreateComponent<ADroneModel>(); //модель дрона
                drone.CreateComponent<AControlGamePad>(); //управление дроном через клаву
                drone.CreateComponent<AMarkerAnhor>(); //якорь указывающий где находится дрон

                drone.Position = new Vector3(0, 5, -6.0f);
                drone.Rotation = new Quaternion(x: 0, y: 0, z: 0);


                var gui = new ADebugInformation(drone);
                gui.SetPosition(20, 20);
                UI.Root.AddChild(gui);



            }
            ///--------------------------------------------------------------------





           



        }
    }
}