//using System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Gui;
using Urho.Resources;
using Urho.Urho2D;

namespace WPF45URHO
{
    class GameApplication : Application
    {
        UrhoConsole console;
        DebugHud debugHud;
        ResourceCache cache;
        UI ui;
        Text textElement;
        Node mobileMushroom;
        bool mobileMushroomMove = false;
        float totalTime;
        int totalSec = 0;
        Scene scene;
        protected Node CameraNode { get; set; }

        protected MonoDebugHud MonoDebugHud { get; set; }
        [Preserve]
        public GameApplication(ApplicationOptions options = null) : base(options) { }
        static readonly Random random = new Random();
        public static float NextRandom(float range) { return (float)random.NextDouble() * range; }

        static GameApplication()
        {
            Urho.Application.UnhandledException += Application_UnhandledException;
        }
        static void Application_UnhandledException(object sender, Urho.UnhandledExceptionEventArgs e)
        {
            var ex = e.Exception;
            var t = ex.GetType();
            //if (Debugger.IsAttached)
            //    Debugger.Break();
            e.Handled = true;
        }

        protected override void Start()
        {
            Log.LogMessage += e => Debug.WriteLine($"[{e.Level}] {e.Message}");
            base.Start();
            if (Platform == Platforms.Android ||
                Platform == Platforms.iOS ||
                Options.TouchEmulation)
            {
                //InitTouchInput();
            }
            Input.Enabled = true;
            MonoDebugHud = new MonoDebugHud(this);
            MonoDebugHud.Show();

            cache = ResourceCache;
            CreateInfo();
            //SetWindowAndTitleIcon();
            CreateConsoleAndDebugHud();
            //Input.SubscribeToKeyDown(HandleKeyDown);
            Input.KeyDown += HandleKeyDown;

            CreateScene();
            //SimpleCreateInstructionsWithWasd(", use PageUp PageDown keys to zoom.");
            SetupViewport();

        }

        void CreateInfo()
        {
            ui = UI;
            var w = ui.Root.CreateWindow("rrr");
            w.SetAlignment(HorizontalAlignment.Left, VerticalAlignment.Bottom);
            w.SetSize(160, 70);
            w.Opacity = 0.5f;

            textElement = new Text()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            textElement.SetFont(ResourceCache.GetFont("Fonts/Anonymous Pro.ttf"), 15);
            UI.Root.AddChild(textElement);
        }

        protected override void OnUpdate(float timeStep)
        {
            totalTime += timeStep;
            var sec = (int)totalTime;
            if (sec!=totalSec)
            {
                totalSec = sec;
                string info = string.Format("Time:{0}\nInfo\nPoint:0", totalSec);
                textElement.Value = info;
            }
            MoveCamera2D(timeStep);
        }

        protected void MoveCamera2D(float timeStep)
        {
            // Do not move if the UI has a focused element (the console)
            if (UI.FocusElement != null)
                return;

            // Movement speed as world units per second
            const float moveSpeed = 4.0f;

            if (mobileMushroomMove)
            {
                if (Input.GetKeyDown(Key.KP_8)) mobileMushroom.Translate(Vector3.UnitY * moveSpeed * timeStep);
                if (Input.GetKeyDown(Key.KP_2)) mobileMushroom.Translate(-Vector3.UnitY * moveSpeed * timeStep);
                if (Input.GetKeyDown(Key.KP_4)) mobileMushroom.Translate(-Vector3.UnitX * moveSpeed * timeStep);
                if (Input.GetKeyDown(Key.KP_6)) mobileMushroom.Translate(Vector3.UnitX * moveSpeed * timeStep);

                if (Input.GetKeyDown(Key.PageUp))
                {
                    mobileMushroom.Translate(Vector3.UnitZ * moveSpeed * timeStep);
                }

                if (Input.GetKeyDown(Key.PageDown))
                {
                    mobileMushroom.Translate(-Vector3.UnitZ * moveSpeed * timeStep);
                }
            }
            else
            {
                // Read WASD keys and move the camera scene node to the corresponding direction if they are pressed
                if (Input.GetKeyDown(Key.KP_8)) CameraNode.Translate(Vector3.UnitY * moveSpeed * timeStep);
                if (Input.GetKeyDown(Key.KP_2)) CameraNode.Translate(-Vector3.UnitY * moveSpeed * timeStep);
                if (Input.GetKeyDown(Key.KP_4)) CameraNode.Translate(-Vector3.UnitX * moveSpeed * timeStep);
                if (Input.GetKeyDown(Key.KP_6)) CameraNode.Translate(Vector3.UnitX * moveSpeed * timeStep);

                if (Input.GetKeyDown(Key.PageUp))
                {
                    Camera camera = CameraNode.GetComponent<Camera>();
                    camera.Zoom = camera.Zoom * 1.01f;
                }

                if (Input.GetKeyDown(Key.PageDown))
                {
                    Camera camera = CameraNode.GetComponent<Camera>();
                    camera.Zoom = camera.Zoom * 0.99f;
                }
            }

        }

