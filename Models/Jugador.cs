using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiJuegoPsp.Models;

 public class Jugador
    {
        [BsonId]
        public long Id { get; set; } 
        [BsonRequired]
        public required string Name { get; set; } 
        public int Coins { get; set; } 
        public int Points { get; set; } 
        public DateTime UltimaConexion { get; set; } 
    }