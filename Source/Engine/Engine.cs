using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Diagnostics;
using System.Threading;

namespace TCEngine
{

    public class TecnoCampusEngine
    {
        //
        // Members
        //  
        public const uint WINDOW_WIDTH = 1024;
        public const uint WINDOW_HEIGHT = 768;

        private const byte FRAME_RATE = 60;
        private const float FIXED_DELTA_TIME = 1.0f / FRAME_RATE;
        private const float MAX_DELTA_TIME = 5.0f;
        private const int MAX_FRAMES_WITHOUT_RENDERING = 5;

        private static TecnoCampusEngine ms_Instance;
        private RenderWindow m_Window;
        private View m_View;
        private Resources m_Resources;
        private DebugManager m_DebugManager;
        private Scene m_Scene;
        private float m_Time;

        //
        // Accessors
        //
        public static TecnoCampusEngine Get
        {
            get
            {
                if( ms_Instance == null)
                {
                    ms_Instance = new TecnoCampusEngine();
                }
                return ms_Instance;
            }
        }

        public RenderWindow Window 
        {
            get => m_Window;
        }

        public Vector2f ViewportSize
        {
            get => m_View.Size;
            set => m_View.Reset(new FloatRect(0.0f, 0.0f, value.X, value.Y));
        }

        public Resources Resources
        {
            get => m_Resources;
        }

        public DebugManager DebugManager
        {
            get => m_DebugManager;
        }

        public Scene Scene
        {
            get => m_Scene;
        }

        public float Time
        {
            get => m_Time;
        }

        // 
        // Methods
        //
        private TecnoCampusEngine()
        {
            // The only way to access this constructor is through the static Get method
            m_Resources = new Resources();
        }


        private void Init()
        {
            m_Resources.LoadResources();

            VideoMode videoMode = new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT);
            m_Window = new RenderWindow(videoMode, "Game");
            m_Window.SetVerticalSyncEnabled(true);

            m_View = new View(new FloatRect(0f, 0f, videoMode.Width, videoMode.Height));
            m_View.Viewport = new FloatRect(0f, 0f, 1f, 1f);

            m_DebugManager = new DebugManager();
            m_DebugManager.Init();

            m_Scene = new Scene();
        }

        private void DeInit()
        {
            m_DebugManager.DeInit();
            m_Window.Dispose();
        }

        private void Update(float _dt)
        {
            m_Window.DispatchEvents();

            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                m_Window.Close();
            }

            m_DebugManager.Update(_dt);
            m_Scene.Update(_dt);
            m_Time += _dt;
        }

        private void Draw()
        {
            Color ClearColor = new Color(40, 40, 40); 
            Window.Clear(ClearColor);
            Window.SetView(m_View);

            Window.Draw(m_Scene);
            Window.Draw(m_DebugManager);

            Window.Display();
        }

        private void EndFrame()
        {
            m_Scene.EndFrame();
        }

        private bool IsAlive()
        {
            return m_Window.IsOpen;
        }

        public void Run(Game _game)
        {
            Debug.Assert(_game != null, "The _game parameter of type Game cannot be null");

            Init();
            _game.Init();

            DateTime startGameTime = DateTime.Now;
            int framesWithoutRendering = 0;
            double nextTime = (DateTime.Now - startGameTime).TotalMilliseconds / 1000.0;
            //  nextTime will be 0

            while(IsAlive())
            {
                // DESIGN NOTES
                // We have 3 cases during the update: 
                //    1. The time passed between 2 frames is exactly FIXED_DELTA_TIME
                //    2. The time passed between 2 frames is greater than FIXED_DELTA_TIME
                //    3. The time passed between 2 frames is smaller than FIXED_DELTA_TIME
                double beginFrameTime = (DateTime.Now - startGameTime).TotalMilliseconds / 1000.0;
                
                // Case 1 -> This check is always false
                // Case 2 -> This check will be true from time to time (when the accumulated time is greater than MAX_DELTA_TIME)
                // Case 3 -> This check is always false
                if((beginFrameTime - nextTime) > MAX_DELTA_TIME) 
                {
                    // We are in Case 2, the game is too slow (it could be because for several reasons), so we "hack" our data in order
                    // to keep updating/rendering the game
                    nextTime = beginFrameTime;
                }

                // Case 1 -> This check is always true
                // Case 2 -> This check is always true
                // Case 3 -> This check is always false
                if (beginFrameTime >= nextTime)
                {
                    // We are in Case 1 or 2
                    nextTime += FIXED_DELTA_TIME; // Update nextTime to the expected next beginFrameTime

                    Update(FIXED_DELTA_TIME);
                    _game.Update(FIXED_DELTA_TIME);

                    // We render the frame only if one of the next two conditions is true:
                    //   - We are in Case 1
                    //   - We are in Case 2, but we already skipped the rendering step durring too many frames in a row
                    if ((beginFrameTime < nextTime) || (framesWithoutRendering >= MAX_FRAMES_WITHOUT_RENDERING))
                    {
                        Draw();
                        framesWithoutRendering = 0;
                    }
                    else
                    {
                        ++framesWithoutRendering;
                    }
                }
                else
                {
                    // We are in Case 3, the game is too fast, so we need to put him to sleep to avoid wasting the CPU.
                    // This will make the code think that at the beginning of the next frame, we are on Case 1.
                    int sleepTime = (int)(1000.0 * (nextTime - beginFrameTime));
                    if(sleepTime > 0)
                    {
                        Thread.Sleep(sleepTime);
                    }
                }

                EndFrame();
            }

            _game.DeInit();
            DeInit();
        }
    }
}
