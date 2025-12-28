// Bi4App - biblioteca que interpreta o conteúdo do QR Code do Bilhete de Identidade de Angola e converte os dados em um objeto .NET tipado e normalizado — pronto para uso em aplicações web, desktop e mobile
// Autor: Simão Brandão
// Email: sibrandao2008@gmail.com 
// Telemovel: +244 948 493 828 
// Linkedin: https://linkedin.com/in/SimaoBrandao 
// Nuget:  https://www.nuget.org/packages/Bi4App
// Github: https://github.com/SimaoBrandao/Bi4App-csharp
// Data: 09/01/2025  

namespace BiApp
{
    public class DadosBilheteIdentidade
    {
        // Dados principais do titular
        public string NomeCompleto { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }

        // Identificação
        public string NumeroBilhete { get; set; }
        public string Versao { get; set; }

        // Naturalidade
        public string Provincia { get; set; }

        // Dados pessoais
        public string DataNascimento { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }

        // Emissão do documento
        public string ProvinciaEmissora { get; set; }
        public string DataEmissao { get; set; }
        public string DataValidade { get; set; }

        // Campos derivados (calculados)
        public int Idade { get; set; }
        public string FaixaEtaria { get; set; }
        public string EstadoValidade { get; set; }
        public string DiasAteExpiracao { get; set; }
    }
}

