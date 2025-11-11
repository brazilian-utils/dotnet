module BrazilianUtils.VoterId

open System

/// <summary>
/// Check if a Brazilian voter id number is valid.
/// It does not verify if the voter id actually exists.
///
/// References:
/// - https://pt.wikipedia.org/wiki/T%C3%ADtulo_de_eleitor
/// - http://clubes.obmep.org.br/blog/a-matematica-nos-documentos-titulo-de-eleitor/
/// </summary>
/// <param name="voterId">String representing the voter id to be verified.</param>
/// <returns>True if the voter id is valid. False otherwise.</returns>
let isValid (voterId: string) : bool =
    
    let getSequentialNumber (id: string) : string =
        id.[..7]
    
    let getFederativeUnion (id: string) : string =
        id.[id.Length - 4..id.Length - 3]
    
    let getVerifyingDigits (id: string) : string =
        id.[id.Length - 2..]
    
    let isFederativeUnionValid (federativeUnion: string) : bool =
        let validUnions = [1..28] |> List.map (sprintf "%02d")
        List.contains federativeUnion validUnions
    
    let isLengthValid (id: string) : bool =
        let fu = getFederativeUnion id
        match id.Length with
        | 12 -> true
        | 13 -> fu = "01" || fu = "02"  // SP and MG edge case
        | _ -> false
    
    let calculateVd1 (sequentialNumber: string) (federativeUnion: string) : int =
        let weights = [2; 3; 4; 5; 6; 7; 8; 9]
        let sum = 
            sequentialNumber
            |> Seq.take 8
            |> Seq.map (string >> int)
            |> Seq.zip weights
            |> Seq.sumBy (fun (w, d) -> w * d)
        
        let rest = sum % 11
        match rest with
        | 0 when federativeUnion = "01" || federativeUnion = "02" -> 1
        | 10 -> 0
        | _ -> rest
    
    let calculateVd2 (federativeUnion: string) (vd1: int) : int =
        let weights = [7; 8; 9]
        let digits = [int (string federativeUnion.[0]); int (string federativeUnion.[1]); vd1]
        
        let sum = 
            List.zip weights digits
            |> List.sumBy (fun (w, d) -> w * d)
        
        let rest = sum % 11
        match rest with
        | 0 when federativeUnion = "01" || federativeUnion = "02" -> 1
        | 10 -> 0
        | _ -> rest
    
    // Main validation logic
    if isNull voterId || String.IsNullOrWhiteSpace(voterId) then
        false
    elif not (voterId |> Seq.forall Char.IsDigit) then
        false
    elif not (isLengthValid voterId) then
        false
    else
        let sequentialNumber = getSequentialNumber voterId
        let federativeUnion = getFederativeUnion voterId
        let verifyingDigits = getVerifyingDigits voterId
        
        if not (isFederativeUnionValid federativeUnion) then
            false
        else
            let vd1 = calculateVd1 sequentialNumber federativeUnion
            let vd2 = calculateVd2 federativeUnion vd1
            
            vd1 = int (string verifyingDigits.[0]) && 
            vd2 = int (string verifyingDigits.[1])


/// <summary>
/// Generates a random valid Brazilian voter registration.
/// </summary>
/// <param name="federativeUnion">Federative union for the voter id that will be generated. 
/// The default value "ZZ" is used for voter IDs issued to foreigners.</param>
/// <returns>A randomly generated valid voter ID for the given federative union, or None if invalid.</returns>
let generate (federativeUnion: string) : string option =
    let ufs = 
        Map.ofList [
            "SP", "01"; "MG", "02"; "RJ", "03"; "RS", "04";
            "BA", "05"; "PR", "06"; "CE", "07"; "PE", "08";
            "SC", "09"; "GO", "10"; "MA", "11"; "PB", "12";
            "PA", "13"; "ES", "14"; "PI", "15"; "RN", "16";
            "AL", "17"; "MT", "18"; "MS", "19"; "DF", "20";
            "SE", "21"; "AM", "22"; "RO", "23"; "AC", "24";
            "AP", "25"; "RR", "26"; "TO", "27"; "ZZ", "28"
        ]
    
    let isFederativeUnionValid (fu: string) : bool =
        let validUnions = [1..28] |> List.map (sprintf "%02d")
        List.contains fu validUnions
    
    let calculateVd1 (sequentialNumber: string) (fu: string) : int =
        let weights = [2; 3; 4; 5; 6; 7; 8; 9]
        let sum = 
            sequentialNumber
            |> Seq.take 8
            |> Seq.map (string >> int)
            |> Seq.zip weights
            |> Seq.sumBy (fun (w, d) -> w * d)
        
        let rest = sum % 11
        match rest with
        | 0 when fu = "01" || fu = "02" -> 1
        | 10 -> 0
        | _ -> rest
    
    let calculateVd2 (fu: string) (vd1: int) : int =
        let weights = [7; 8; 9]
        let digits = [int (string fu.[0]); int (string fu.[1]); vd1]
        
        let sum = 
            List.zip weights digits
            |> List.sumBy (fun (w, d) -> w * d)
        
        let rest = sum % 11
        match rest with
        | 0 when fu = "01" || fu = "02" -> 1
        | 10 -> 0
        | _ -> rest
    
    let ufUpper = federativeUnion.ToUpper()
    match Map.tryFind ufUpper ufs with
    | Some ufNumber when isFederativeUnionValid ufNumber ->
        let random = Random()
        let sequentialNumber = random.Next(0, 100000000).ToString().PadLeft(8, '0')
        let vd1 = calculateVd1 sequentialNumber ufNumber
        let vd2 = calculateVd2 ufNumber vd1
        Some (sprintf "%s%s%d%d" sequentialNumber ufNumber vd1 vd2)
    | _ -> None


/// <summary>
/// Format a voter ID for display with visual spaces.
///
/// This function takes a numeric voter ID string as input and adds standard
/// formatting for display purposes.
/// </summary>
/// <param name="voterId">A numeric voter ID string.</param>
/// <returns>A formatted voter ID string with standard visual spacing, or None if the input is invalid.</returns>
/// <example>
/// <code>
/// formatVoterId "690847092828" // Returns: Some "6908 4709 28 28"
/// formatVoterId "163204010922" // Returns: Some "1632 0401 09 22"
/// </code>
/// </example>
let formatVoterId (voterId: string) : string option =
    if not (isValid voterId) then
        None
    else
        sprintf "%s %s %s %s" 
            voterId.[..3]
            voterId.[4..7]
            voterId.[8..9]
            voterId.[10..11]
        |> Some
