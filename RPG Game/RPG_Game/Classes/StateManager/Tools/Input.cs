namespace RPG_Game
{
    public class Input
    {
        public enum inputStates
        {
            pressed,
            held,
            released
        }

        public inputStates inputState = inputStates.released;

        public enum inputTypes
        {
            mouse,
            keyboard,
            scrollWheel
        }

        public inputTypes inputType = inputTypes.keyboard;
    }
}
