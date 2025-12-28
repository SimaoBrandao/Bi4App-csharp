# **Bi4App**

**Bi4App** é uma biblioteca .NET que interpreta o conteúdo do QR Code do **Bilhete de Identidade de Angola**, valida a estrutura dos dados e converte as informações em um objeto .NET tipado e normalizado — pronto para uso em aplicações web, desktop e mobile.

---

## ✅ Funcionalidade

A biblioteca:

* interpreta o texto extraído do QR Code
* valida estrutura e formato dos campos
* converte valores e normaliza conteúdo
* calcula atributos derivados (idade, validade etc.)
* retorna um objeto tipado pronto para integração

**Fluxo de processamento**

```
Entrada → texto bruto do QR Code
Saída   → objeto estruturado
```

Integração simples. Processamento local. Sem dependência externa.

---

## ⚠️ Escopo e Limitações

A Bi4App **não**:

* lê imagens ou QR Codes (a string deve ser obtida antes)
* valida autenticidade do documento
* executa verificações antifraude
* consulta serviços externos
* envia ou armazena dados

* ✔ Processamento é **100% local**
* ✔ Ideal para automação de cadastro e onboarding
* ✘ Não substitui validação legal do documento

---

## 🧩 Compatibilidade

Suporte a:

* .NET Framework 4.6.2+
* .NET Framework 4.8
* .NET Standard 2.0
* ASP.NET / Web API
* serviços backend
* aplicações desktop e server-side

Distribuição multi-target via NuGet.

---

## 📦 Instalação (NuGet)

```bash
dotnet add package Bi4App
```

Ou via Package Manager:

```powershell
Install-Package Bi4App
```

---

## 🧷 Exemplo de uso

```csharp
using Bi4App;

var bi = new Bi4App();
var dados = bi.InterpretarQRCode(textoQrCode);

Console.WriteLine(dados.NomeCompleto);
Console.WriteLine(dados.NumeroBilhete);
Console.WriteLine(dados.DataValidade);
Console.WriteLine(dados.Idade);
```

---

## 🧾 Estrutura de retorno

```csharp
public class BiResult
{
    public string NomeCompleto { get; set; }
    public string NumeroBilhete { get; set; }
    public string Provincia { get; set; }
    public string DataNascimento { get; set; }
    public string Sexo { get; set; }
    public string EstadoCivil { get; set; }
    public string DataEmissao { get; set; }
    public string DataValidade { get; set; }
    public string ProvinciaEmissora { get; set; }
    public string Versao { get; set; }
    public string PrimeiroNome { get; set; }
    public string UltimoNome { get; set; }

    // Campos derivados
    public int Idade { get; set; }
    public string EstadoValidade { get; set; }
    public string FaixaEtaria { get; set; }
    public string DiasAteExpiracao { get; set; }
}
```

Campos derivados são calculados automaticamente.

---

## ✔️ Validações executadas

* verificação de campos obrigatórios
* validação de estrutura e layout
* conversão segura de datas e números
* normalização de conteúdo textual
* cálculo de atributos derivados:

  * idade
  * faixa etária
  * estado de validade
  * dias até expiração

---

## 🖨️ Como obter o texto do QR Code

A biblioteca trabalha somente com a **string do QR Code**.

Você pode capturar o texto via:

* webcam / câmera
* scanner POS
* upload de imagem
* arquivo local

A escolha depende do ambiente de uso.

---

## 👍 Leitura via imagem (ZXing.NET)

```csharp
using ZXing;
using System.Drawing;

var reader = new BarcodeReader();

using var bitmap = (Bitmap)Image.FromFile("qr.png");
var qrText = reader.Decode(bitmap)?.Text;

if (!string.IsNullOrWhiteSpace(qrText))
{
    var bi = new Bi4App();
    var dados = bi.InterpretarQRCode(qrText);

    Console.WriteLine(dados.NomeCompleto);
}
```

Funciona com:

* webcam
* upload de imagem
* screenshots
* arquivos locais

---

## 🌐 Uso em aplicações Web

Fluxo recomendado:

1. decodificar o QR Code no frontend
2. enviar somente o texto para a API
3. processar com a Bi4App no backend

```javascript
const qrText = decodedQrResult;

await fetch("/api/bi/parse", {
  method: "POST",
  body: qrText
});
```

---

## 🔌 Integração rápida (Web API)

```csharp
app.MapPost("/api/bi/parse", (string qr) =>
{
    var bi = new Bi4App();
    return Results.Ok(bi.InterpretarQRCode(qr));
});
```

---

## 🧲 POS Scanner (Leitor físico)

Recomendado para:

* balcões de atendimento
* instituições públicas e bancárias
* operações de alto volume

Na maioria dos dispositivos, o texto já chega pronto:

```csharp
var bi = new Bi4App();
var dados = bi.InterpretarQRCode(qrCodeTextFromScanner);
```

Não requer biblioteca adicional.

---

## 🧠 Boas práticas de captura

* centralizar o QR Code
* evitar reflexos ou baixa iluminação
* validar a string recebida
* garantir leitura completa

---

## 🔗 Demo Online

Teste a demonstração:
(Se a câmara estiver a falhar, feche e volte a abrir a aplicação da câmara e tente novamente)

[https://bi4app-demo.vercel.app/](https://bi4app-demo.vercel.app/)

---

## 🧪 Projeto de exemplo de implementação

Repositórios:

[https://github.com/SimaoBrandao/Bi4AppWinForms.git](https://github.com/SimaoBrandao/Bi4AppWinForms.git)

---

## 📜 Licença — **LGPLv3**

A biblioteca é distribuída sob licença **LGPL-3.0**.

Isso significa:

* você pode usar em softwares proprietários
* aplicações podem permanecer fechadas
* desde que a biblioteca seja vinculada dinamicamente
* modificações na biblioteca devem ser disponibilizadas sob LGPLv3

📌 A biblioteca não pode ser fechada nem apropriada.

Alterações na biblioteca precisam ser públicas
quando distribuídas a terceiros.

---

## 🤝 Contribuições

Sugestões, melhorias e reports são bem-vindos.

Abra uma issue ou pull request no repositório:

[https://github.com/SimaoBrandao/Bi4App-csharp](https://github.com/SimaoBrandao/Bi4App-csharp)

---

## 📬 Contato

*  **Autor:** Simão Brandão
* **Email:** [sibrandao2008@gmail.com](mailto:sibrandao2008@gmail.com)
* **WhatsApp:** +244 948 493 828
* **GitHub:** https://github.com/SimaoBrandao
* **Linkedin:** https://linkedin.com/in/SimaoBrandao