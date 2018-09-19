using System;
using Windows.ApplicationModel.Core;

using Urho;
using Urho.Actions;
using Urho.SharpReality;
using Urho.Shapes;
using Urho.Resources;

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
        Node earthNode;
        MediaCapture mediaCapture;
        Sphere earth;

        bool look = false;
        public HelloWorldApplication(ApplicationOptions opts) : base(opts) { }

        protected override async void Start()
        {
            // Create a basic scene, see StereoApplication
            base.Start();

            // Enable input
            EnableGestureManipulation = true;
            EnableGestureTapped = true;

            // Create a node for the Earth
            earthNode = Scene.CreateChild();
            earthNode.Position = new Vector3(0, 0, 1.5f); //1.5m away
            earthNode.SetScale(0.3f); //D=30cm

            // Scene has a lot of pre-configured components, such as Cameras (eyes), Lights, etc.
            DirectionalLight.Brightness = 1f;
            DirectionalLight.Node.SetDirection(new Vector3(-1, 0, 0.5f));

            //Sphere is just a StaticModel component with Sphere.mdl as a Model.
            earth = earthNode.CreateComponent<Sphere>();
            earth.Material = Material.FromImage("Textures/Earth.jpg");

            var moonNode = earthNode.CreateChild();
            moonNode.SetScale(0.27f); //27% of the Earth's size
            moonNode.Position = new Vector3(1.2f, 0, 0);

            // Same as Sphere component:
            var moon = moonNode.CreateComponent<StaticModel>();
            moon.Model = CoreAssets.Models.Sphere;

            moon.Material = Material.FromImage("Textures/Moon.jpg");

            // Run a few actions to spin the Earth, the Moon and the clouds.
            earthNode.RunActions(new RepeatForever(new RotateBy(duration: 1f, deltaAngleX: 0, deltaAngleY: -4, deltaAngleZ: 0)));
           // await TextToSpeech("Привет мир");

            // More advanced samples can be found here:
            // https://github.com/xamarin/urho-samples/tree/master/HoloLens



            mediaCapture = new MediaCapture();
            await mediaCapture.InitializeAsync();
            //await mediaCapture.AddVideoEffectAsync(new MrcVideoEffectDefinition(), MediaStreamType.Photo);
           // CaptureAndShowResult();
        }

        // For HL optical stabilization (optional)
        public override Vector3 FocusWorldPoint => earthNode.WorldPosition;

        //Handle input:

        Vector3 earthPosBeforeManipulations;
        public override void OnGestureManipulationStarted()
        {
            earthPosBeforeManipulations = earthNode.Position;
        }
        public override void OnGestureManipulationUpdated(Vector3 relativeHandPosition)
        {
            earthNode.Position = relativeHandPosition + earthPosBeforeManipulations;
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
            if (!look)
            {
                CaptureAndShowResult();
            }

        }






        async void CaptureAndShowResult()
        {
            look = true;
            var desc = await CaptureAndAnalyze();
        }






        async Task<int> CaptureAndAnalyze()
        {
            look = true;
            var imgFormat = ImageEncodingProperties.CreateJpeg();

            //NOTE: this is how you can save a frame to the CameraRoll folder:
            //var file = await KnownFolders.CameraRoll.CreateFileAsync($"MCS_Photo{DateTime.Now:HH-mm-ss}.jpg", CreationCollisionOption.GenerateUniqueName);
            //await mediaCapture.CapturePhotoToStorageFileAsync(imgFormat, file);
            //var stream = await file.OpenStreamForReadAsync();

            // Capture a frame and put it to MemoryStream
            var memoryStream = new MemoryStream();
            using (var ras = new InMemoryRandomAccessStream())
            {
                await mediaCapture.CapturePhotoToStreamAsync(imgFormat, ras);
                ras.Seek(0);
                using (var stream = ras.AsStreamForRead())
                    stream.CopyTo(memoryStream);
            }

            var imageBytes = memoryStream.ToArray();
            memoryStream.Position = 0;


            InvokeOnMain(() =>
            {
                var image = new Image();
                image.Load(new Urho.MemoryBuffer(imageBytes));
                /*
                Node child = Scene.CreateChild();
                child.Position = LeftCamera.Node.WorldPosition + LeftCamera.Node.WorldDirection * 2f;
                child.LookAt(LeftCamera.Node.WorldPosition, Vector3.Up, TransformSpace.World);

                child.Scale = new Vector3(1f, image.Height / (float)image.Width, 0.1f) / 10;
                var texture = new Texture2D();
                texture.SetData(image, true);

                var material = new Material();
                material.SetTechnique(0, CoreAssets.Techniques.Diff, 0, 0);
                material.SetTexture(TextureUnit.Diffuse, texture);

                var box = child.CreateComponent<Box>();
                box.SetMaterial(material);

                child.RunActions(new EaseBounceOut(new ScaleBy(1f, 5)));
                */

                var texture = new Texture2D();
                texture.SetData(image, true);
                var material = new Material();
                material.SetTechnique(0, CoreAssets.Techniques.Diff, 0, 0);
                material.SetTexture(TextureUnit.Diffuse, texture);

                earth.SetMaterial(material);
                look = false;

            });



            return 0;
        }


    }



}