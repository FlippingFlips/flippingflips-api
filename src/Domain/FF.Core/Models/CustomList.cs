using FF.Domain.Enum;

namespace FF.Core.Models
{
    /// <summary>
    /// Custom Friend List. Allows user to add friend or block or hide the player
    /// </summary>
    public class CustomList
    {
        public int Id { get; set; }
        public CustomListType Type { get; set; }
        public string ApplicationUserId { get; set; }
        public string ApplicationUserId2 { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
