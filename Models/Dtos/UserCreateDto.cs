namespace agendamento_coordenacao.Models.Dtos
{
    public class UserCreateDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int TypeUser { get; set; }
    }
}