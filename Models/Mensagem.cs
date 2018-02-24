using System;

namespace SimpleBot.Models
{
    public class Mensagem
    {
        public string Usuario { get; set; }
        public string Texto { get; set; }
        public DateTime Data { get; set; }

        public override string ToString()
        {
            return $"Usuario: {this.Usuario}, Texto: {this.Texto}, Data: {this.Data.ToLongTimeString()}";
        }
    }
}