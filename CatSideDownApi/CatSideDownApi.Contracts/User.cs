using System.Runtime.Serialization;

namespace CatSideDownApi.Contracts
{
    [DataContract]
    public class User
    {
        public int Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        [DataMember(IsRequired = true)]
        public string Password { get; set; }
    }
}
