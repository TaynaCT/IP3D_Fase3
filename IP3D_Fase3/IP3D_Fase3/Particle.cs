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
                
        public VertexPositionColor[] drop;
        float radius;
        float distance;
        //float height;
        Vector3 fallSpeed;
        Vector3 position;
        Vector3 tankPos;
                
        public Particle(GraphicsDevice device, Vector3 tankPos, float radius, Random rand)
        {
            effect = new BasicEffect(device);            
            effect.VertexColorEnabled = true;
            this.tankPos = tankPos;
            //this.height = center.Y;
            this.radius = radius;
            distance = (float)rand.NextDouble() * radius;
            float angle = (float)rand.NextDouble() * MathHelper.TwoPi;

            //pos = c + r * d * (cos(a); 0 ; sin(a)) 
            float x = distance * (float)Math.Cos(angle);
            float z = distance * (float)Math.Sin(angle);
            float y = distance * (float)Math.Tan(angle);
            //posição inicial da particula
            position = tankPos - new Vector3(x, y, z);

            // Dois vertices para cada gota
            drop = new VertexPositionColor[2];

            //direção da queda
            float fallSpeedX = (float)rand.NextDouble() * .1f;
            float fallSpeedZ = (float)rand.NextDouble() * .1f;
            float fallSpeedY = (float)rand.NextDouble() * .1f;

            fallSpeed = new Vector3(fallSpeedX, fallSpeedY, fallSpeedZ);
            
            fallSpeed.Normalize();

            drop[0] = new VertexPositionColor(position, Color.Purple);
            drop[1] = new VertexPositionColor(position, Color.Purple);
            
        }

        public void Update(GameTime gameTime)
        {
            fallSpeed += new Vector3(.0f, -9.8f, .0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            drop[1].Position = drop[0].Position;
            drop[0].Position += fallSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //drop[1].Position += drop[0].Position * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(GraphicsDevice device, Matrix view, Matrix projection)
        {
            effect.CurrentTechnique.Passes[0].Apply();

            effect.World = Matrix.Identity;
            effect.View = view;
            effect.Projection = projection;
            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, drop, 0, 1);
        }

    }
}
