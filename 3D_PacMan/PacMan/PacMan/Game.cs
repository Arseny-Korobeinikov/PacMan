using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
//using OpenTK.Graphics;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PacMan.Graphics;
using Microsoft.VisualBasic.FileIO;

namespace PacMan
{

    
    internal class Game : GameWindow
    {
        ShaderProgram program;
        Brick brick;
        Coin coin;
        Pill pill;
        Player player;
        Map map;
        Ghost1 ghost1;
        Ghost1 ghost_fear1;
        Ghost2 ghost2;
        Ghost2 ghost_fear2;
        Ghost3 ghost3;
        Ghost3 ghost_fear3;
        Ghost4 ghost4;
        Ghost4 ghost_fear4;
        int width, height;
        Camera camera;

        int lose = -1;
        int lifeghost1 = 1;
        int lifeghost2 = 1;
        int lifeghost3 = 1;
        int lifeghost4 = 1;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.width = width;
            this.height = height;

            // center window
            CenterWindow(new Vector2i(width, height));
        }
        // called whenever window is resized
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
        }

        // called once when game is started
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);
            brick = new Brick();
            player = new Player();
            coin = new Coin();
            pill = new Pill();
            ghost1 = new Ghost1(0);
            ghost_fear1 = new Ghost1(1);
            ghost2 = new Ghost2(0);
            ghost_fear2 = new Ghost2(1);
            ghost3 = new Ghost3(0);
            ghost_fear3 = new Ghost3(1);
            ghost4 = new Ghost4(0);
            ghost_fear4 = new Ghost4(1);
            map = new Map("../../../Textures/map.bmp");
            program = new ShaderProgram("Default.vert", "Default.frag");
            camera = new Camera(width, height, new Vector3(0.048f*14,0.048f*15.5f,2.5f));
            CursorState = CursorState.Grabbed;
        }
        // called once when game is closed
        protected override void OnUnload()
        {
            base.OnUnload();
            coin.Delete();
            pill.Delete();
            brick.Delete();
            player.Delete();
        }

        float depth = -0.5f;
        //int Frames = 0;
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //Frames++;
            GL.ClearColor(0.3f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            KeyboardState input = KeyboardState;

            if(map.coins==0)
            {
                Console.WriteLine("You Win! :)");
                this.Close();
            }
            if(lose == 1)
            {
                Console.WriteLine("You Lose :(");
                this.Close();
            }
            // transformation matrices
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();
            int modelLocation = GL.GetUniformLocation(program.ID, "model");
            int viewLocation = GL.GetUniformLocation(program.ID, "view");
            int projectionLocation = GL.GetUniformLocation(program.ID, "projection");
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            for (int i=0;i<28;++i)
            {
                for(int j=0;j<31;++j) 
                {
                    model = Matrix4.Identity;
                    model *= Matrix4.CreateTranslation((28 - i) * 0.048f, (31 - j) * 0.048f,0);
                    GL.UniformMatrix4(modelLocation,true,ref model);

                    if (map.map[i, j] == 1)
                        brick.Render(program);
                    if (map.map[i, j] == 2)
                        coin.Render(program);
                    if (map.map[i, j] == 3)
                        pill.Render(program);                    
                }
            }

            
            model = Matrix4.Identity;
            model *= Matrix4.CreateTranslation((28 - player.Position.X) * 0.048f, (31 - player.Position.Y) * 0.048f,0);
            GL.UniformMatrix4(modelLocation, true, ref model);
            player.Render(program);

            //if(player.RageTime > 0)
            //{
            //    if (lifeghost1 == 1)
            //        ghost1.BuildObject("ghost_fear.png");
            //    if (lifeghost2 == 1)
            //        ghost2.BuildObject("ghost_fear.png");
            //    if (lifeghost3 == 1)
            //        ghost3.BuildObject("ghost_fear.png");
            //    if (lifeghost4 == 1)
            //        ghost4.BuildObject("ghost_fear.png");
            //}
            //else
            //{
            //    if (lifeghost1 == 1)
            //        ghost1.BuildObject("ghost1.png");
            //    if (lifeghost2 == 1)
            //        ghost2.BuildObject("ghost2.png");
            //    if (lifeghost3 == 1)
            //        ghost3.BuildObject("ghost3.png");
            //    if (lifeghost4 == 1)
            //        ghost4.BuildObject("ghost4.png");
            //}

            if (lifeghost1 == 1)
            {
                //if (player.RageTime > 0) { ghost1 = new Ghost1(1); }
                //else { ghost1 = new Ghost1(0); }
                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation((28 - ghost1.Position.X) * 0.048f, (31 - ghost1.Position.Y) * 0.048f, 0);
                GL.UniformMatrix4(modelLocation, true, ref model);
                if(player.RageTime>0)
                    ghost_fear1.Render(program);
                else
                    ghost1.Render(program);
            }

            if (lifeghost2 == 1)
            {
                //if (player.RageTime > 0) { ghost1 = new Ghost1(1); }
                //else { ghost1 = new Ghost1(0); }
                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation((28 - ghost2.Position.X) * 0.048f, (31 - ghost2.Position.Y) * 0.048f, 0);
                GL.UniformMatrix4(modelLocation, true, ref model);
                if (player.RageTime > 0)
                    ghost_fear2.Render(program);
                else
                    ghost2.Render(program);
            }
            if (lifeghost3 == 1)
            {
                //if (player.RageTime > 0) { ghost1 = new Ghost1(1); }
                //else { ghost1 = new Ghost1(0); }
                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation((28 - ghost3.Position.X) * 0.048f, (31 - ghost3.Position.Y) * 0.048f, 0);
                GL.UniformMatrix4(modelLocation, true, ref model);
                if (player.RageTime > 0)
                    ghost_fear3.Render(program);
                else
                    ghost3.Render(program);
                //ghost3.Render(program);
            }
            if (lifeghost4 == 1)
            {
                //if (player.RageTime > 0) { ghost1 = new Ghost1(1); }
                //else { ghost1 = new Ghost1(0); }
                model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation((28 - ghost4.Position.X) * 0.048f, (31 - ghost4.Position.Y) * 0.048f, 0);
                GL.UniformMatrix4(modelLocation, true, ref model);
                if (player.RageTime > 0)
                    ghost_fear4.Render(program);
                else
                    ghost4.Render(program);
                //ghost4.Render(program);
            }

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
        // called every frame. All updating happens here

        int Timer = 0;

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            Timer++;
            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;

            base.OnUpdateFrame(args);

            int changeRes = player.ChangePosition(input, map, Timer, ghost1, ghost2, ghost3, ghost4);
            if (changeRes == -1)
            {
                lose = 1;
            }
            if (changeRes == 1)
            {
                lifeghost1 = -1;
                ghost1.Delete();
            }
            if (changeRes == 2)
            {
                lifeghost2 = -1;
                ghost2.Delete();
            }
            if (changeRes == 3)
            {
                lifeghost3 = -1;
                ghost3.Delete();
            }
            if (changeRes == 4)
            {
                lifeghost4 = -1;
                ghost4.Delete();
            }





            if (lifeghost1 == 1)
            {
                ghost1.ChangePosition(Timer);
            }
            if (lifeghost2 == 1)
            {
                ghost2.ChangePosition(Timer);
            }
            if (lifeghost3 == 1)
            {
                ghost3.ChangePosition(Timer);
            }
            if (lifeghost4 == 1)
            {
                ghost4.ChangePosition(Timer);
            }


            if (Timer == 80) {Timer = 0;}
            

            camera.Update(input, mouse, args);
            
        }

        // Function to load a text file and return its contents as a string


    }
}
