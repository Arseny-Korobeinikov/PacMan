using PacMan.Graphics;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace PacMan
{
    internal class Ghost2
    {
        public List<Vector3> gameVerts;
        public List<Vector2> gameGraphic = new List<Vector2>();
        public List<uint> gameInd;

        VAO gameVAO;
        VBO gameVertexVBO;
        VBO gameGraphicVBO;
        IBO gameIBO;

        Texture texture;

        public Ghost2(int fear)
        {
            gameVerts = new List<Vector3>()
            {
            new Vector3(-0.02f, 0.02f, 0.02f),
            new Vector3(0.02f, 0.02f, 0.02f),
            new Vector3(0.02f, -0.02f, 0.02f),
            new Vector3(-0.02f, -0.02f, 0.02f),

            new Vector3(0.02f, 0.02f, 0.02f),
            new Vector3(0.02f, 0.02f, -0.02f),
            new Vector3(0.02f, -0.02f, -0.02f),
            new Vector3(0.02f, -0.02f, 0.02f),

            new Vector3(0.02f, 0.02f, -0.02f),
            new Vector3(-0.02f, 0.02f, -0.02f),
            new Vector3(-0.02f, -0.02f, -0.02f),
            new Vector3(0.02f, -0.02f, -0.02f),

            new Vector3(-0.02f, 0.02f, -0.02f),
            new Vector3(-0.02f, 0.02f, 0.02f),
            new Vector3(-0.02f, -0.02f, 0.02f),
            new Vector3(-0.02f, -0.02f, -0.02f),

            new Vector3(-0.02f, 0.02f, -0.02f),
            new Vector3(0.02f, 0.02f, -0.02f),
            new Vector3(0.02f, 0.02f, 0.02f),
            new Vector3(-0.02f, 0.02f, 0.02f),

            new Vector3(-0.02f, -0.02f, 0.02f),
            new Vector3(0.02f, -0.02f, 0.02f),
            new Vector3(0.02f, -0.02f, -0.02f),
            new Vector3(-0.02f, -0.02f, -0.02f),
            };
            gameGraphic = new List<Vector2>()
            {
             new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),

            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f),
            };

            gameInd = new List<uint>{
            0, 1, 2,
            2, 3, 0,

            4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
            };
            if (fear == 1)
            {
                BuildObject("ghost_fear.png");
            }
            else
            {
                BuildObject("ghost2.png");
            }
            //BuildObject("ghost2.png");
        }

        public void BuildObject(String path)
        {
            gameVAO = new VAO();
            gameVAO.Bind();

            gameVertexVBO = new VBO(gameVerts);
            gameVertexVBO.Bind();
            gameVAO.LinkToVAO(0, 3, gameVertexVBO);

            gameGraphicVBO = new VBO(gameGraphic);
            gameGraphicVBO.Bind();
            gameVAO.LinkToVAO(1, 2, gameGraphicVBO);

            gameIBO = new IBO(gameInd);

            texture = new Texture(path);
        }

        public void Render(ShaderProgram program)
        {
            program.Bind();
            gameVAO.Bind();
            gameIBO.Bind();
            texture.Bind();
            GL.DrawElements(PrimitiveType.Triangles, gameInd.Count, DrawElementsType.UnsignedInt, 0);
        }

        public void Delete()
        {
            gameVAO.Delete();
            gameVertexVBO.Delete();
            gameGraphicVBO.Delete();
            gameIBO.Delete();
            texture.Delete();
            Position = (-1, -1);
            nextPosition = (-1, -1);
        }


        //---   GAME LOGIC   ---
        public Vector2i Position = new Vector2i(26, 5);
        List<Vector2i> Dir = new List<Vector2i>
            {
                new Vector2i(0,-1),
                new Vector2i(1,0),
                new Vector2i(0,1),
                new Vector2i(-1,0),
                new Vector2i(0,0)
            };
        int curDir = 4;

        public void ChangePosition(int Time)
        {
            if (Time == 1)
                Move();
        }

        int IrerationsMove = 0;
        public Vector2i nextPosition;

        void Move()
        {


            if (IrerationsMove % 18 < 5)
            {
                nextPosition = Position + Dir[3];
            }
            else
            {
                if (IrerationsMove % 18 < 9)
                {
                    nextPosition = Position + Dir[0];
                }
                else
                {
                    if (IrerationsMove % 18 < 14)
                    {
                        nextPosition = Position + Dir[1];
                    }
                    else
                    {
                        nextPosition = Position + Dir[2];
                    }
                }
            }

            Position = nextPosition;
            IrerationsMove++;
            if (IrerationsMove == 18) { IrerationsMove = 0; }

        }







    }
}