        void SetupViewport()
        {
            var renderer = Renderer;
            renderer.SetViewport(0, new Viewport(Context, scene, CameraNode.GetComponent<Camera>(), null));
        }

        void CreateScene()
        {
            scene = new Scene();
            scene.CreateComponent<Octree>();

            // Create camera node
            CameraNode = scene.CreateChild("Camera");
            // Set camera's position
            CameraNode.Position = (new Vector3(0.0f, 0.0f, -10.0f));

            Camera camera = CameraNode.CreateComponent<Camera>();
            camera.Orthographic = true;

            var graphics = Graphics;
            camera.OrthoSize = (float)graphics.Height * PixelSize;
            // Set zoom according to user's resolution to ensure full visibility (initial zoom (1.0) is set for full visibility at 1280x800 resolution)
            camera.Zoom = (1.0f * Math.Min((float)graphics.Width / 1280.0f, (float)graphics.Height / 800.0f)); 

            var cache = ResourceCache;
            // Get tmx file
            TmxFile2D tmxFile = cache.GetTmxFile2D("data/isometric_grass_and_water.tmx");
            if (tmxFile == null)
                return;

            /*var n = tmxFile.NumLayers;
            for (uint i=0; i<n; i++)
            {
                TmxLayer2D layer = tmxFile.GetLayer(i);
                layer.
            }*/

            
            Node tileMapNode = scene.CreateChild("TileMap");
            tileMapNode.Position = new Vector3(0.0f, 0.0f, -1.0f);

            TileMap2D tileMap = tileMapNode.CreateComponent<TileMap2D>();

            // Set animation
            tileMap.TmxFile = tmxFile;


            var n = tileMap.NumLayers;
            var layer = tileMap.GetLayer(0);
            var node = tileMap.Node;
            var name = node.Name;
            while (node.GetNumChildren()==1)
            {
                node = node.Children[0];
                name = node.Name;
            }
            var n0 = node.GetNumChildren();

            int ii = 0;
            var result = from Node xx in node.Children /*where (ii++ > n / 3) && (ii < n * 2 / 3)*/ select xx;
            foreach (var nn in result)
                nn.Remove();

            var nx = result.Count();
            var n1 = node.GetNumChildren();

            //node.GetChild()


            // Set camera's position
            TileMapInfo2D info = tileMap.Info;

            float x = info.MapWidth * 0.5f;
            float y = info.MapHeight * 0.5f;
            
            CameraNode.Position = new Vector3(x, y, -10.0f);
            
            // Create a Zone component for ambient lighting & fog control
            var zoneNode = scene.CreateChild("Zone");
            var zone = zoneNode.CreateComponent<Zone>();
            zone.SetBoundingBox(new BoundingBox(-1000.0f, 1000.0f));
            zone.AmbientColor = new Color(0.15f, 0.15f, 0.15f);
            zone.FogColor = new Color(0.5f, 0.5f, 0.7f);
            zone.FogStart = 100.0f;
            zone.FogEnd = 300.0f;

            // Create a directional light to the world. Enable cascaded shadows on it
            var lightNode = scene.CreateChild("DirectionalLight");
            lightNode.SetDirection(new Vector3(0.6f, -1.0f, 0.8f));
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Directional;
            light.CastShadows = true;
            light.ShadowBias = new BiasParameters(0.00025f, 0.5f);
            // Set cascade splits at 10, 50 and 200 world units, fade shadows out at 80% of maximum shadow distance
            light.ShadowCascade = new CascadeParameters(10.0f, 50.0f, 200.0f, 0.0f, 0.8f);


            // Create some mushrooms
            const uint numMushrooms = 240;
            for (uint i = 0; i < numMushrooms; ++i)
            {
                var mushroomNode = scene.CreateChild("Mushroom");
                mushroomNode.Position = new Vector3(NextRandom(90.0f) - 45.0f, 0.0f, NextRandom(90.0f) - 45.0f);
                mushroomNode.Rotation = new Quaternion(0.0f, NextRandom(360.0f), 0.0f);
                mushroomNode.SetScale(0.5f + NextRandom(2.0f));

                StaticModel mushroomObject = mushroomNode.CreateComponent<StaticModel>();
                mushroomObject.Model = cache.GetModel("data/Mushroom.mdl");
                mushroomObject.SetMaterial(cache.GetMaterial("data/Mushroom.xml"));
                mushroomObject.CastShadows = true;
            }

            {
                mobileMushroom = scene.CreateChild("Mushroom");
                mobileMushroom.Position = new Vector3(20.0f, 10.0f, -10.0f);
                mobileMushroom.Rotation = new Quaternion(0.0f, NextRandom(360.0f), 0.0f);
                mobileMushroom.SetScale(0.5f + NextRandom(2.0f));

                StaticModel mushroomObject = mobileMushroom.CreateComponent<StaticModel>();
                mushroomObject.Model = cache.GetModel("data/Mushroom.mdl");
                mushroomObject.SetMaterial(cache.GetMaterial("data/Mushroom.xml"));
                mushroomObject.CastShadows = true;
            }

        }

