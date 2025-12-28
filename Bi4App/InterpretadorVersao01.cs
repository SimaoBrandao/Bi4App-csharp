using System;
using System.Collections.Generic;
using System.Linq;

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
    internal class InterpretadorVersao01
    {
        public DadosBilheteIdentidade Interpretar(List<string> linhasProcessadas)
        {
            try
            {
                var dados = new DadosBilheteIdentidade();
                var validacao = new ValidacaoVersao01();

                int indiceBase = linhasProcessadas.Count == 11 ? 1 : 0;

                dados.NomeCompleto =
                    linhasProcessadas.Count == 11
                    ? $"{linhasProcessadas[0]} {linhasProcessadas[1]}"
                    : linhasProcessadas[0];

                dados.NumeroBilhete = linhasProcessadas[indiceBase + 1].ToUpperInvariant();
                dados.Provincia = validacao.NormalizarNomeCultural(linhasProcessadas[indiceBase + 2]);
                dados.DataNascimento = linhasProcessadas[indiceBase + 3];
                dados.Sexo = validacao.NormalizarNomeCultural(linhasProcessadas[indiceBase + 4]);
                dados.EstadoCivil = validacao.NormalizarNomeCultural(linhasProcessadas[indiceBase + 5]);
                dados.DataEmissao = linhasProcessadas[indiceBase + 6];
                dados.DataValidade = linhasProcessadas[indiceBase + 7];
                dados.ProvinciaEmissora = validacao.NormalizarNomeCultural(linhasProcessadas[indiceBase + 8]);
                dados.Versao = linhasProcessadas[indiceBase + 9].ToUpperInvariant();

                var partesNome = dados.NomeCompleto.Split(' ');
                dados.PrimeiroNome = partesNome.FirstOrDefault() ?? "";
                dados.UltimoNome = partesNome.LastOrDefault() ?? "";

                // Validação forte → lança exceção se inválido
                validacao.ValidarEstruturaVersao01(dados);

                // Campos derivados
                PreencherCamposDerivados(dados, validacao);

                return dados;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro ao interpretar dados do bilhete: " + ex.Message);
            }
        }

        private void PreencherCamposDerivados(DadosBilheteIdentidade dados, ValidacaoVersao01 validacao)
        {
            dados.Idade = validacao.CalcularIdade(dados.DataNascimento);
            dados.FaixaEtaria = validacao.FaixaEtaria(dados.Idade);
            dados.EstadoValidade = validacao.EstadoValidade(dados.DataValidade);
            dados.DiasAteExpiracao = validacao.CalcularDiasAteExpiracao(dados.DataValidade);
        }
    }
}
