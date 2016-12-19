using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace IP3D_Fase3
{
    
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        //cameras
        CameraThirdPerson cameraTP;        
        CameraFree cameraF;
        CameraSurfaceFollow cameraSF;
        int selectCam;

        Matrix view, projection;
        //mapa
        Map mapa;

        //tanks       
        Player1 tank;
        Enemy tank2;
        //particula
             
        //Dustgen gen;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1800;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here           
            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            cameraTP = new CameraThirdPerson(GraphicsDevice);            
            cameraF = new CameraFree(GraphicsDevice);
            cameraSF = new CameraSurfaceFollow(GraphicsDevice);
            selectCam = 1;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mapa = new Map(GraphicsDevice, Content, cameraF.effect);

            tank = new Player1(GraphicsDevice, Content, mapa, new Vector2(10, 10));
            tank2 = new Enemy(GraphicsDevice, Content, mapa, new Vector2(10, 6));
                             
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            CamSelection(gameTime);

                       
            tank.Update(gameTime);
            tank2.Update(gameTime);
            //gen.ciclo();
            
            if (Collisions.TankCollision(tank.tankModel, tank.BoneTransforms, tank2.tankModel, tank2.BoneTransforms))
            {
                tank.Position = tank.LasPosition;
                tank2.Position = tank2.LasPosition;
            }

           // COLISÕES
            Console.WriteLine(Collisions.TankCollision(tank.tankModel, tank.BoneTransforms,
                tank2.tankModel, tank2.BoneTransforms));

            if (tank2.GetSetBullet != null && tank2.GetSetBullet.BulletFlag)
            {
                if (Collisions.BulletCollision(tank.tankModel, tank.BoneTransforms, tank2.GetSetBullet.BulletModel, tank2.GetSetBullet.WorldMatrix) ||
                     tank2.GetSetBullet.Position.Y < mapa.SurfaceFollow(tank2.GetSetBullet.Position.X, tank2.GetSetBullet.Position.Z))
                {
                    tank2.GetSetBullet.BulletFlag = false;
                    tank2.GetSetBullet = null;
                }
            }
            if (tank.GetSetBullet != null && tank.GetSetBullet.BulletFlag)
            {
                if (Collisions.BulletCollision(tank2.tankModel, tank2.BoneTransforms, tank.GetSetBullet.BulletModel, tank.GetSetBullet.WorldMatrix) ||
                    tank.GetSetBullet.Position.Y < mapa.SurfaceFollow(tank.GetSetBullet.Position.X, tank.GetSetBullet.Position.Z))
                {
                    tank.GetSetBullet.BulletFlag = false;
                    tank.GetSetBullet = null;
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            mapa.Draw(GraphicsDevice, view, projection);
            //gen.Draw(view, projection);
            tank.Draw(view, projection);
            tank2.Draw(view, projection);
                  

             // TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }

        public void CamSelection(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                selectCam = 1;
            else if(Keyboard.GetState().IsKeyDown(Keys.F2))
                    selectCam = 2;
            else if(Keyboard.GetState().IsKeyDown(Keys.F3))
                selectCam = 3;

            switch (selectCam)
            {
                case 1:
                    cameraTP.Update(tank.Rotation, tank.Position);
                    view = cameraTP.View();
                    projection = cameraTP.projection;
                    
                    break;
                case 2:
                    cameraSF.Update(new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), (float)gameTime.ElapsedGameTime.TotalSeconds, mapa.SurfaceFollow(cameraSF.position.X, cameraSF.position.Z));
                    view = cameraSF.View();
                    projection = cameraSF.projection;
                    break;
                case 3:
                    cameraF.Update(new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), (float)gameTime.ElapsedGameTime.TotalSeconds);
                    view = cameraF.View();
                    projection = cameraF.projection;
                    break;
            }

        }
    }
}

