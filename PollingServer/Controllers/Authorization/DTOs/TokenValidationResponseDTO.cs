namespace PollingServer.Controllers.Authorization.DTOs
{
    public class TokenValidationResponseDTO
    {
        public int Id { get; protected set; }
        public string Nickname { get; protected set; }

        public TokenValidationResponseDTO(int id, string nickname)
        {
            Id = id;
            Nickname = nickname;
        }
    }
}
