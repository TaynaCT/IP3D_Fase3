using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace IP3D_Fase3
{
    class Map
    {
        public BasicEffect effect;
        Matrix worldMatrix;
        Color[] texels;
        Texture2D texture;
        Texture2D groundTexture;
        public VertexPositionNormalTexture[] vertex;

        short[] index;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        List<Vector3> normalList;
        
        public Map(GraphicsDevice device, ContentManager content, BasicEffect camEffect/*CameraSurfaceFollow cam*/)
        {            
            worldMatrix = Matrix.Identity;
            effect = camEffect;

            //textura do mapa
            groundTexture = content.Load<Texture2D>("grassTexture");
            effect.TextureEnabled = true;
            effect.Texture = groundTexture;

            normalList = new List<Vector3>();

            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;
            
            //effects relacionados as luzes
            effect.EnableDefaultLighting();// ativa a luz 
            effect.LightingEnabled = true;
            effect.AmbientLightColor = new Vector3(.4f, .4f, .4f);
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 1f, 1f);
            effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1f, -1f, 0f));
            //effect.DirectionalLight0.SpecularColor = new Vector3(2f, 0f, 2f);    

            effect.VertexColorEnabled = false;

            //Indexação dos vertices do mapa, a partir dos valores rgb da textura
            texture = content.Load<Texture2D>("lh3d1");
            texels = new Color[texture.Height * texture.Width]; // tamanho do array =  altura *  largura da img
            texture.GetData(texels);
            vertex = new VertexPositionNormalTexture[texels.Length];

            //Gerar vertices
            int y;
            for (int z = 0; z < texture.Height; z++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    y = (texels[z * texture.Width + x].R);
                    vertex[z * texture.Width + x] = new VertexPositionNormalTexture(new Vector3(x, y * .04f, z), Vector3.Up, new Vector2(x % 2, z % 2));
                }
            }

            //indexação do vértices
            index = new short[6 * (texture.Height - 1) * (texture.Width - 1)];//inicialização do array de indices

            int a = 0;
            for (int z = 0; z < texture.Height - 1; z++)
            {
                int aux = texture.Width - 1;
                for (int x = 0; x < texture.Width; x++)
                {
                    if (z % 2 == 0)
                    {
                        index[2 * a] = (short)(z * texture.Width + x + texture.Width);
                        index[2 * a + 1] = (short)(z * texture.Width + x);
                        a++;
                    }
                    if (z % 2 == 1)
                    {
                        index[2 * a] = (short)(z * texture.Width + aux);
                        index[2 * a + 1] = (short)(z * texture.Width + aux + texture.Width);
                        a++;
                        aux--;
                    }
                }
            }

            vertexBuffer = new VertexBuffer(device,
                typeof(VertexPositionNormalTexture),
                vertex.Length,
                BufferUsage.None);

            vertexBuffer.SetData<VertexPositionNormalTexture>(vertex);

            indexBuffer = new IndexBuffer(device,
                typeof(short),
                index.Length,
                BufferUsage.None);
            indexBuffer.SetData(index);
            Normals();
        }

        /// <summary>
        /// calcula a normal de cada vertice no plano
        /// gera as normais
        /// </summary>
        public void Normals()
        {
            Vector3 normalVertex = new Vector3();

            //Declaração das normais no centro do mapa
            //vertices dos cantos do terreno
            for (int x = 0; x < texture.Width - 1; x++)
            {
                int z = 0;

                if (z == 0 && x == 0)
                {
                    normalList.Add(Vector3.Cross(vertex[z * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z + 1) * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z + 1) * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z + 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position));
                }
                else if (x == texture.Width)
                {
                    normalList.Add(Vector3.Cross(vertex[z * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z + 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z + 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z + 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position));
                }
                else
                {

                    normalList.Add(Vector3.Cross(vertex[z * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z + 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z + 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z + 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z + 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z + 1) * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z + 1) * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[z * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position));
                }
                //soma das normais do vertice atual
                foreach (Vector3 normal in normalList)
                {
                    normalVertex += Vector3.Normalize(normal);
                }

                //calcula a media das nomais no vertice.
                normalVertex /= normalList.Count;

                vertex[z * texture.Width + x].Normal = normalVertex;

                normalList.Clear();
            }

            for (int x = texture.Width - 1; x >= 0; x--)
            {
                int z = texture.Height - 1;

                if (z == texture.Height - 1 && x == texture.Width - 1)
                {
                    normalList.Add(Vector3.Cross(vertex[(z - 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z - 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z - 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[z * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));
                }
                else if (x == 0)
                {
                    normalList.Add(Vector3.Cross(vertex[z * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z - 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z - 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z - 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position));
                }
                else
                {

                    normalList.Add(Vector3.Cross(vertex[z * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z - 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z - 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z - 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z - 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z - 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z - 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[z * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));
                }
                //soma das normais do vertice atual
                foreach (Vector3 normal in normalList)
                {
                    normalVertex += Vector3.Normalize(normal);
                }

                //calcula a media das nomais no vertice.
                normalVertex /= normalList.Count;

                vertex[z * texture.Width + x].Normal = normalVertex;

                normalList.Clear();
            }

            //vertices do centro do terreno
            for (int z = 1; z < texture.Height - 2; z++)
            {
                for (int x = 1; x < texture.Width - 2; x++)
                {
                    normalList.Add(Vector3.Cross(vertex[(z - 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z - 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z - 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[z * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[z * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z + 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z + 1) * texture.Width + x - 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z + 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z + 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z + 1) * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z + 1) * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[z * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[z * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z - 1) * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position));

                    normalList.Add(Vector3.Cross(vertex[(z - 1) * texture.Width + x + 1].Position - vertex[z * texture.Width + x].Position,
                                                 vertex[(z - 1) * texture.Width + x].Position - vertex[z * texture.Width + x].Position));

                    //soma das normais do vertice atual
                    foreach (Vector3 normal in normalList)
                    {
                        normalVertex += Vector3.Normalize(normal);
                    }

                    //calcula a media das nomais no vertice.
                    normalVertex /= normalList.Count;

                    vertex[z * texture.Width + x].Normal = normalVertex;

                    normalList.Clear();

                }
            }
        }

        /// <summary>
        /// Função que devolve um float correspondente a altura da num determinado ponto do plano a partir do parametros x e z
        /// </summary>
        /// <param name="x">pos.X</param>
        /// <param name="z">pos.Z</param>
        /// <returns>height</returns>
        public float SurfaceFollow(float x, float z)
        {
            int xMin = (int)Math.Floor(x);
            int xMax = xMin + 1;
            int zMin = (int)Math.Floor(z);
            int zMax = zMin + 1;

            //verifica se está dentro dos limites  do mapa
            if ((xMin < 0) || (zMin < 0) || (xMax > texture.Width) || (zMax > texture.Height))
                return -1;

            Vector3 p1 = new Vector3(xMin, vertex[zMax * texture.Width + xMin].Position.Y, zMax);
            Vector3 p2 = new Vector3(xMax, vertex[zMin * texture.Width + xMax].Position.Y, zMin);
            Vector3 p3;

            if ((x - xMin) + (z - zMin) <= 1)
                p3 = new Vector3(xMin, vertex[zMin * texture.Width + xMin].Position.Y, zMin);
            else
                p3 = new Vector3(xMax, vertex[zMax * texture.Width + xMax].Position.Y, zMax);

            Plane plane = new Plane(p1, p2, p3);
            Ray ray = new Ray(new Vector3(x, 0, z), Vector3.Up);

            //o raio intercede com o plano ?
            // float? - pode conter um valor do tipo float ou esta null
            float? height = ray.Intersects(plane);

            //se height tem valor retorna esse valor
            //se não, retorna 0
            return height.HasValue ? height.Value : 0f;
        }

        public Vector3 NormalFollow(float x, float z)
        {
            int xMin = (int)Math.Floor(x);
            int xMax = xMin + 1;
            int zMin = (int)Math.Floor(z);
            int zMax = zMin + 1;

            //verifica se está dentro dos limites  do mapa
            if ((xMin < 0) || (zMin < 0) || (xMax > texture.Width) || (zMax > texture.Height))
                return Vector3.Up;

            Vector3 p1 = vertex[zMax * texture.Width + xMin].Normal;
            Vector3 p2 = vertex[zMin * texture.Width + xMax].Normal;

            return p1 + (p2 - p1) * ((x - xMin) + (z - zMin));
        }

        public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
        {
            effect.View = view;
            effect.Projection = projection;
            effect.World = worldMatrix;
            effect.CurrentTechnique.Passes[0].Apply();
            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;

            device.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, texels.Length * 6);
        }

        public int MapLimit
        {
            get { return texture.Height; }
        }
    }
}
