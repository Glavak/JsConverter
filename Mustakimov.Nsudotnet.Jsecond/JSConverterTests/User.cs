namespace JSConverterTests
{
    internal class User
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public decimal? Balance { get; set; }
        public Gender Gender { get; set; }
    }

    internal static class UserWeirdExtensions
    {
        public static int Where(this User user)
        {
            return user.Age;
        }
    }
}