        void CreateConsoleAndDebugHud()
        {
            var xml = cache.GetXmlFile("UI/DefaultStyle.xml");
            console = Engine.CreateConsole();
            console.DefaultStyle = xml;
            console.Background.Opacity = 0.8f;

            debugHud = Engine.CreateDebugHud();
            debugHud.DefaultStyle = xml;
        }
        void HandleKeyDown(KeyDownEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Esc:
                    Exit();
                    return;
                case Key.F1:
                    console.Toggle();
                    return;
                case Key.F2:
                    debugHud.ToggleAll();
                    return;
                case Key.F3:
                    {
                        var ep = new List<ObjectProperty>()
                        {
                            new ObjectProperty(Renderer, "TextureQuality", flags:PropertyFlags.UpDownSpinner, min:0, max:2),
                            new ObjectProperty(Renderer, "MaterialQuality", flags:PropertyFlags.UpDownSpinner, min:0, max:2),
                            new ObjectProperty(Renderer, "DrawShadows"),
                        };
                        MainWindow.thisOne.showToolWindow(ep);
                    }
                    return;
            }

            var renderer = Renderer;
            switch (e.Key)
            {
                case Key.X:
                    mobileMushroomMove = !mobileMushroomMove;
                    break;
                case Key.N1:
                    var quality = renderer.TextureQuality;
                    ++quality;
                    if (quality > 2)
                        quality = 0;
                    renderer.TextureQuality = quality;
                    break;

                case Key.N2:
                    var mquality = renderer.MaterialQuality;
                    ++mquality;
                    if (mquality > 2)
                        mquality = 0;
                    renderer.MaterialQuality = mquality;
                    break;

                case Key.N3:
                    renderer.SpecularLighting = !renderer.SpecularLighting;
                    break;

                case Key.N4:
                    renderer.DrawShadows = !renderer.DrawShadows;
                    break;

                case Key.N5:
                    var shadowMapSize = renderer.ShadowMapSize;
                    shadowMapSize *= 2;
                    if (shadowMapSize > 2048)
                        shadowMapSize = 512;
                    renderer.ShadowMapSize = shadowMapSize;
                    break;

                // shadow depth and filtering quality
                case Key.N6:
                    var q = (int)renderer.ShadowQuality++;
                    if (q > 3)
                        q = 0;
                    renderer.ShadowQuality = (ShadowQuality)q;
                    break;

                // occlusion culling
                case Key.N7:
                    var o = !(renderer.MaxOccluderTriangles > 0);
                    renderer.MaxOccluderTriangles = o ? 5000 : 0;
                    break;

                // instancing
                case Key.N8:
                    renderer.DynamicInstancing = !renderer.DynamicInstancing;
                    break;

                case Key.N9:
                    Image screenshot = new Image();
                    Graphics.TakeScreenShot(screenshot);
                    screenshot.SavePNG(FileSystem.ProgramDir + $"Data/Screenshot_{GetType().Name}_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture)}.png");
                    break;
            }
        }

    }
}
