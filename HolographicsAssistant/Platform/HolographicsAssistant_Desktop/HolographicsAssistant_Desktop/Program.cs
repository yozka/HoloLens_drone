using System;
using Urho;
using Urho.Portable;
using Urho.Desktop;
using Urho.Gui;

using Urho.Actions;
using Urho.SharpReality;
using Urho.Shapes;
using Urho.Resources;

namespace HolographicsAssistant
{
    class AProgram
    {
        static void Main(string[] args)
        {
            //DesktopUrhoInitializer.AssetsDirectory = "Data";
            new AHelloWorld().Run();

        }







        public class AHelloWorld : Application
        {
            Node cameraNode;



            public AHelloWorld(ApplicationOptions options = null) 
                : 
                    base(options)
            {

            }

            protected override void Start()
            {
                Input.SetMouseVisible(true);
                //GetSubsystem<Urho3D::Input>()->SetMouseVisible(true);

                var cache = ResourceCache;
                var helloText = new Text();
                helloText.Value = "Hello world!!";

                helloText.SetColor(new Color(0f, 1f, 0f));
                helloText.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 30);


                helloText.SetPosition(100, 100);

                UI.Root.AddChild(helloText);


                //Graphics.SetWindowIcon(cache.GetImage("Textures/UrhoIcon.png"));
                Graphics.WindowTitle = "UrhoSharp Sample";



                var scene = new Scene();
                scene.CreateComponent<Octree>();


                //light
                /*
                var lightNode = scene.CreateChild("DirectionalLight");
                lightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f));
                var light = lightNode.CreateComponent<Light>();
                */

                var LightNode = scene.CreateChild("DirectionalLight");
                LightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f));
                var light = LightNode.CreateComponent<Light>();
                light.LightType = LightType.Directional;




                //camera
                cameraNode = scene.CreateChild("camera");
                var camera = cameraNode.CreateComponent<Camera>();
                cameraNode.Position = new Vector3(0, 0, -10);



                Renderer.SetViewport(0, new Viewport(Context, scene, camera, null));



                var node = scene.CreateChild("obj");
                var model = node.CreateComponent<Sphere>();
                model.Color = Color.Red;
                node.Position = new Vector3(0, 0, 0f); //1.5m away
                node.SetScale(2.0f); //D=30cm

            }






            protected override void OnUpdate(float timeStep)
            {
                Input input = Input;
                // Movement speed as world units per second
                const float moveSpeed = 4.0f;
                // Read WASD keys and move the camera scene node to the
                // corresponding direction if they are pressed
                if (input.GetKeyDown(Key.W))
                {
                    cameraNode.Translate(Vector3.UnitY * moveSpeed * timeStep, TransformSpace.Local);
                }
                if (input.GetKeyDown(Key.S))
                {
                    cameraNode.Translate(new Vector3(0.0f, -1.0f, 0.0f) * moveSpeed * timeStep, TransformSpace.Local);
                }
                if (input.GetKeyDown(Key.A))
                {
                    cameraNode.Translate(new Vector3(-1.0f, 0.0f, 0.0f) * moveSpeed * timeStep, TransformSpace.Local);
                }
                if (input.GetKeyDown(Key.D))
                {
                    cameraNode.Translate(Vector3.UnitX * moveSpeed * timeStep, TransformSpace.Local);
                }
            }



        }
    }
}