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
        protected Model myModel;
        protected Bullet bamB;
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
        Matrix[] boneTransforms;


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

        //Dustgen generator;
        

        public ClsTank(GraphicsDevice device, ContentManager content,Map map,/*Dustgen gen, */Vector2 newPlacement)
        {
            placement = newPlacement;            
            terrain = map;
            //this.generator = gen;

            yaw = 0;
            this.content = content;
            this.device = device;

            //generator = new Dustgen(device);

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
        }

        public void Update(/*int num,*/ GameTime gameTime)
        {
 
            lastPosition = position;

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

                //generator.Draw(view, projection);
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
 
