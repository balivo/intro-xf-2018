using Newtonsoft.Json;
using SQLite;
using System;

namespace BuscaCep.Data.Dtos
{
    [Table("Cep")]
    sealed class CepDto
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string UF { get; set; }
        public string Unidade { get; set; }
        public string IBGE { get; set; }
        public string GIA { get; set; }
        public DateTime DataInclusao { get; set; } = DateTime.Now;
        public DateTime? DataAlteracao { get; set; }

        [Ignore]
        [JsonIgnore]
        public string Detalhes
        {
            get
            {
                var detalhes = Logradouro;

                if (!string.IsNullOrWhiteSpace(Bairro))
                    detalhes += $", {Bairro}";

                if (!string.IsNullOrWhiteSpace(Localidade) && !string.IsNullOrWhiteSpace(UF))
                    detalhes += $", {Localidade}/{UF}";

                return detalhes;
            }
        }
    }
}
