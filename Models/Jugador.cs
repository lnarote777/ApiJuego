namespace ApiJuegoPsp.Models;

 public class Jugador
    {
        public long Id { get; set; } 
        public required string Name { get; set; } 
        public int Coins { get; set; } 
        public int Points { get; set; } 
        public DateTime UltimaConexion { get; set; } 
    }