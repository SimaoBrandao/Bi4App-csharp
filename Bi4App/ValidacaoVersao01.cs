using System;
using System.Linq;

// Bi4App - biblioteca que interpreta o conteúdo do QR Code do Bilhete de Identidade de Angola e converte os dados em um objeto .NET tipado e normalizado — pronto para uso em aplicações web, desktop e mobile
// Autor: Simão Brandão
// Email: sibrandao2008@gmail.com 
// Telemovel: +244 948 49 38 28 
// Nuget:  https://www.nuget.org/packages/Bi4App
// Data: 09/01/2025 

namespace BiApp
{
    public class ValidacaoVersao01 : ValidacaoBase
    {
        private static readonly string[] SexosValidos =
        {
            "MASCULINO",
            "FEMININO"
        };

        private static readonly string[] EstadosCivisValidos =
        {
            "SOLTEIRO", "SOLTEIRA",
            "CASADO", "CASADA",
            "DIVORCIADO", "DIVORCIADA",
            "VIÚVO", "VIÚVA"
        };

        public void ValidarEstruturaVersao01(DadosBilheteIdentidade dados)
        {
            // -------- Número do BI --------
            if (!ValidarNumeroBilhete(dados.NumeroBilhete))
                throw new ArgumentException($"Número do Bilhete de Identidade inválido: {dados.NumeroBilhete}");

            // -------- Datas principais --------
            if (!ValidarData(dados.DataNascimento))
                throw new ArgumentException($"Data de nascimento inválida: {dados.DataNascimento}");

            if (!ValidarData(dados.DataEmissao))
                throw new ArgumentException($"Data de emissão inválida: {dados.DataEmissao}");

            // -------- Regras de datas --------
            var dataNascimento = ConverterDataParaDateTime(dados.DataNascimento);
            var dataEmissao = ConverterDataParaDateTime(dados.DataEmissao);

            if (dataNascimento.Value.Year < 1900)
                throw new ArgumentException($"Data de nascimento inferior a 1900: {dados.DataNascimento}");

            // -------- Cálculo de idade seguro --------
            var idade = CalcularIdade(dados.DataNascimento);

            var valorValidade = dados.DataValidade.Trim().ToUpperInvariant();

            // Vitalício antes de 56 → inválido
            if (valorValidade == "VITALÍCIO" && idade < 56)
                throw new ArgumentException("Documento vitalício permitido apenas para cidadãos com 56 anos ou mais.");

            // Se não for vitalício → validar ordem cronológica
            if (valorValidade != "VITALÍCIO")
            {
                if (!ValidarData(dados.DataValidade))
                    throw new ArgumentException($"Data de validade inválida: {dados.DataValidade}");

                var dataValidade = ConverterDataParaDateTime(dados.DataValidade);

                if (dataValidade <= dataEmissao)
                    throw new ArgumentException("A data de validade deve ser posterior à data de emissão.");
            }

            // -------- Sexo --------
            var sexoNormalizado = dados.Sexo.Trim().ToUpperInvariant();

            if (!SexosValidos.Contains(sexoNormalizado))
                throw new ArgumentException($"Sexo inválido: {dados.Sexo}");

            // -------- Estado Civil --------
            var estadoCivilNormalizado = dados.EstadoCivil.Trim().ToUpperInvariant();

            if (!EstadosCivisValidos.Contains(estadoCivilNormalizado))
                throw new ArgumentException($"Estado civil inválido: {dados.EstadoCivil}");
        }
    }
}
