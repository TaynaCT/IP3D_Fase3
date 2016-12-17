using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class Bullet
    {
        Model myBullet;
        Matrix worldMatrix;         
        Map terrain;        
        Vector3 position, inicialPos;
        public Vector3 speed, aceleration;
        public float scale;
       
        public float yaw = 0;
        bool bulletFlag;
                
        CameraSurfaceFollow camera;
        
        public Bullet(GraphicsDevice device, ContentManager content, CameraSurfaceFollow cam, Map map, Vector3 pos)
        {
            camera = cam;
            terrain = map;
            myBullet = content.Load<Model>("Cube");
            bulletFlag = false;
                       
            position = pos;
            inicialPos = position;
            scale = 0.025f;            
        }

        //public void bulletUpdate(GameTime gameTime,float x, float z)
        //{
        //    float gravity = 0;
        //    if (bulletFlag)
        //    {
        //        gravity -= 0.3f;
        //        position += new Vector3(x, gravity, z) * 0.04f;

        //        Trajectory(gameTime, x, z);                
        //    }                     
        //}
        public void bulletUpdate(GameTime gameTime, Vector3 direction)
        {
            
            Vector3 gravity = new Vector3(0, -.5f, 0);
            if (bulletFlag)
                position += direction * (float)gameTime.ElapsedGameTime.TotalSeconds;    
        }

        /// <summary>
        /// Calcula a trajetoria o projetil a ser disparado
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>Posição final do projetil</returns>
        public Vector3 Trajectory(GameTime gameTime, float x, float z)
        {
            float timePassed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            speed = new Vector3(x, 0, z);
            aceleration = new Vector3(.5f, 0.1f, 2f);
            //x = xo + vo t + ½ a t2 
            Vector3 finalPos = inicialPos + speed * timePassed + aceleration * 1 / 2 * (float)Math.Pow(timePassed, 2);

            return finalPos;
        }

        public void Draw()
        {
            myBullet.Root.Transform = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            worldMatrix = myBullet.Root.Transform;

            if (bulletFlag)
            {
                foreach (ModelMesh mesh in myBullet.Meshes) // Desenha o modelo
                {
                    Matrix world1;

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        world1 = effect.World;
                        effect.World = worldMatrix;
                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                        effect.EnableDefaultLighting();
                        if (bulletFlag == false)
                        {
                            effect.World = Matrix.Identity;
                        }
                    }
                    mesh.Draw();
                }
            }                       
        }
        
        public bool BulletFlag
        {
            get { return bulletFlag; }
            set { bulletFlag = value; }
        }

        public Vector3 Position
        {
            get { return position; }
        }

        public Matrix WorldMatrix
        {
            get { return worldMatrix; }
        }

        public Model BulletModel
        {
            get { return myBullet; }
        }
    }
}
