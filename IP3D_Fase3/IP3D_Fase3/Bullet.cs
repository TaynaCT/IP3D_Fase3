﻿using Microsoft.Xna.Framework;
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
        Matrix View;
        Matrix Projection;
       
        Map terrain;
        public Vector2 placement;
        Vector3 position, inicialPos;
        Vector3 speed, aceleration;
        public float scale;
        public float height;
        public float yaw = 0;
        bool bulletFlag;

        CameraSurfaceFollow camera;

        public Bullet(GraphicsDevice device, ContentManager content, CameraSurfaceFollow cam, Map map, Vector3 pos)
        {
            camera = cam;
            terrain = map;
            myBullet = content.Load<Model>("Cube");
            bulletFlag = false;

            height = terrain.SurfaceFollow(placement.X, placement.Y);
            position = pos;
            inicialPos = position;
            scale = 0.09f;

            worldMatrix = camera.world;
            View = camera.view;
            Projection = camera.projection;
        }
        
        public void bulletUpdate(GameTime gameTime,float x, float z)
        {
            float gravity = 0;
            if (bulletFlag)
            {
                gravity -= 0.3f;
                position += new Vector3(x, gravity, z) * 0.04f;
                Trajectory(gameTime);
            }                     
        }

        /// <summary>
        /// Calcula a trajetoria o projetil a ser disparado
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>Posição final do projetil</returns>
        public Vector3 Trajectory(GameTime gameTime)
        {
            float timePassed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //x = xo + vo t + ½ a t2 
            Vector3 finalPos = inicialPos + speed * timePassed + aceleration * 1 / 2 * (float)Math.Pow(timePassed, 2);

            return finalPos;
        }

        public void Draw()
        {
            if (bulletFlag)
            {
                foreach (ModelMesh mesh in myBullet.Meshes) // Desenha o modelo
                {
                    Matrix world1;

                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        world1 = effect.World;
                        effect.World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
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
    }
}
