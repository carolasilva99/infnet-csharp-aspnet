using System.ComponentModel.DataAnnotations;

namespace GerenciadorAniversarios.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        public Pessoa()
        {
        }

        public Pessoa(string nome, string sobrenome, DateTime dataNascimento)
        {
            Nome = nome.Trim();
            Sobrenome = sobrenome.Trim();
            DataNascimento = dataNascimento;
        }

        public string MontarNomeCompleto()
        {
            return $"{Nome} {Sobrenome}";
        }

        public int DiasParaProximoAniversario()
        {
            DateTime dataAtual = DateTime.Today;
            DateTime dataAniversario = DataNascimento.AddYears(dataAtual.Year - DataNascimento.Year);

            // Se o aniversário já passou
            if (dataAniversario < DateTime.Today)
                dataAniversario = dataAniversario.AddYears(1);

            int diasParaAniversario = (dataAniversario - dataAtual).Days;

            if (diasParaAniversario < 0)
                diasParaAniversario *= -1;

            return diasParaAniversario;
        }

        public bool NomeContem(string nome)
        {
            string nomeCompleto = MontarNomeCompleto();

            return nomeCompleto.ToLowerInvariant().Contains(nome.Trim().ToLowerInvariant());
        }
    }
}
