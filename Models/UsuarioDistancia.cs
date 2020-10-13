namespace Hermes.Models
{
    public class UsuarioDistancia
    {
        public double Distancia { get; set; }
        public long UsuarioId { get; set; }

        public UsuarioDistancia(double distancia, long usuarioId)
        {
            this.Distancia = distancia;
            this.UsuarioId = usuarioId;
        }
    }
}