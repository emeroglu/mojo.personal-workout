using Assets.Scripts.Abstract.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Abstract.UI;
using Assets.Scripts.Abstract.Json;

namespace Assets.Scripts.Repository
{
    public static class Variables
    {
        public static Action<object> nextTask;

        public static string Current_Page;
        public static string Exception_Message;

        public static JsonMojo Current_Mojo;

        public static int Switch_Inc;   
        public static int Image_Cache_Inc;        

        public static string Keyboard_Text;

        public static Vector2 Touch_Position_Start;
        public static Vector2 Touch_Position_End;

        public static float Touch_Position_Delta_X;        

        public static float Swipe_Horizontal;
        public static float Swipe_Left;
        public static float Swipe_Right;

        public static UIDictionary<string, UIComponent> UI;        
        
        public static JsonSelf Self;
        public static string App_Instance_Key;        

        public static string Notification_Type;
        
        public static int Friend_Request_Count;                           		
    }
}