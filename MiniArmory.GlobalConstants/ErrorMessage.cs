namespace MiniArmory.GlobalConstants
{
    public class ErrorMessage
    {
        //C
        public const string COOLDOWN_FIELD = "Must have {0} between {1} and {2} seconds.";

        //D
        public const string DROPDOWN = "You must select an option different than the default one.";

        //I
        public const string IMAGE_GENERAL_MESSAGE = "Must have an image. Please post a link.";

        public const string IMAGE_REGEX = @"^(https://)(www)?(render-(eu)?)?(render)?(wow)?(assets)?(images)?(imgur)?.?(deviantart)?(com)?(worldofwarcraft)?(blz-contentstack)?(zamimg)?(.)?(/)?(.)?(com)/(images)?/?(wow)?/?(icons)?.*$";

        public const string INVALID_IMAGE = "Only images from deviantart, wow armory, wowhead, and imgur are allowed.";

        //N
        public const string NUMBERS_FIELD = "Must have an amount of {0} between {1} and {2}.";

        //R
        public const string RANGE_FIELD = "Must have a {0} between {1} and {2} yards.";

        //S
        public const string SELECT = "You must select a {0}.";

        //T
        public const string TEXT_FIELD = "Must have a {0} between {2} and {1} characters.";

        public const string TEXT_FIELD_REGEX = "^[\"\\d\"]+$";

        public const string TYPE_REGEX = "^[A-Z{1}a-z]+$";
    }
}
