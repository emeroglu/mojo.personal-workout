using UnityEngine;

namespace Assets.Scripts.Repository
{
    public static class Config
    {
        public static class URLs
        {
            //private static string prefix_API = "http://localhost:23386/Mojo";
            private static string prefix_API = "https://testapi.letsmojoapp.com/Mojo";
            private static string prefix_REPO = "https://testrepo.letsmojoapp.com";

            private static string platform = (Application.platform == RuntimePlatform.Android) ? "/android" : (Application.platform == RuntimePlatform.IPhonePlayer) ? "/ios" : "/android";

            public static string Animator = prefix_REPO + platform + "_animator_{0}.zip";
            public static string Avatar = prefix_REPO + platform + "_character_{0}.zip";
            public static string Music = prefix_REPO + "/soundtrack_{0}.zip";
            public static string Skyboxes = prefix_REPO + platform + "_skybox_{0}.zip";
            public static string Environments = prefix_REPO + platform + "_environment_{0}.zip";

            public static string Instance_Termination = prefix_API + "/Instance_Termination";
            public static string Logout = prefix_API + "/Logout";
            public static string App_Instance = prefix_API + "/App_Instance";

            public static string Remove_Friend = prefix_API + "/Remove_Friend";
            public static string Approve = prefix_API + "/Approve";
            public static string Friend_Request = prefix_API + "/Friend_Request";
            public static string Friend_Search = prefix_API + "/Friend_Search";
            public static string Foreign_Search = prefix_API + "/Foreign_Search";
            public static string Send = prefix_API + "/Send";

            public static string Mojo_Viewed = prefix_API + "/Mojo_Viewed";
            public static string Mojo_Previewed = prefix_API + "/Mojo_Previewed";

            public static string Friends_Page = prefix_API + "/Friends_Page";
            public static string Friend_Requests_Page = prefix_API + "/Friend_Requests_Page";

            public static string Inbox_Page = prefix_API + "/Inbox_Page";
            public static string Profile_Page = prefix_API + "/Profile_Page";
            public static string Main_Page = prefix_API + "/Main_Page";
            public static string Characters_Page = prefix_API + "/Characters_Page";
            public static string Soundtracks_Page = prefix_API + "/Soundtracks_Page";
        }

        public static class Directories
        {
            public static string Root = Application.persistentDataPath + "/Root";

            public static string Version = Root + "/version.mojo";
            public static string Identity = Root + "/identity.mojo";

            public static string Image = Root + "/Images/image_{0}{1}";
            public static string Images = Root + "/Images";
            public static string Images_Cache_File = Root + "/Images/cache.txt";

            public static string Animator = Root + "/Animators/Animator_{0}/animator_{0}.unity3d";
            public static string Animator_Extract = Root + "/Animators/Animator_{0}";
            public static string Animators_Folder = Root + "/Animators";
            public static string Animators_Cache_File = Root + "/Animators/cache.txt";

            public static string Character = Root + "/Characters/Character_{0}/character_{0}.unity3d";
            public static string Character_Extract = Root + "/Characters/Character_{0}";
            public static string Characters_Folder = Root + "/Characters";
            public static string Characters_Cache_File = Root + "/Characters/cache.txt";

            public static string Soundtrack = Root + "/Soundtracks/Soundtrack_{0}/soundtrack_{0}.mp3";
            public static string Soundtrack_Extract = Root + "/Soundtracks/Soundtrack_{0}";
            public static string Soundtracks_Folder = Root + "/Soundtracks";
            public static string Soundtracks_Cache_File = Root + "/Soundtracks/cache.txt";

            public static string Skybox = Root + "/Skyboxes/Skybox_{0}/skybox_{0}.unity3d";
            public static string Skybox_Extract = Root + "/Skyboxes/Skybox_{0}";
            public static string Skyboxes_Folder = Root + "/Skyboxes";
            public static string Skyboxes_Cache_File = Root + "/Skyboxes/cache.txt";

            public static string Environment = Root + "/Environments/Environment_{0}/environment_{0}.unity3d";
            public static string Environment_Extract = Root + "/Environments/Environment_{0}";
            public static string Environments_Folder = Root + "/Environments";
            public static string Environments_Cache_File = Root + "/Environments/cache.txt";
        }

        public static class Variables
        {
            public static int App_Version = 1;

            public static string Date_Format = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
