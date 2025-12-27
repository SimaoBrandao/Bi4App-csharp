using System;
using System.Collections.Generic;
using System.Linq;

// Biblioteca responsável por interpretar o conteúdo do QR Code
// do Bilhete de Identidade de Angola e converter os dados para
// um objeto tipado e normalizado para uso em aplicações .NET.
//
// Autor: Simão Brandão
// Email: sibrandao2008@gmail.com 
// Telefone: +244 948 49 38 28 
// Nuget: https://www.nuget.org/packages/Bi4App
// Data: 09/01/2025 

namespace BiApp
{
    public class Bi4App
    {
        // Método principal — interpreta o conteúdo do QR Code
        private const int TamanhoMaximoQRCode = 5000;
        public DadosBilheteIdentidade InterpretarQRCode(string conteudoQRCode)
        {
            try
            {
                string versaoDocumento;
                List<string> linhasProcessadas;

                ProcessarConteudoQRCode(conteudoQRCode, out versaoDocumento, out linhasProcessadas);

                var dados = new DadosBilheteIdentidade();

                switch (versaoDocumento)
                {
                    case VersoesBilheteIdentidade.Versao01:
                        var interpretador = new InterpretadorVersao01();
                        dados = interpretador.Interpretar(linhasProcessadas);
                        break;

                    default:
                        throw new ArgumentException("Versão do documento não suportada: " + versaoDocumento);
                }

                return dados;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro ao processar QR Code.", ex);
            }
        }

        // Método responsável por:
        // - Proteção contra QR Codes excessivamente grandes
        // - Remoção de caracteres inválidos
        // - Processamento das linhas do conteúdo
        // - Identificação da versão do documento
        private void ProcessarConteudoQRCode(string conteudoQRCode, out string versaoDocumento, out List<string> linhasProcessadas)
        {
            if (string.IsNullOrWhiteSpace(conteudoQRCode))
                throw new ArgumentException("QR Code vazio ou nulo.");

            // Proteção contra QR Codes demasiado grandes (DoS / consumo de memória)
            if (conteudoQRCode.Length > TamanhoMaximoQRCode)
                throw new ArgumentException("QR Code demasiado grande.");

            // Remover caracteres inválidos e ocultos
            conteudoQRCode = RemoverCaracteresInvalidos(conteudoQRCode);

            var linhas = conteudoQRCode
                .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None)
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrEmpty(l))
                .ToList();

            if (linhas.Count < 10)
                throw new ArgumentException("Formato inesperado. Não parece um Bilhete de Identidade.");

            // Última linha guarda a versão do documento
            versaoDocumento = linhas.Last().ToUpperInvariant();
            linhasProcessadas = linhas;
        }

        private static string RemoverCaracteresInvalidos(string texto)
        {
            return texto
                .Replace("\0", "")      // null byte
                .Replace("\u202E", "")  // Right-to-Left override
                .Replace("\u200B", "")  // zero-width space
                .Replace("\u200C", "")  // zero-width non-joiner
                .Replace("\u200D", ""); // zero-width joiner
        }
    }
}

