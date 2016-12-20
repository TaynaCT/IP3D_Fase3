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
        Vector3 position, gravidade; //variaveis vetoriais
        Matrix worldMatrix;
        Vector3 velocity;

        public Particle(GraphicsDevice d, Random rnd, float raio, float altura,float dx, float dz, float px, float pz)
        {
            effect = new BasicEffect(d);
            effect.World = Matrix.Identity;
            worldMatrix = Matrix.Identity;

            //vetor de posição de particulas (o rnd.nextdouble gera um valor random entre 0 e 1)
            position = new Vector3(px+(float)rnd.NextDouble() * raio,
                                    altura+0.03f,
                                    pz+(float)rnd.NextDouble() * raio);

            //direção e sentido das particulas
            gravidade = new Vector3((float)(rnd.NextDouble())*0.1f * (dx/dx)*2,
                                      0/*-0.3f*/,
                                    (float)(rnd.NextDouble())* 0.1f * (dz/dz)*2);
            velocity = position;
            
        }

        public void Update(GameTime gameTime)
        {
            float timePassed = 0;
            timePassed += (float)gameTime.ElapsedGameTime.Milliseconds / 2096.0f;
            //posicao += gravidade;
            position += velocity * ((float)gameTime.ElapsedGameTime.Milliseconds / 90.0f);
            position.Y -= velocity.Y * 9.8f * timePassed * timePassed;
        }

        //métodos de retorno
        public float Height
        {
            get { return position.Y; }
        }
        public Vector3 Pos
        {
            get { return position; }
        }

    }
}
