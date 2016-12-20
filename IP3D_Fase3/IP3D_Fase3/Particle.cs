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
    {   //efeito basico
        BasicEffect effect;
        Vector3 posicao, gravidade; //variaveis vetoriais
        Matrix worldMatrix;

        public Particle(GraphicsDevice d, Random rnd, float raio, float altura,float dx, float dz, float px, float pz)
        {
            effect = new BasicEffect(d);
            effect.World = Matrix.Identity;
            worldMatrix = Matrix.Identity;

            //vetor de posição de particulas (o rnd.nextdouble gera um valor random entre 0 e 1)
            posicao = new Vector3(px+(float)rnd.NextDouble() * raio,
                                    altura+0.03f,
                                    pz+(float)rnd.NextDouble() * raio);

            //direção e sentido das particulas
            gravidade = new Vector3((float)(rnd.NextDouble())*0.1f * (dx/dx)*2,
                                      0/*-0.3f*/,
                                     (float)(rnd.NextDouble())* 0.1f * (dz/dz)*2);
        }

        public void Update()
        {
            posicao += gravidade;
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
