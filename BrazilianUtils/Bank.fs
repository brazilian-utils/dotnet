module BrazilianUtils.Bank

open System

type BankInfo =
    { Code: string
      Ispb: string
      Name: string }

/// Participantes do STR (Sistema de Transferência de Reservas) com código
/// COMPE, publicados pelo Banco Central do Brasil.
let internal bancos =
    [ { Code = "001"; Ispb = "00000000"; Name = "Banco do Brasil S.A." }
      { Code = "003"; Ispb = "04902979"; Name = "BANCO DA AMAZONIA S.A." }
      { Code = "004"; Ispb = "07237373"; Name = "Banco do Nordeste do Brasil S.A." }
      { Code = "010"; Ispb = "81723108"; Name = "CREDICOAMO CREDITO RURAL COOPERATIVA" }
      { Code = "018"; Ispb = "57839805"; Name = "Banco Tricury S.A." }
      { Code = "021"; Ispb = "28127603"; Name = "BANESTES S.A. BANCO DO ESTADO DO ESPIRITO SANTO" }
      { Code = "033"; Ispb = "90400888"; Name = "BANCO SANTANDER (BRASIL) S.A." }
      { Code = "036"; Ispb = "06271464"; Name = "Banco Bradesco BBI S.A." }
      { Code = "037"; Ispb = "04913711"; Name = "Banco do Estado do Pará S.A." }
      { Code = "041"; Ispb = "92702067"; Name = "Banco do Estado do Rio Grande do Sul S.A." }
      { Code = "047"; Ispb = "13009717"; Name = "Banco do Estado de Sergipe S.A." }
      { Code = "063"; Ispb = "04184779"; Name = "Banco Bradescard S.A." }
      { Code = "065"; Ispb = "48795256"; Name = "Banco AndBank (Brasil) S.A." }
      { Code = "069"; Ispb = "61033106"; Name = "Banco Crefisa S.A." }
      { Code = "070"; Ispb = "00000208"; Name = "BRB - BANCO DE BRASILIA S.A." }
      { Code = "074"; Ispb = "03017677"; Name = "Banco J. Safra S.A." }
      { Code = "077"; Ispb = "00416968"; Name = "Banco Inter S.A." }
      { Code = "083"; Ispb = "10690848"; Name = "Banco da China Brasil S.A." }
      { Code = "084"; Ispb = "02398976"; Name = "SISPRIME DO BRASIL - COOPERATIVA DE CRÉDITO" }
      { Code = "085"; Ispb = "05463212"; Name = "Cooperativa Central de Crédito - Ailos" }
      { Code = "089"; Ispb = "62109566"; Name = "CREDISAN COOPERATIVA DE CRÉDITO" }
      { Code = "097"; Ispb = "04632856"; Name = "CREDISIS - CENTRAL DE COOPERATIVAS DE CRÉDITO" }
      { Code = "099"; Ispb = "03046391"; Name = "UNIPRIME CENTRAL NACIONAL - CENTRAL NACIONAL DE COOPERATIVA DE CREDITO" }
      { Code = "104"; Ispb = "00360305"; Name = "CAIXA ECONOMICA FEDERAL" }
      { Code = "107"; Ispb = "15114366"; Name = "Banco Bocom BBM S.A." }
      { Code = "120"; Ispb = "33603457"; Name = "BANCO RODOBENS S.A." }
      { Code = "122"; Ispb = "33147315"; Name = "Banco Bradesco BERJ S.A." }
      { Code = "124"; Ispb = "15357060"; Name = "Banco Woori Bank do Brasil S.A." }
      { Code = "125"; Ispb = "45246410"; Name = "BANCO GENIAL S.A." }
      { Code = "133"; Ispb = "10398952"; Name = "CONFEDERAÇÃO NACIONAL DAS COOPERATIVAS CENTRAIS DE CRÉDITO E ECONOMIA FAMILIAR E SOLIDÁRIA - CRESOL CONFEDERAÇÃO" }
      { Code = "136"; Ispb = "00315557"; Name = "COOPERATIVA CENTRAL DE CRÉDITO UNICRED DO BRASIL - UNICRED DO BRASIL" }
      { Code = "208"; Ispb = "30306294"; Name = "Banco BTG Pactual S.A." }
      { Code = "212"; Ispb = "92894922"; Name = "Banco Original S.A." }
      { Code = "213"; Ispb = "54403563"; Name = "Banco Arbi S.A." }
      { Code = "224"; Ispb = "58616418"; Name = "Banco Fibra S.A." }
      { Code = "237"; Ispb = "60746948"; Name = "Banco Bradesco S.A." }
      { Code = "241"; Ispb = "31597552"; Name = "BANCO CLASSICO S.A." }
      { Code = "249"; Ispb = "61182408"; Name = "Banco Investcred Unibanco S.A." }
      { Code = "254"; Ispb = "14388334"; Name = "PARANÁ BANCO S.A." }
      { Code = "265"; Ispb = "33644196"; Name = "Banco Fator S.A." }
      { Code = "266"; Ispb = "33132044"; Name = "BANCO CEDULA S.A." }
      { Code = "281"; Ispb = "76461557"; Name = "Cooperativa de Crédito Rural Coopavel" }
      { Code = "318"; Ispb = "61186680"; Name = "Banco BMG S.A." }
      { Code = "322"; Ispb = "01073966"; Name = "Cooperativa de Crédito Rural de Abelardo Luz - Sulcredi/Crediluz" }
      { Code = "341"; Ispb = "60701190"; Name = "ITAÚ UNIBANCO S.A." }
      { Code = "389"; Ispb = "17184037"; Name = "Banco Mercantil do Brasil S.A." }
      { Code = "394"; Ispb = "07207996"; Name = "Banco Bradesco Financiamentos S.A." }
      { Code = "421"; Ispb = "39343350"; Name = "LAR COOPERATIVA DE CRÉDITO - LAR CREDI" }
      { Code = "422"; Ispb = "58160789"; Name = "Banco Safra S.A." }
      { Code = "430"; Ispb = "00204963"; Name = "COOPERATIVA DE CREDITO RURAL SEARA - CREDISEARA" }
      { Code = "435"; Ispb = "38224857"; Name = "DELFINANCE SOCIEDADE DE CREDITO DIRETO S.A." }
      { Code = "456"; Ispb = "60498557"; Name = "Banco MUFG Brasil S.A." }
      { Code = "477"; Ispb = "33042953"; Name = "Citibank N.A." }
      { Code = "487"; Ispb = "62331228"; Name = "DEUTSCHE BANK S.A. - BANCO ALEMAO" }
      { Code = "505"; Ispb = "32062580"; Name = "BANCO UBS (BRASIL) S.A." }
      { Code = "511"; Ispb = "44683140"; Name = "MAGNUM SOCIEDADE DE CRÉDITO DIRETO S.A." }
      { Code = "600"; Ispb = "59118133"; Name = "Banco Luso Brasileiro S.A." }
      { Code = "604"; Ispb = "31895683"; Name = "Banco Industrial do Brasil S.A." }
      { Code = "611"; Ispb = "61820817"; Name = "Banco Paulista S.A." }
      { Code = "612"; Ispb = "31880826"; Name = "Banco Guanabara S.A." }
      { Code = "633"; Ispb = "68900810"; Name = "Banco Rendimento S.A." }
      { Code = "637"; Ispb = "60889128"; Name = "BANCO SOFISA S.A." }
      { Code = "643"; Ispb = "62144175"; Name = "Banco Pine S.A." }
      { Code = "707"; Ispb = "62232889"; Name = "Banco Daycoval S.A." }
      { Code = "743"; Ispb = "00795423"; Name = "Banco Semear S.A." }
      { Code = "745"; Ispb = "33479023"; Name = "Banco Citibank S.A." }
      { Code = "748"; Ispb = "01181521"; Name = "BANCO COOPERATIVO SICREDI S.A." }
      { Code = "752"; Ispb = "01522368"; Name = "Banco BNP Paribas Brasil S.A." }
      { Code = "753"; Ispb = "74828799"; Name = "Novo Banco Continental S.A. - Banco Múltiplo" }
      { Code = "756"; Ispb = "02038232"; Name = "BANCO COOPERATIVO SICOOB S.A. - BANCO SICOOB" }
      { Code = "757"; Ispb = "02318507"; Name = "BANCO KEB HANA DO BRASIL S.A." } ]

let private paraTexto (value: obj) : string =
    match value with
    | :? string as s -> s.Trim()
    | :? int as i -> string i
    | :? int64 as i -> string i
    | _ -> ""

/// Consulta um banco pelo código COMPE (3 dígitos, aceita sem zeros à
/// esquerda ou como número).
let GetByCode (code: obj) : BankInfo option =
    let texto = paraTexto code
    match Int32.TryParse(texto) with
    | false, _ -> None
    | true, numero -> bancos |> List.tryFind (fun b -> int b.Code = numero)

/// Consulta um banco pelo ISPB (8 dígitos, com ou sem zeros à esquerda).
let GetByIspb (value: obj) : BankInfo option =
    let texto = paraTexto value
    match Int32.TryParse(texto) with
    | false, _ -> None
    | true, numero -> bancos |> List.tryFind (fun b -> int b.Ispb = numero)

/// Retorna todos os bancos com código COMPE publicados pelo Banco Central.
let List () : BankInfo list = bancos
