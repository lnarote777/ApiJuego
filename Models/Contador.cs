using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiJuegoPsp.Models
{
    public class Contador
    {
        [BsonId]
        public string Id { get; set; } = "jugador";
        public long Secuencia { get; set; } = 0; //Último id usado
    }
}