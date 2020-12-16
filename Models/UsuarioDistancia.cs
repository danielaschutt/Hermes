namespace Hermes.Models
{
    public class UsuarioDistancia
    {
        public double Distancia { get; set; }
        public long UsuarioId { get; set; }
        public string Token { get; set; }

        public UsuarioDistancia(double distancia, long usuarioId, string token)
        {
            this.Distancia = distancia;
            this.UsuarioId = usuarioId;
            this.Token = token;
        }
    }
}