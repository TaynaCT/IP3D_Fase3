using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IP3D_Fase3
{
    class Particle
    {
        BasicEffect effect;
        Vector3 posicao, gravidaded;
        Matrix worldMatrix;

        public Particle(GraphicsDevice d, Random rnd, float raio, float altura)
        {
            effect = new BasicEffect(d);
            effect.World = Matrix.Identity;
            worldMatrix = Matrix.Identity;

            posicao = new Vector3((float)rnd.NextDouble() * raio * (float)(Math.Cos(rnd.NextDouble() * MathHelper.TwoPi)),
                                    altura,
                                    (float)rnd.NextDouble() * raio * (float)Math.Sin(rnd.NextDouble() * MathHelper.TwoPi));

            gravidaded = new Vector3((float)(rnd.NextDouble() * 0.10) - 0.1f,
                                                -0.3f,
                                                (float)(rnd.NextDouble() * 0.10) - 0.1f);
        }

        public void Update()
        {
            posicao += gravidaded;
        }

        //métodos de retorno
        public float Height
        {
            get { return posicao.Y; }
        }
        public Vector3 Pos
        {
            get { return posicao; }
        }

    }
}
