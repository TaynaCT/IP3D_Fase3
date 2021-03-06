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
    class ClsTank
    {
        protected Model myModel;
        protected Bullet bamB;
        protected Dustgen generator;
        public float scale;

        //bones
        protected ModelBone turretBone;
        protected ModelBone cannonBone;
        protected ModelBone rFrontWheel;
        protected ModelBone lFrontWheel;
        protected ModelBone lBackWheel;
        protected ModelBone rBackWheel;
        protected ModelBone rFrontSteer;
        protected ModelBone lFrontSteer;

        // Transformações iniciais
        // (posicionar torre e canhão)
        protected Matrix cannonTransform;
        protected Matrix turretTransform;
        protected Matrix rFrontwheelTransform;
        protected Matrix lFrontWheelTransform;
        protected Matrix lBackWheelTransform;
        protected Matrix rBackWheelTransform;
       // Guarda todas as transformações
        protected Matrix[] boneTransforms;
        
        protected Matrix rotation; // rotação do tank

        protected float height;
        protected float turretRotation;
        protected float cannonRotation;
        protected float wheelRotation;
        protected float yaw;

        protected Vector3 position, lastPosition;
        protected Vector3 target;

        protected Map terrain;

        protected Vector2 placement;
        protected Vector3 direction;

        protected ContentManager content;
        protected GraphicsDevice device;

        protected float directionX;
        protected float directionZ;
        
        public ClsTank(GraphicsDevice device, ContentManager content,Map map, Vector2 newPlacement)
        {
            placement = newPlacement;            
            terrain = map;
            
            yaw = 0;
            this.content = content;
            this.device = device;
            
            height = terrain.SurfaceFollow(placement.X, placement.Y);
            position = new Vector3(placement.X, height, placement.Y);          
            
            scale = 0.001f;
            myModel = content.Load<Model>("tank");
                                   
            // Lê os bones
            turretBone = myModel.Bones["turret_geo"];//torre
            cannonBone = myModel.Bones["canon_geo"];//canhão
            rFrontWheel = myModel.Bones["r_front_wheel_geo"];//roda direita frontal
            lFrontWheel = myModel.Bones["l_front_wheel_geo"];//roda esqueda frontal
            lBackWheel = myModel.Bones["l_back_wheel_geo"];//roda esquerda traseira
            rBackWheel = myModel.Bones["r_back_wheel_geo"];//roda direita traseira
            rFrontSteer = myModel.Bones["r_steer_geo"];//eixo da roda direita dianteira 
            lFrontSteer = myModel.Bones["l_steer_geo"];//eixo da roda esquerda dianteira

            // Lê as transformações iniciais dos bones            
            turretTransform = turretBone.Transform;
            cannonTransform = cannonBone.Transform;
            rFrontwheelTransform = rFrontWheel.Transform;
            lFrontWheelTransform = lFrontWheel.Transform;
            lBackWheelTransform = lBackWheel.Transform;
            rBackWheelTransform = rBackWheel.Transform;

            // cria o array que armazenará as transformações em cascata dos bones
            boneTransforms = new Matrix[myModel.Bones.Count];

            turretRotation = 1f;
            cannonRotation = .2f;
            wheelRotation = .5f;

            generator = new Dustgen(device);
            directionX = (float)Math.Sin(yaw);
            directionZ = (float)Math.Cos(yaw);
        }

        public void Update(int num, GameTime gameTime, Vector3 targetPos)
        {
            lastPosition = position;

            switch (num)
            {
                case 1:
                    Player1(gameTime);
                    break;

                case 2:
                    player2(gameTime, 2, targetPos);
                    break;
                    
            }

            directionX = (float)Math.Sin(yaw);
            directionZ = (float)Math.Cos(yaw);
                   

            //põe o tank em cima do terreno
            position.Y = terrain.SurfaceFollow(position.X, position.Z);

            //direciona o tank
            rotation = Matrix.Identity;
            rotation.Up = terrain.NormalFollow(position.X, position.Z);
            //Vector3 horizontalDirection = Vector3.Transform(new Vector3(0, 0, -1), Matrix.CreateFromAxisAngle(rotation.Up, yaw));
            Vector3 horizontalDirection = new Vector3(directionX, 0, directionZ);
            rotation.Right = Vector3.Normalize(Vector3.Cross(horizontalDirection, rotation.Up));
            rotation.Forward = Vector3.Normalize(Vector3.Cross(rotation.Up, rotation.Right));
                       
            target = rotation.Forward;
           
            //caso a bala saia dos limites do terreno 
            if (bamB != null && bamB.BulletFlag)
                if ((bamB.Position.X < 0 || bamB.Position.X > terrain.MapLimit) || (bamB.Position.Z < 0 || bamB.Position.Z > terrain.MapLimit))
                    bamB = null;
        }
        
        public void Draw(Matrix view, Matrix projection)
        {
            // Aplica as transformações em cascata por todos os bones       
            myModel.Root.Transform = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.Pi) * rotation * Matrix.CreateTranslation(position);
            turretBone.Transform = Matrix.CreateRotationY(turretRotation) * turretTransform;
            cannonBone.Transform = Matrix.CreateRotationX(cannonRotation) * cannonTransform;
            rFrontWheel.Transform = Matrix.CreateRotationX(wheelRotation) * rFrontwheelTransform;
            lFrontWheel.Transform = Matrix.CreateRotationX(wheelRotation) * lFrontWheelTransform;
            lBackWheel.Transform = Matrix.CreateRotationX(wheelRotation) * lBackWheelTransform;
            rBackWheel.Transform = Matrix.CreateRotationX(wheelRotation) * rBackWheelTransform;
            
            myModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in myModel.Meshes) // Desenha o modelo
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();                    
                }
                mesh.Draw();
            }

            if (bamB!= null && bamB.BulletFlag) 
                bamB.Draw(view, projection);

                generator.Draw(view, projection);
        }


        protected void Player1(GameTime gameTime)
        {
            turretRotation += ((Keyboard.GetState().IsKeyDown(Keys.Right) ? 1 : 0) -
                              (Keyboard.GetState().IsKeyDown(Keys.Left) ? 1 : 0)) * -.02f;

            cannonRotation += ((Keyboard.GetState().IsKeyDown(Keys.Up) ? 1 : 0) -
                          (Keyboard.GetState().IsKeyDown(Keys.Down) ? 1 : 0)) * -.02f;
            
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position += new Vector3(directionX, 0, directionZ) * .02f;
                wheelRotation += .2f; //rotação das rodas
                position.Y = terrain.SurfaceFollow(position.X, position.Z);
                generator.Ciclo(gameTime, rotation, position);
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position -= new Vector3(directionX, 0, directionZ) * .02f;
                position.Y = terrain.SurfaceFollow(position.X, position.Z);
                wheelRotation -= .2f;
                generator.Ciclo(gameTime, rotation, position);
            }

            //rodar o tanque
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                yaw += .05f;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                yaw -= .05f;
           
            //BULLET 
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (bamB == null)
                    bamB = new Bullet(device, content, terrain, Position + new Vector3(0, 0.3f, 0), Target);
                bamB.BulletFlag = true;

            }

            if (bamB != null && bamB.BulletFlag)
                bamB.bulletUpdate(gameTime);
        }

        protected void player2(GameTime gameTime, int mode, Vector3 targetPos)
        {
            switch (mode) {
                case 1:
                    turretRotation += ((Keyboard.GetState().IsKeyDown(Keys.O) ? 1 : 0) -
                                     (Keyboard.GetState().IsKeyDown(Keys.U) ? 1 : 0)) * -.02f;

                    cannonRotation += ((Keyboard.GetState().IsKeyDown(Keys.Y) ? 1 : 0) -
                                  (Keyboard.GetState().IsKeyDown(Keys.H) ? 1 : 0)) * -.02f;

                    //andar com o tank
                    if (Keyboard.GetState().IsKeyDown(Keys.I))
                    {
                        position += new Vector3(directionX, 0, directionZ) * .02f;
                        position.Y = terrain.SurfaceFollow(position.X, position.Z);
                        wheelRotation += .2f; //rotação das rodas
                        generator.Ciclo(gameTime, rotation, position);
                        wheelRotation += .2f;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.K))
                    {
                        position -= new Vector3(directionX, 0, directionZ) * .02f;
                        position.Y = terrain.SurfaceFollow(position.X, position.Z);
                        wheelRotation -= .2f;
                        generator.Ciclo(gameTime, rotation, position);
                        wheelRotation -= .2f;
                    }

                    //rodar o tanque
                    if (Keyboard.GetState().IsKeyDown(Keys.J))
                        yaw += .05f;

                    if (Keyboard.GetState().IsKeyDown(Keys.L))
                        yaw -= .05f;
                    break;

                case 2:

                    float searchRadius = 10;
                    
                    position += rotation.Forward * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    position.Y = terrain.SurfaceFollow(position.X, position.Z);

                    if ((position.X > terrain.MapLimit - 10 || position.Z > terrain.MapLimit - 10) || (position.X < 10 || position.Z < 10))
                        yaw -= MathHelper.Pi / 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    //if (targetPos.X < searchRadius || targetPos.Z < searchRadius) {

                    //    float distanceX = position.X - targetPos.X;
                    //    float distanceZ = position.Z - targetPos.Z;
                        
                    //    float heightX = (distanceX/2)

                    //}
                        Random rand = new Random();
                        float distance = (float)rand.NextDouble() * 5 /*radius*/;
                        float angle = (float)rand.NextDouble() * MathHelper.TwoPi;

                        yaw += distance * (float)Math.Cos(angle) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        yaw -= distance * (float)Math.Sin(angle) * (float)(float)gameTime.ElapsedGameTime.TotalSeconds;
                        
                    
                    wheelRotation += .2f;
                    break;
        }
            //BULLET 
            if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
            {
                if (bamB == null)
                    bamB = new Bullet(device, content, terrain, Position + new Vector3(0, 0.3f, 0), Target);
                bamB.BulletFlag = true;
            }

            if (bamB != null && bamB.BulletFlag)
                bamB.bulletUpdate(gameTime);


        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }        

        public Vector3 Target
        {
            get { return rotation.Forward; }
        }

        public Matrix Rotation
        {
            get { return rotation; }
        }
        
        public Model tankModel
        {
            get { return myModel; }
        }       
        public Vector3 LasPosition
        {
            get { return lastPosition; }
        }
                
        public Matrix[] BoneTransforms
        {
            get { return boneTransforms; }
        }

        public Bullet GetSetBullet
        {
            get { return bamB; }
            set { bamB = value; }
        }
    }
}
 
