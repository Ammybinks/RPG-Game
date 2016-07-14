namespace RPG_Game
{
    class Input
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
            keyboard
        }

        public inputTypes inputType = inputTypes.keyboard;
    }
}
