using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class MenuCamera
    {
        GraphicsDevice device;
        Matrix projection;
        Matrix view;
        Matrix world;
        Vector3 position;
        BasicEffect effect;

        CameraFree freeCam;
        CameraSurfaceFollow sfCam;
        CameraThirdPerson tpCam;

        int option;

        public MenuCamera(GraphicsDevice device, Vector3 pos)
        {
            this.device = device;
            position = pos;
            option = 1;
            Select();
            SwitchOption();
        }

        public MenuCamera(GraphicsDevice device)
        {
            this.device = device;
            
            option = 1;
            Select();
            SwitchOption();
        }

        //Seleciona o valor de options apartir do input das teclas
        public void Select()
        {
            //F1 - seleciona camera 3° pessoa
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                option = 1;                
            }
            //F2 - seleciona camera com surface follow
            else if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                option = 2;                
            }
            //F3 - seleciona camera livre 
            else if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                option = 3;                
            }
        }

        //seleciona a camera de qacordo com o valor de option
        public void SwitchOption()
        {
            switch (option)
            {
                case 1:
                    if(tpCam == null)
                        tpCam = new CameraThirdPerson(device, position);
                    
                    projection = tpCam.projection;
                    view = tpCam.view;
                    world = tpCam.world;
                    effect = tpCam.effect;

                    sfCam = null;
                    freeCam = null;
                break;

                case 2:
                    if(sfCam == null)
                        sfCam = new CameraSurfaceFollow(device);

                    projection = sfCam.projection;
                    view = sfCam.view;
                    world = sfCam.world;
                    effect = sfCam.effect;

                    tpCam = null;
                    freeCam = null;
                break;

                case 3:
                    if(freeCam == null)
                        freeCam = new CameraFree(device);
                    
                    projection = freeCam.projection;
                    view = freeCam.view;
                    world = freeCam.world;
                    effect = freeCam.effect;

                    tpCam = null;
                    freeCam = null;
                break;
            }
        }
        
        public void Update(Point centre, float timePassed, float surfaceFollow, Vector3 target)
        {
            switch (option)
            {
                case 1:
                    tpCam.Update(position, target);
                    break;
                case 2:
                    sfCam.Update(centre, timePassed, surfaceFollow);
                    break;
                case 3:
                    freeCam.Update(centre, timePassed);
                    break;
            }
        }

        public Matrix Projection
        {
            get { return projection; }
        }

        public Matrix View
        {
            get { return view; }
        }

        public Matrix World
        {
            get { return world; }
        }

        public BasicEffect Effect
        {
            get { return effect; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }


    }
}
