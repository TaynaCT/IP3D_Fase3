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
        Vector3 speed, velocity;
        public float scale;
       
        public float yaw = 0;
        bool bulletFlag;
                
        CameraSurfaceFollow camera;
        
        public Bullet(GraphicsDevice device, ContentManager content, CameraSurfaceFollow cam, Map map, Vector3 pos, Vector3 direction)
        {
            camera = cam;
            terrain = map;
            myBullet = content.Load<Model>("Cube");
            bulletFlag = false;
                       
            position = pos;
            inicialPos = position;
            scale = 0.025f;
            velocity = direction;
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
        public void bulletUpdate(GameTime gameTime)
        {
            float timePassed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector3 speed = new Vector3(.5f, -9.8f, .5f);            
            //Vector3 velocity = Vector3.Zero;
            //x = xo + vo t + ½ a t2 
            if (bulletFlag)
            {
                position += velocity * timePassed;
            }
            
        }      

        public void Draw()
        {
            myBullet.Root.Transform = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            worldMatrix = myBullet.Root.Transform;

            //if (bulletFlag)
            //{
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
            //}                       
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
