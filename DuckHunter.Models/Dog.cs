using DuckHunter.Models.Enums;

namespace DuckHunter.Models
{
    public class Dog
    {

        public float posX { get; set; }
        public float posY { get; set; }
        public int posTargetX;
        public int posTargetY;

        public bool isInBackround { get; set; }
        public bool isVisable { get; set; }

        public float animDuration { get; set; }
        public EnumDogState enumDogAnimState { get; set; }
    }
}