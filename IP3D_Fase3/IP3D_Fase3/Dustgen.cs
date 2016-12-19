using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class Dustgen
    {
        BasicEffect effect;
        int particulas;
        float raio, comprimento, largura;
        List<Particle> dust;
        GraphicsDevice dev;
        Random rnd;
        

        public Dustgen(GraphicsDevice d)
        {
            effect = new BasicEffect(d);
            effect.VertexColorEnabled = true;
            particulas = 10000;
            raio = 0.6f;
            comprimento = 1f;
            largura = 1f;

            this.dev = d;
            rnd = new Random();
            dust = new List<Particle>();
        }

        public void Ciclo(float dx, float dz, float px, float alt, float pz)
        {

            for (int i = 0; i < 10f; i++)
                if (dust.Count < particulas)
                {
                    dust.Add(new Particle(dev, rnd, raio, alt,dx,dz, px, pz));
                }
                else
                    break;

            for (int i = 0; i < dust.Count; i++)
            {
                dust[i].Update();
                if (dust[i].Height < 0)
                {
                    dust.RemoveAt(i);
                }
            }
        }

        public void Draw(Matrix view, Matrix projection)
        {
            effect.World = Matrix.Identity;
            effect.View = view;
            effect.Projection = projection;

            effect.CurrentTechnique.Passes[0].Apply();

            VertexPositionColor[] vertices = new VertexPositionColor[dust.Count * 2];

            for(int i = 0; i< dust.Count; i++)
            {
                vertices[i * 2] =new VertexPositionColor(dust[i].Pos, Color.White);

                vertices[i * 2 + 1] = new VertexPositionColor(new Vector3(dust[i].Pos.X, dust[i].Pos.Y + 0.05f, dust[i].Pos.Z), Color.White);
            }
                if(vertices.Length>0)
                dev.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, dust.Count);
        }
    }
}   
