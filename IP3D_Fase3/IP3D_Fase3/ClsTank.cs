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
    class ClsTank
    {
       
        Model myModel;
        Matrix worldMatrix;
        Matrix View;
        Matrix Projection;
        Bullet bamB;
        public float scale;

        //bones
        ModelBone turretBone;
        ModelBone cannonBone;
        ModelBone rFrontWheel;
        ModelBone lFrontWheel;
        ModelBone lBackWheel;
        ModelBone rBackWheel;
        ModelBone rFrontSteer;
        ModelBone lFrontSteer;

        // Transformações iniciais
        // (posicionar torre e canhão)
        Matrix cannonTransform;
        Matrix turretTransform;
        Matrix rFrontwheelTransform;
        Matrix lFrontWheelTransform;
        Matrix lBackWheelTransform;
        Matrix rBackWheelTransform;
       // Guarda todas as transformações
        Matrix[] boneTransforms;

        Matrix rotation; // rotação do tank

        float height;
        float turretRotation;
        float cannonRotation;
        float wheelRotation;
        float yaw;
                
        Vector3 position;
        
        CameraSurfaceFollow camera;
        Map terrain;
        
        public Vector2 placement;
        Vector3 direction;
        DrawBoundingSphere boundingSphere;

        ContentManager content;
        GraphicsDevice device;

        public ClsTank(GraphicsDevice device, ContentManager content, CameraSurfaceFollow cam, Map map, Vector2 newPlacement)
        {
            placement = newPlacement;
            camera = cam;
            terrain = map;
            yaw = 0;
            this.content = content;
            this.device = device;
            direction = Vector3.Cross(Vector3.Forward, Vector3.Up);

            float aspectRatio = (float)device.Viewport.Width /
                                       device.Viewport.Height;

            height = terrain.SurfaceFollow(placement.X, placement.Y);
            position = new Vector3(placement.X, height, placement.Y);
            // bamB = new Bullet(device, content, cam, map, Position);
            scale = 0.001f;
            myModel = content.Load<Model>("tank");
            worldMatrix = cam.world;
            View = cam.view;
            Projection = cam.projection;
           
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

            boundingSphere = new DrawBoundingSphere(myModel, position, scale);         
        }

        public void Update(int num, GameTime gameTime)
        {
            switch (num)
            {
                case 1:
                    turretRotation += ((Keyboard.GetState().IsKeyDown(Keys.Right) ? 1 : 0) -
                              (Keyboard.GetState().IsKeyDown(Keys.Left) ? 1 : 0)) * -.2f;

                    cannonRotation += ((Keyboard.GetState().IsKeyDown(Keys.Up) ? 1 : 0) -
                                  (Keyboard.GetState().IsKeyDown(Keys.Down) ? 1 : 0)) * -.02f;
                    break;
                case 2:
                    turretRotation += ((Keyboard.GetState().IsKeyDown(Keys.O) ? 1 : 0) -
                              (Keyboard.GetState().IsKeyDown(Keys.U) ? 1 : 0)) * -.2f;

                    cannonRotation += ((Keyboard.GetState().IsKeyDown(Keys.Y) ? 1 : 0) -
                                  (Keyboard.GetState().IsKeyDown(Keys.H) ? 1 : 0)) * -.02f;
                    break;
            }

            float directionX = (float)Math.Sin(yaw);
            float directionZ = (float)Math.Cos(yaw);

            switch (num)
            {
                case 1:
                    //andar com o tank
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        position += new Vector3(directionX, terrain.SurfaceFollow(position.X, position.Z), directionZ) * .02f;
                        wheelRotation += .2f; //rotação das rodas
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        position -= new Vector3(directionX, terrain.SurfaceFollow(position.X, position.Z), directionZ) * .02f;
                        wheelRotation -= .2f;
                    }

                    //rodar o tanque
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                        yaw += .05f;

                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                        yaw -= .05f;
                    break;

                case 2:
                    //andar com o tank
                    if (Keyboard.GetState().IsKeyDown(Keys.I))
                    {
                        position += new Vector3(directionX, terrain.SurfaceFollow(position.X, position.Z), directionZ) * .02f;
                        wheelRotation += .2f; //rotação das rodas
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.K))
                    {
                        position -= new Vector3(directionX, terrain.SurfaceFollow(position.X, position.Z), directionZ) * .02f;
                        wheelRotation -= .2f;
                    }

                    //rodar o tanque
                    if (Keyboard.GetState().IsKeyDown(Keys.J))
                        yaw += .05f;

                    if (Keyboard.GetState().IsKeyDown(Keys.L))
                        yaw -= .05f;
                    break;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (bamB == null)
                    bamB = new Bullet(device, content, camera, terrain, Position);
                bamB.BulletFlag = true;
            }

            if(bamB != null && bamB.BulletFlag)
                bamB.bulletUpdate(gameTime, directionX, directionZ);

            position.Y = terrain.SurfaceFollow(position.X, position.Z);
            rotation = Matrix.Identity;
            rotation.Up = terrain.NormalFollow(position.X, position.Z);
            Vector3 horizontalDirection = Vector3.Transform(new Vector3(0, 0, -1), Matrix.CreateFromAxisAngle(rotation.Up, yaw));
            rotation.Right = Vector3.Normalize(Vector3.Cross(horizontalDirection, rotation.Up));
            rotation.Forward = Vector3.Normalize(Vector3.Cross(rotation.Up, rotation.Right));
        }

        public void Draw()
        {
        // Aplica as transformações em cascata por todos os bones       
            myModel.Root.Transform = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(position);
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
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }

            if (bamB!= null && bamB.BulletFlag) 
                bamB.Draw();
          

        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

    }
}
 
