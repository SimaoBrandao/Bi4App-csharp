using System;
using System.Globalization;
using System.Text.RegularExpressions;

// Bi4App - biblioteca que interpreta o conteúdo do QR Code do Bilhete de Identidade de Angola e converte os dados em um objeto .NET tipado e normalizado — pronto para uso em aplicações web, desktop e mobile
// Autor: Simão Brandão
// Email: sibrandao2008@gmail.com 
// Telemovel: +244 948 49 38 28 
// Nuget:  https://www.nuget.org/packages/Bi4App
// Data: 09/01/2025 

namespace BiApp
{
    public class ValidacaoBase
    {
        public bool ValidarNumeroBilhete(string numeroBilhete)
        {
            return Regex.IsMatch(
                numeroBilhete.ToUpperInvariant(),
                @"^\d{9}[A-Z]{2}\d{3}$"
            );
        }

        public bool ValidarData(string data, bool permitirVitalicio = false)
        {
            if (permitirVitalicio && data.Trim().ToUpperInvariant() == "VITALÍCIO")
                return true;

            return ConverterDataParaDateTime(data) != null;
        }

        public DateTime? ConverterDataParaDateTime(string data)
        {
            string[] formatos =
            {
                "dd-MM-yyyy",
                "yyyy-MM-dd",
                "dd/MM/yyyy",
                "yyyy/MM/dd"
            };

            data = data.Replace("/", "-");

            foreach (var formato in formatos)
            {
                if (DateTime.TryParseExact(
                        data,
                        formato,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var resultado))
                {
                    return resultado;
                }
            }

            return null;
        }

        public int CalcularIdade(string dataNascimento)
        {
            var nascimento = ConverterDataParaDateTime(dataNascimento);
            if (nascimento == null)
                return 0;

            var hoje = DateTime.Today;
            var idade = hoje.Year - nascimento.Value.Year;

            if (nascimento.Value.Date > hoje.AddYears(-idade))
                idade--;

            return idade;
        }

        public string CalcularDiasAteExpiracao(string dataValidade)
        {
            var valor = dataValidade.Trim().ToUpperInvariant();

            if (valor == "VITALÍCIO")
                return "Validade vitalícia";

            var validade = ConverterDataParaDateTime(dataValidade);
            if (validade == null)
                return "Data inválida";

            var hoje = DateTime.Today;

            var diferenca = validade.Value >= hoje
                ? validade.Value - hoje
                : hoje - validade.Value;

            var prefixo = validade.Value >= hoje
                ? "Faltam"
                : "Venceu há";

            if (diferenca.TotalDays < 31)
                return $"{prefixo} {diferenca.Days} dia(s)";

            int anos = (int)(diferenca.TotalDays / 365);
            int meses = (int)((diferenca.TotalDays % 365) / 30);

            return $"{prefixo} {anos} ano(s) e {meses} mês(es)";
        }

        public string FaixaEtaria(int idade)
        {
            if (idade < 13) return "Criança";
            if (idade < 18) return "Adolescente";
            if (idade < 45) return "Jovem adulto";
            if (idade < 60) return "Adulto";
            return "Idoso";
        }

        public string NormalizarNomeCultural(string texto)
        {
            return CultureInfo
                .CurrentCulture
                .TextInfo
                .ToTitleCase(texto.ToLowerInvariant());
        }

        public bool EhBase64(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return false;

            texto = texto.Trim();

            // comprimento tem que ser múltiplo de 4
            if (texto.Length % 4 != 0)
                return false;

            return Regex.IsMatch(
                texto,
                @"^[A-Za-z0-9+/]*={0,2}$"
            );
        }

        public string EstadoValidade(string dataValidade)
        {
            var valor = dataValidade.Trim().ToUpperInvariant();

            if (valor == "VITALÍCIO")
                return "Vitalício";

            var validade = ConverterDataParaDateTime(dataValidade);
            if (validade == null)
                return "Data inválida";

            return validade.Value >= DateTime.Today
                ? "Válido"
                : "Vencido";
        }
    }
}
