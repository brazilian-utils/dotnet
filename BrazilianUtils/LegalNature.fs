module BrazilianUtils.LegalNature

open System
open System.Text

type LegalNatureCategory =
    { Code: string
      Description: string }

type LegalNatureInfo =
    { Code: string
      Description: string
      Category: LegalNatureCategory
      Legacy: bool
      CurrentCode: string option }

let private categorias =
    [ "1", "Administração Pública"
      "2", "Entidades Empresariais"
      "3", "Entidades sem Fins Lucrativos"
      "4", "Pessoas Físicas"
      "5", "Organizações Internacionais e Outras Instituições Extraterritoriais" ]
    |> Map.ofList

let private categoria (codigo: string) : LegalNatureCategory =
    { Code = codigo
      Description = categorias |> Map.tryFind codigo |> Option.defaultValue "" }

// Tabela CONCLA Natureza Jurídica 2021 — os 92 códigos em vigor.
let private emVigor =
    [ "1015", "Órgão Público do Poder Executivo Federal"
      "1023", "Órgão Público do Poder Executivo Estadual ou do Distrito Federal"
      "1031", "Órgão Público do Poder Executivo Municipal"
      "1040", "Órgão Público do Poder Legislativo Federal"
      "1058", "Órgão Público do Poder Legislativo Estadual ou do Distrito Federal"
      "1066", "Órgão Público do Poder Legislativo Municipal"
      "1074", "Órgão Público do Poder Judiciário Federal"
      "1082", "Órgão Público do Poder Judiciário Estadual"
      "1104", "Autarquia Federal"
      "1112", "Autarquia Estadual ou do Distrito Federal"
      "1120", "Autarquia Municipal"
      "1139", "Fundação Pública de Direito Público Federal"
      "1147", "Fundação Pública de Direito Público Estadual ou do Distrito Federal"
      "1155", "Fundação Pública de Direito Público Municipal"
      "1163", "Órgão Público Autônomo Federal"
      "1171", "Órgão Público Autônomo Estadual ou do Distrito Federal"
      "1180", "Órgão Público Autônomo Municipal"
      "1198", "Comissão Polinacional"
      "1210", "Consórcio Público de Direito Público (Associação Pública)"
      "1228", "Consórcio Público de Direito Privado"
      "1236", "Estado ou Distrito Federal"
      "1244", "Município"
      "1252", "Fundação Pública de Direito Privado Federal"
      "1260", "Fundação Pública de Direito Privado Estadual ou do Distrito Federal"
      "1279", "Fundação Pública de Direito Privado Municipal"
      "1287", "Fundo Público da Administração Indireta Federal"
      "1295", "Fundo Público da Administração Indireta Estadual ou do Distrito Federal"
      "1309", "Fundo Público da Administração Indireta Municipal"
      "1317", "Fundo Público da Administração Direta Federal"
      "1325", "Fundo Público da Administração Direta Estadual ou do Distrito Federal"
      "1333", "Fundo Público da Administração Direta Municipal"
      "1341", "União"
      "2011", "Empresa Pública"
      "2038", "Sociedade de Economia Mista"
      "2046", "Sociedade Anônima Aberta"
      "2054", "Sociedade Anônima Fechada"
      "2062", "Sociedade Empresária Limitada"
      "2070", "Sociedade Empresária em Nome Coletivo"
      "2089", "Sociedade Empresária em Comandita Simples"
      "2097", "Sociedade Empresária em Comandita por Ações"
      "2127", "Sociedade em Conta de Participação"
      "2135", "Empresário (Individual)"
      "2143", "Cooperativa"
      "2151", "Consórcio de Sociedades"
      "2160", "Grupo de Sociedades"
      "2178", "Estabelecimento, no Brasil, de Sociedade Estrangeira"
      "2194", "Estabelecimento, no Brasil, de Empresa Binacional Argentino-Brasileira"
      "2216", "Empresa Domiciliada no Exterior"
      "2224", "Clube/Fundo de Investimento"
      "2232", "Sociedade Simples Pura"
      "2240", "Sociedade Simples Limitada"
      "2259", "Sociedade Simples em Nome Coletivo"
      "2267", "Sociedade Simples em Comandita Simples"
      "2275", "Empresa Binacional"
      "2283", "Consórcio de Empregadores"
      "2291", "Consórcio Simples"
      "2305", "Empresa Individual de Responsabilidade Limitada (de Natureza Empresária)"
      "2313", "Empresa Individual de Responsabilidade Limitada (de Natureza Simples)"
      "2321", "Sociedade Unipessoal de Advogados"
      "2330", "Cooperativas de Consumo"
      "2348", "Empresa Simples de Inovação - Inova Simples"
      "2356", "Investidor Não Residente"
      "3034", "Serviço Notarial e Registral (Cartório)"
      "3069", "Fundação Privada"
      "3077", "Serviço Social Autônomo"
      "3085", "Condomínio Edilício"
      "3107", "Comissão de Conciliação Prévia"
      "3115", "Entidade de Mediação e Arbitragem"
      "3131", "Entidade Sindical"
      "3204", "Estabelecimento, no Brasil, de Fundação ou Associação Estrangeiras"
      "3212", "Fundação ou Associação Domiciliada no Exterior"
      "3220", "Organização Religiosa"
      "3239", "Comunidade Indígena"
      "3247", "Fundo Privado"
      "3255", "Órgão de Direção Nacional de Partido Político"
      "3263", "Órgão de Direção Regional de Partido Político"
      "3271", "Órgão de Direção Local de Partido Político"
      "3280", "Comitê Financeiro de Partido Político"
      "3298", "Frente Plebiscitária ou Referendária"
      "3301", "Organização Social (OS)"
      "3310", "Demais Condomínios"
      "3328", "Plano de Benefícios de Previdência Complementar Fechada"
      "3999", "Associação Privada"
      "4014", "Empresa Individual Imobiliária"
      "4022", "Segurado Especial"
      "4081", "Contribuinte individual"
      "4090", "Candidato a Cargo Político Eletivo"
      "4111", "Leiloeiro"
      "4120", "Produtor Rural (Pessoa Física)"
      "5010", "Organização Internacional"
      "5029", "Representação Diplomática Estrangeira"
      "5037", "Outras Instituições Extraterritoriais" ]
    |> List.map (fun (codigo, descricao) ->
        { Code = codigo
          Description = descricao
          Category = categoria (codigo.Substring(0, 1))
          Legacy = false
          CurrentCode = None })

