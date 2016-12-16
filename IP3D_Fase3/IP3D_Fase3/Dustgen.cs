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
        float raio, comprimento, largura, altura;
        List<Particle> dust;
        GraphicsDevice dev;
        CameraSurfaceFollow cam;
        Random rnd;

        public Dustgen(GraphicsDevice d,CameraSurfaceFollow c)
        {
            effect = new BasicEffect(d);
            effect.VertexColorEnabled = true;
            particulas = 1000;
            raio = 6f;
            comprimento = 6f;
            largura = 10f;
            altura = 20f;

            this.cam = c;
            this.dev = d;
            rnd = new Random();
            dust = new List<Particle>();
        }

        public void ciclo()
        {

            for (int i = 0; i < 10f; i++)
                if (dust.Count < particulas)
                {
                    dust.Add(new Particle(dev, rnd, raio, altura));
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

        public void Draw()
        {
            effect.World = Matrix.Identity;
            //effect.View = cam.;
            effect.Projection = cam.projection;




        }
    }
}