// Códigos aposentados por revisões anteriores (best-effort, ver
// contract-data/README.md).
let private aposentados =
    [ "2208", "Entidade Binacional Itaipu", Some "2275"
      "2100", "Sociedade Mercantil de Capital e Indústria", None
      "3042", "Organização Social", Some "3301"
      "3050", "Organização da Sociedade Civil de Interesse Público (Oscip)", None
      "3093", "Unidade Executora (Programa Dinheiro Direto na Escola)", Some "3999"
      "3123", "Partido Político", None ]
    |> List.map (fun (codigo, descricao, codigoAtual) ->
        { Code = codigo
          Description = descricao
          Category = categoria (codigo.Substring(0, 1))
          Legacy = true
          CurrentCode = codigoAtual })

let private todos = emVigor @ aposentados

/// Extrai apenas os dígitos de um código de natureza jurídica, tolerando
/// hífen, ponto e espaço, e limita a 4 dígitos.
let private normalizarCodigo (value: string) : string =
    if isNull value then
        ""
    else
        let apenasDigitos = value |> Seq.filter Char.IsDigit |> Seq.toArray |> String
        if apenasDigitos.Length > 4 then apenasDigitos.Substring(0, 4) else apenasDigitos

/// Formata um código de natureza jurídica no padrão `NNN-N`, aplicando a
/// máscara até onde os dígitos alcançarem.
///
/// Examples:
///     Format "2062" = "206-2"
let Format (value: string) : string =
    let apenasDigitos = value |> Seq.filter Char.IsDigit |> Seq.toArray |> String
    let sb = StringBuilder(apenasDigitos)
    if sb.Length > 3 then sb.Insert(3, "-") |> ignore
    sb.ToString()

/// Remove a formatação e mantém apenas dígitos, limitado a 4 dígitos.
let Parse (value: string) : string = normalizarCodigo value

/// Consulta um código de natureza jurídica na tabela CONCLA Natureza
/// Jurídica 2021; devolve None para um código desconhecido.
let Get (value: string) : LegalNatureInfo option =
    let codigo = normalizarCodigo value
    if codigo.Length <> 4 then None
    else todos |> List.tryFind (fun n -> n.Code = codigo)

/// Descrição de um código de natureza jurídica.
let GetDescription (code: string) : string option =
    Get code |> Option.map (fun n -> n.Description)

/// Verifica se um código de natureza jurídica existe na tabela (em vigor ou
/// aposentado).
let IsValid (code: string) : bool = Get code |> Option.isSome

/// Retorna toda a natureza jurídica de uma categoria CONCLA (o primeiro
/// dígito do código), ordenada pelo código.
let ListByCategory (category: obj) (incluirAposentados: bool option) : LegalNatureInfo list =
    let codigoCategoria =
        match category with
        | :? string as s -> s.Trim()
        | :? int as i -> string i
        | _ -> ""
    if codigoCategoria.Length <> 1 || not (Char.IsDigit codigoCategoria.[0]) then
        []
    else
        let incluir = defaultArg incluirAposentados false
        let base' = if incluir then todos else emVigor
        base'
        |> List.filter (fun n -> n.Code.StartsWith(codigoCategoria))
        |> List.sortBy (fun n -> n.Code)

/// Gera um código de natureza jurídica aleatório, sorteado apenas entre os
/// 92 códigos em vigor.
let Generate () : string =
    let random = Random()
    emVigor.[random.Next(emVigor.Length)].Code

/// Retorna a tabela de natureza jurídica como um mapa código -> descrição.
/// Por padrão, somente os 92 códigos em vigor; `incluirAposentados` inclui
/// também os aposentados.
let List (incluirAposentados: bool option) : Map<string, string> =
    let incluir = defaultArg incluirAposentados false
    let base' = if incluir then todos else emVigor
    base' |> List.map (fun n -> n.Code, n.Description) |> Map.ofList
